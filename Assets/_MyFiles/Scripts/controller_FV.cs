using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class controller_FV : MonoBehaviour
{
    public WheelCollider[] wheelColliders = new WheelCollider[4];
    public GameObject[] wheelMeshes = new GameObject[4];

    public float torque = 200f;
    public float steeringMax = 3f;


    // Start is called before the first frame update
    void Start()
    {
        
    }

    void FixedUpdate()
    {
        animateWheels();

        if(Input.GetKeyDown(KeyCode.W))
        {
            for(int i = 0; i < wheelColliders.Length; i++)
            {
                wheelColliders[i].motorTorque = torque;
            }
        }

        if (Input.GetAxis("Horizontal") != 0)
        {
            for (int i = 0; i < wheelColliders.Length - 2; i++)
            {
                wheelColliders[i].steerAngle = Input.GetAxis("Horizontal") * steeringMax;
            }
        }
        else
        {
            for (int i = 0; i < wheelColliders.Length - 2; i++)
            {
                wheelColliders[i].steerAngle = 0;
            }
        }
    }

    private void animateWheels()
    {
        Vector3 wheelPosition = Vector3.zero;
        Quaternion wheelRotation = Quaternion.identity;

        for(int i = 0; i < 4; i++)
        {
            wheelColliders [i].GetWorldPose(out wheelPosition, out wheelRotation);
            wheelMeshes[i].transform.position = wheelPosition;
            wheelMeshes[i].transform.rotation = wheelRotation;
        }

    }
}
