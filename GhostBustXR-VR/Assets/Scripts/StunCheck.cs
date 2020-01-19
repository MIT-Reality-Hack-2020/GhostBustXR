using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using UnityEngine;
using UnityEngine.Events;

public class StunCheck : MonoBehaviour, IPunObservable
{
    private bool _isStunned = false;
    public float StunCooldown = 2F;
    public float PushbackDistance = 1f;
    public UnityEvent Stunned;
    public UnityEvent Released;
    public int StunnerLayer = 8;

    //private void OnCollision(Collision other)
    //{
    //    if (other.gameObject.layer != StunnerLayer) return;
    //    var old = _isStunned;
    //    _isStunned = true;
    //    if (!old && _isStunned)
    //    {
    //        Stun(other.transform.forward, PushbackDistance, StunCooldown);
    //    }
    //}

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer != StunnerLayer) return;
        var old = _isStunned;
        _isStunned = true;
        if (!old && _isStunned)
        {
            Stun(other.transform.forward, PushbackDistance, StunCooldown);
        }
    }

    public void Stun(Vector3 stunDirection, float pushbackDistance, float stunCooldown)
    {
        Stunned.Invoke();
        transform.localPosition = transform.localPosition + Vector3.Scale(stunDirection, new Vector3(1f, 0f, 1f)).normalized * pushbackDistance;
        Invoke("Release", stunCooldown);
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
