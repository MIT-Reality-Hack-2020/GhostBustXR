using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.XR.ARFoundation;

public class ArTouchHandler : MonoBehaviour
{

    public Transform ObjToPlace;
    private Camera _camera;
    public ARRaycastManager RayManager;
    public UnityEvent ObjectPlaced;

    // Start is called before the first frame update
    void Start()
    {
        _camera = Camera.main;
    }

    // Update is called once per frame
    void Update()
    {
        foreach (var touch in Input.touches)
        {
            if (touch.phase == TouchPhase.Began)
            {
                var hitResults = new List<ARRaycastHit>();
                if (RayManager.Raycast(touch.position, hitResults))
                {
                    ObjToPlace.position = hitResults[0].pose.position;
                    ObjToPlace.gameObject.SetActive(true);
                    ObjectPlaced.Invoke();
                }
                return;
            }
        }
    }
}
