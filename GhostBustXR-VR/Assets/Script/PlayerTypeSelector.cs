using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR;

public class PlayerTypeSelector : MonoBehaviour
{

    public Vector3 ARScale = new Vector3(0.3f, 0.3f, 0.3f);

    // Start is called before the first frame update
    void Start()
    {
        if (Application.platform == RuntimePlatform.Android)
        {
            if (XRSettings.enabled)
            {
                // do quest stuff

            }
            else
            {
                // do normal android stuff
                transform.localScale = ARScale;
            }
        }

        if (Application.platform == RuntimePlatform.WSAPlayerARM)
        {
            // do HoloLens 2 stuff
            transform.localScale = ARScale;
        }

        if (Application.platform == RuntimePlatform.WSAPlayerX64 || Application.platform == RuntimePlatform.WSAPlayerX86)
        {
            // do HoloLens 1 stuff
            transform.localScale = ARScale;
        }
    }
}
