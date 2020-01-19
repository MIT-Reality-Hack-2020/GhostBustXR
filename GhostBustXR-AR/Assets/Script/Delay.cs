using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Delay : MonoBehaviour
{
    public UnityEvent Fire;

    public float DelaySeconds = 3f;

    // Start is called before the first frame update
    void Start()
    {
        Invoke("Done", DelaySeconds);
    }

    public void Done()
    {
        Fire.Invoke();
    }
}
