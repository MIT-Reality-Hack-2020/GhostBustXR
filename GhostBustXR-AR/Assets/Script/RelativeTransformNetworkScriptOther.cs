using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using UnityEngine;

public class RelativeTransformNetworkScriptOther : MonoBehaviour, IPunObservable
{

    public float PositionLerpSpeed = 0.7F;
    public float RotationLerpSpeed = 0.7F;
    public Vector3 ScaleMovement = new Vector3(1f, 1f, 1f);

    [Tooltip("Obj Name to move relative to.. this or RootObj needs to be set")]
    public string RootObjName;

    [Tooltip("Obj to move relative to.. this or RootObjName needs to be set")]
    public GameObject RootObj;

    [Tooltip("Object which should be followed (if null this one will be used)")]
    public Transform TransformToMove;

    public void Awake()
    {
        if (TransformToMove) return;
        TransformToMove = transform;
    }

    Vector3 RelativePosition
    {
        get => RelativeGameObject.transform.InverseTransformPoint(TransformToMove.position);
        set => TransformToMove.position = Vector3.Lerp(TransformToMove.position, RelativeGameObject.transform.TransformPoint(Vector3.Scale(value, ScaleMovement)), PositionLerpSpeed);
    }

    Quaternion RelativeRotation
    {
        get => (Quaternion.Inverse(RelativeGameObject.transform.rotation) * TransformToMove.rotation);
        set => TransformToMove.rotation = Quaternion.Lerp(TransformToMove.rotation, RelativeGameObject.transform.rotation * value, RotationLerpSpeed);
    }

    GameObject RelativeGameObject
    {
        get
        {
            if (RootObj == null)
            {
                RootObj = GameObject.Find(RootObjName);
            }
            return (RootObj);
        }
    }

    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if (stream.IsWriting)
        {
            stream.SendNext(RelativePosition);
            stream.SendNext(RelativeRotation);
        }
        else
        {
            RelativePosition = (Vector3)stream.ReceiveNext();
            RelativeRotation = (Quaternion)stream.ReceiveNext();
        }
    }
}
