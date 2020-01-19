using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using UnityEngine;
using UnityEngine.Events;

public class StunCheck : MonoBehaviour, IPunObservable
{
    private bool _isStunned = true;
    public float StunCooldown = 2F;
    public float PushbackDistance = 1f;
    public UnityEvent Stunned;
    public UnityEvent Released;

    private void OnTriggerStay(Collider other)
    {
        _isStunned = true;
        Stunned.Invoke();
        transform.localPosition = transform.localPosition + Vector3.Scale(other.transform.forward, new Vector3(1f, 0f, 1f)).normalized * PushbackDistance;
        Invoke("Release", StunCooldown);
    }

    public void Release()
    {
        _isStunned = false;
        Released.Invoke();
    }

    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if (stream.IsWriting)
        {
            stream.SendNext(_isStunned);
        }
        else
        {
            _isStunned = (bool)stream.ReceiveNext();
        }
    }
}
