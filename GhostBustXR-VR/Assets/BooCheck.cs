using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class BooCheck : MonoBehaviour
{

    public UnityEvent DidBoo;
    public float BooTimeout = 1f;
    private bool _canBoo = true;
    public int PlayerLayer = 9;
    public float PushbackDistance = 3;
    public float StunCooldown = 2;

    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject.layer != PlayerLayer) return;
        if (!_canBoo) return;
        Invoke("Reset", BooTimeout);
        _canBoo = false;
        DidBoo.Invoke();
        GetComponent<StunCheck>().Stun(Vector3.Scale(transform.position - other.transform.position, new Vector3(1f, 0f, 1f)).normalized, PushbackDistance, StunCooldown);
        GameState.Instance.ChangeLives(GameState.Instance.Lives--);
    }

    private void Reset()
    {
        _canBoo = true;
    }

}
