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

    public int Lives = 3;
    public float SurviveTime = 60f;

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

    private void Start()
    {
        Invoke("CheckState", SurviveTime);
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

    public void CheckState()
    {
        if(Lives > 0)
        {
            ChangeState(States.Win);
        }
    }

    internal void ChangeLives(int tmp)
    {
        Lives = tmp;
        LivesChanged.Invoke(tmp);
        if (Lives == 0)
        {
            CancelInvoke("CheckState");
            ChangeState(States.Loose);
        }
    }
}
