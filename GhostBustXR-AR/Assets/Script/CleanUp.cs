using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.ARFoundation;

public class CleanUp : MonoBehaviour
{
    public void RemoveAllARPlanes()
    {
        foreach (var item in FindObjectsOfType<ARPlane>())
        {
            Destroy(item.gameObject);
        }
    }

}
