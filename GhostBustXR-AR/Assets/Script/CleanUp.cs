using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.ARFoundation;

public class CleanUp : MonoBehaviour
{
    private void Start()
    {
        Screen.sleepTimeout = SleepTimeout.NeverSleep;
    }

    public void RemoveAllARPlanes()
    {
        foreach (var item in FindObjectsOfType<ARPlane>())
        {
            Destroy(item.gameObject);
        }
    }

}
