using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlashLightControl : MonoBehaviour
{
    public Transform FlashLight;

    // Start is called before the first frame update
    void Start()
    {
        FlashLight.parent = transform.parent;
    }

    private void OnDestroy()
    {
        Destroy(FlashLight.gameObject);
    }
}
