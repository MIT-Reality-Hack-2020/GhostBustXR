using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class GameState : MonoBehaviour
{
    public class LivesChangedEvent : UnityEvent<int> { }

    [Serializable]
    public enum States
    {
        StartUp,
        Running,
        Win,
        Loose
    }

    private int _lives = 3;

    public States CurrentState = States.StartUp;

    public static GameState Instance;

    public LivesChangedEvent LivesChanged;
    public UnityEvent StateChangedToRunning;
    public UnityEvent Lost;
    public UnityEvent Won;
    public UnityEvent GameStartUp;

    private void Awake()
    {
        Instance = this;
    }

    public void ChangeState(States state)
    {
        switch (state)
        {
            case States.StartUp:
                GameStartUp.Invoke();
                break;
            case States.Running:
                StateChangedToRunning.Invoke();
                break;
            case States.Win:
                Won.Invoke();
                break;
            case States.Loose:
                Lost.Invoke();
                break;
        }
    }

    public void Win()
    {
        ChangeState(States.Win);
    }

    internal void ChangeLives(int tmp)
    {
        _lives = tmp;
        LivesChanged.Invoke(tmp);
    }
}
