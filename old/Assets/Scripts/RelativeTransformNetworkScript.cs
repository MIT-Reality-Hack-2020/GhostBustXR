using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using UnityEngine;

public class RelativeTransformNetworkScript : MonoBehaviour, IPunObservable
{

    public float PositionLerpSpeed = 0.7F;
    public float RotationLerpSpeed = 0.7F;

    public string RootObjName;

    private GameObject _relativeGameObject;

    private Transform _myTransform;

    public void Awake() => _myTransform = transform;

    Vector3 RelativePosition
    {
        get => RelativeGameObject.transform.InverseTransformPoint(_myTransform.position);
        set => _myTransform.position = Vector3.Lerp(_myTransform.position, RelativeGameObject.transform.TransformPoint(value), PositionLerpSpeed);
    }

    Quaternion RelativeRotation
    {
        get => (Quaternion.Inverse(RelativeGameObject.transform.rotation) * _myTransform.rotation);
        set => _myTransform.rotation = Quaternion.Lerp(_myTransform.rotation, RelativeGameObject.transform.rotation * value, RotationLerpSpeed);
    }

    GameObject RelativeGameObject
    {
        get
        {
            if (_relativeGameObject == null)
            {
                _relativeGameObject = GameObject.Find(RootObjName);
            }
            return (_relativeGameObject);
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
