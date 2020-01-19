using System;
using System.Linq;
using System.Threading.Tasks;
using ExitGames.Client.Photon;
using Microsoft.Azure.SpatialAnchors;
using Microsoft.Azure.SpatialAnchors.Unity;
using Microsoft.MixedReality.Toolkit.Input;
using Microsoft.MixedReality.Toolkit.Utilities;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine;
using UnityEngine.Events;

public class AnchorInit : MonoBehaviour
{
    [Serializable]
    public class ProgressEvent : UnityEvent<float> { }

    public SpatialAnchorManager ASAManager;
    public static readonly string ANCHOR_ID_PROPERTY = "THIS_IS_ANCHORRR";
    private CloudSpatialAnchorWatcher _watcher;
    public ProgressEvent AnchorProgressUpdated;
    public UnityEvent AnchorSaved;
    public UnityEvent AnchorFound;
    public UnityEvent AnchorLoading;
    public UnityEvent SessionReady;
    private bool _anchorReceived = false;
    private string _myAnchorID = String.Empty;

    // Start is called before the first frame update
    void Start()
    {
        ASAManager.AnchorLocated += ASAManager_AnchorLocated;
        ASAManager.SessionStarted += ASAManager_SessionStarted;
        ASAManager.Error += (sender, args) =>
        {
            Debug.LogError("ASA MANAGER ERROR:");
            Debug.LogError(args.ErrorMessage);
        };
    }

    private void ASAManager_SessionStarted(object sender, EventArgs e)
    {
        //hmm?
    }

    private void ASAManager_AnchorLocated(object sender, AnchorLocatedEventArgs args)
    {
        if (args.Status == LocateAnchorStatus.Located)
        {
            var anchor = args.Anchor;
            UnityDispatcher.InvokeOnAppThread(() =>
            {
                var cna = GetComponent<CloudNativeAnchor>();
                if (!cna)
                {
                    Debug.Log("Adding CloudNativeAnchor");
                    cna = gameObject.AddComponent<CloudNativeAnchor>();
                }
                cna.CloudToNative(anchor);
                AnchorFound.Invoke();
            });
        }
        else if (args.Status == LocateAnchorStatus.NotLocatedAnchorDoesNotExist || args.Status == LocateAnchorStatus.NotLocated)
        {
            Debug.LogError("Anchor not Found");
        }
    }

    public void StartInitialize()
    {
        UnityDispatcher.InvokeOnAppThread(async () =>
        {
            await StartASAsync();
        });
    }

    private async Task StartASAsync()
    {
        try
        {
            Debug.Log("Creating Session");
            await ASAManager.CreateSessionAsync();
            Debug.Log("Starting Session");
            await ASAManager.StartSessionAsync();
            Debug.Log("Session Ready");
            SessionReady.Invoke();
        }
        catch (Exception e)
        {
            Debug.LogError("Sessions SetUp failed");
            Debug.LogException(e);
        }
    }

    public void CreateAnchor()
    {
        UnityDispatcher.InvokeOnAppThread(async () =>
        {
            await SaveCurrentObjectAnchorToCloudAsync();
        });
    }

    protected virtual async Task SaveCurrentObjectAnchorToCloudAsync()
    {
        var cna = gameObject.GetComponent<CloudNativeAnchor>();
        if (!cna)
        {
            cna = gameObject.AddComponent<CloudNativeAnchor>();
        }

        if (cna.CloudAnchor == null) { cna.NativeToCloud(); }
        var cloudAnchor = cna.CloudAnchor;

        cloudAnchor.Expiration = DateTimeOffset.Now.AddDays(7);

        while (!ASAManager.IsReadyForCreate && ASAManager.SessionStatus.RecommendedForCreateProgress < 1F)
        {
            await Task.Delay(330);
            var createProgress = ASAManager.SessionStatus.RecommendedForCreateProgress;
            if (_anchorReceived) return;
            UnityDispatcher.InvokeOnAppThread(() =>
            {
                AnchorProgressUpdated.Invoke(createProgress);
            });
        }

        try
        {
            await ASAManager.CreateAnchorAsync(cloudAnchor);
            var success = cloudAnchor != null;
            if (success && !_anchorReceived)
            {
                _myAnchorID = cloudAnchor.Identifier;
                var props = new Hashtable()
                    {
                        { ANCHOR_ID_PROPERTY, cloudAnchor.Identifier }
                    };
                PhotonNetwork.CurrentRoom.SetCustomProperties(props);
                AnchorSaved.Invoke();
            }
            else
            {
                Debug.LogError(new Exception("Failed to save, but no exception was thrown."));
            }
        }
        catch (Exception ex)
        {
            Debug.LogError(ex);
        }
    }

    public async void LoadAnchor()
    {
        await SearchAnchorAsync();
    }

    async Task SearchAnchorAsync()
    {
        if (PhotonNetwork.CurrentRoom.CustomProperties.TryGetValue(ANCHOR_ID_PROPERTY, out var keyValue))
        {
            if (String.Equals(keyValue, _myAnchorID)) return;
            AnchorLoading.Invoke();
            var anchorLocateCriteria = new AnchorLocateCriteria
            {
                Identifiers = new[] { (string)keyValue }
            };
            _anchorReceived = true;
            // If we didn't create the room then we want to try and get the anchor
            // from the cloud and apply it.
            _watcher = ASAManager.Session.CreateWatcher(anchorLocateCriteria);
            //handler => CloudManager_AnchorLocated
        }
    }

    private void OnDestroy()
    {
        if (ASAManager != null)
        {
            ASAManager.StopSession();
        }

        if (_watcher != null)
        {
            _watcher.Stop();
            _watcher = null;
        }
    }
}
