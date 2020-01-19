using System.Collections;
using System.Collections.Generic;
using Microsoft.MixedReality.Toolkit.Utilities;
using UnityEngine;

public class GhostARPhoneController : MonoBehaviour
{
    public float speed = 0.05f;
    public FixedJoystick joystick;
    private Transform _myTransform;
    private Transform _camTransform;
    private Vector3 _plane = new Vector3(1f, 0f, 1f);

    public void Start()
    {
        _myTransform = transform;
        _camTransform = CameraCache.Main.transform;
    }

    public void FixedUpdate()
    {
        if (joystick.Horizontal == 0f && joystick.Vertical == 0f) return;
        var forward = Vector3.Scale(_camTransform.forward, _plane).normalized;
        var right = Vector3.Scale(_camTransform.right, _plane).normalized;
        var direction = forward * joystick.Vertical + right * joystick.Horizontal;
        _myTransform.position = _myTransform.position + direction * speed * Time.fixedDeltaTime;
        _myTransform.forward = direction;
    }
}
