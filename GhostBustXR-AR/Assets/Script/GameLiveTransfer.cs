using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using UnityEngine;

public class GameLiveTransfer : MonoBehaviour, IPunObservable
{
    private GameState _state;
    private int _lastLives;

    void Start()
    {
        _state = FindObjectOfType<GameState>();
    }

    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if (stream.IsWriting)
        {
            //fine this way?
        }
        else if (stream.IsReading)
        {
            var tmp = (int)stream.ReceiveNext();
            if(_lastLives != tmp)
            {
                _lastLives = tmp;
                _state.ChangeLives(tmp);
            }
        }
    }

}
