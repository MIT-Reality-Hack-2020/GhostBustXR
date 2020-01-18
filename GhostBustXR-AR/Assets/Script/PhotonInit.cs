using System.Collections;
using System.Collections.Generic;
using Microsoft.MixedReality.Toolkit.Utilities;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine;
using UnityEngine.Events;

public class PhotonInit : MonoBehaviourPunCallbacks
{
    public GameObject PlayPrefab;

    public UnityEvent ConnectedToPhoton;

    public UnityEvent PhotonFailed;

    // Start is called before the first frame update
    void Start()
    {
        PhotonNetwork.ConnectUsingSettings();
    }

    public override void OnConnectedToMaster()
    {
        base.OnConnectedToMaster();

        var roomOptions = new RoomOptions
        {
            EmptyRoomTtl = 60 * 1000 //60 seconds
        };
        //only one room for now.. would be interesting for later
        PhotonNetwork.JoinOrCreateRoom("defaultRoom", roomOptions, null);
    }

    public override void OnJoinRoomFailed(short returnCode, string message)
    {
        base.OnJoinRoomFailed(returnCode, message);
        Debug.LogError(message);
        PhotonFailed.Invoke();
    }

    public async override void OnJoinedRoom()
    {
        base.OnJoinedRoom();

        var halo = PhotonNetwork.Instantiate(PlayPrefab.name, Vector3.zero, Quaternion.identity);
        halo.transform.SetParent(CameraCache.Main.transform);
        halo.transform.localPosition = Vector3.zero;
        halo.transform.localRotation = Quaternion.identity;
        ConnectedToPhoton.Invoke();
    }
}
