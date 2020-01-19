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
    public int StunnerLayer = 8;

    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("OnTriggerEnter");
        if (other.gameObject.layer != StunnerLayer) return;
        Debug.Log("OnTriggerEnter_After");
        //    var old = _isStunned;
        //  _isStunned = true;
        //if (!old && _isStunned)
        {
            Stun(-transform.forward, PushbackDistance, StunCooldown);
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
            var before = _isStunned;
            _isStunned = (bool)stream.ReceiveNext();



            //events only for local clients
            if (!GetComponent<PhotonView>().IsMine) return;
            if (_isStunned && !before)
            {
                Stunned.Invoke();
            }
            else if (!_isStunned && before)
            {
                Released.Invoke();
            }
        }
    }
}
