using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;

public class CameraController : MonoBehaviour
{

    public GameObject player;
    private controller_FV pController_FV;
    public GameObject cameraConstraint;
    public GameObject cameraLookAt;
    public float speed;
    public float defaultFOV = 0f, desiredFOV = 0f;
    [Range(0,5)] public float smoothTime = 0f;

    private void Awake()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        cameraConstraint = GameObject.FindGameObjectWithTag("CameraConstraint");
        cameraLookAt = GameObject.FindGameObjectWithTag("CameraLookAt");
        pController_FV = player.GetComponent<controller_FV>();
        defaultFOV = Camera.main.fieldOfView;

    }

    private void FixedUpdate()
    {
        Follow();
        BoostFOV();

    }

    private void Follow()
    {
        if (speed <= 23)
        {
            speed = Mathf.Lerp(speed, pController_FV.KPH / 3, Time.deltaTime);
        }
        else
        {
            speed = 23;
        }

        //speed = pController_FV.KPH / smoothTime;

        gameObject.transform.position = Vector3.Lerp(transform.position, cameraConstraint.transform.position, speed * Time.deltaTime);
        gameObject.transform.LookAt(cameraLookAt.transform.position);
    }

    private void BoostFOV()
    {
        if(pController_FV.nitrousFlag)
            Camera.main.fieldOfView = Mathf.Lerp(Camera.main.fieldOfView, desiredFOV, smoothTime * Time.deltaTime);
        else
            Camera.main.fieldOfView = Mathf.Lerp(Camera.main.fieldOfView, defaultFOV, smoothTime * Time.deltaTime);

    }

}
