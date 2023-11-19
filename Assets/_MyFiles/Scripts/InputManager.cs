using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputManager : MonoBehaviour
{
    public float vertical;
    public float horizontal;
    public bool handbrake;
    public bool boosting;
    public bool rearViewCamera;
    public bool zoneActivated;

    private void FixedUpdate()
    {
        vertical = Input.GetAxis("Vertical");
        horizontal = Input.GetAxis("Horizontal");
        handbrake = (Input.GetAxis("Jump") != 0) ? true : false;
        if(Input.GetKey(KeyCode.LeftShift)) 
            boosting = true; 
        else 
            boosting = false;

        if(Input.GetKey(KeyCode.R))
            rearViewCamera = true;
        else
            rearViewCamera = false;

        if (Input.GetMouseButton(0))
            zoneActivated = true;
        else
            zoneActivated = false;
    }

}
