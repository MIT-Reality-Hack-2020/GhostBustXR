using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttachObjects : MonoBehaviour
{

    public RelativeTransformNetworkScript ToAttachHead;
    public RelativeTransformNetworkScriptOther ToAttachFlashLight;

    // Start is called before the first frame update
    void Start()
    {
        ToAttachHead.TransformToMove = GameObject.Find("FollowHead").transform;
        transform.parent = ToAttachHead.TransformToMove;
        transform.localPosition = Vector3.zero;
        ToAttachFlashLight.TransformToMove = GameObject.Find("Flashlight").transform;
    }

}
