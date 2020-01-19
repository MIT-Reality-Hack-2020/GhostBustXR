using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using UnityEngine;

public class GameStateTransfer : MonoBehaviour, IPunObservable
{
    private GameState _state;
    private GameState.States _lastState;

    void Start()
    {
        _state = FindObjectOfType<GameState>();
    }

    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if (stream.IsWriting)
        {
            if (_lastState != _state.CurrentState)
            {
                _lastState = _state.CurrentState;
                stream.SendNext((int)_lastState);
            }
        }
        else if (stream.IsReading)
        {
            var tmp = (GameState.States)stream.ReceiveNext();
            if(_lastState != tmp)
            {
                _lastState = tmp;
                _state.ChangeState(tmp);
            }
        }
    }

}
