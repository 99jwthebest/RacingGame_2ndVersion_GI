using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.UIElements.Experimental;

public class CameraController : MonoBehaviour
{

    public GameObject player;
    private controller_FV pController_FV;
    private InputManager inputManager;
    private CameraShaker cameraShaker;
    
    [Space(10f)]
    [Header("Camera Constraints")]
    public GameObject cameraFront;
    public GameObject cameraBack;
    public GameObject cameraLookAt;
    public GameObject driftCamConstraintLeft;
    public GameObject driftCamConstraintRight;
    public GameObject lookAtEmptyLeft;
    public GameObject lookAtEmptyRight;
    public GameObject rearViewCamera;

    [Space(10f)]
    [Header("Camera Variables")]
    public Vector3 velocity = Vector3.zero;
    public float smoother = 1f;
    public float smoother2 = 1f;
    public float smoother3 = 1f;
    public float speed;
    public float driftCamSpeed;
    public float defaultFOV = 0f, desiredFOV = 0f;
    [Range(0,5)] public float smoothTime = 0f;

    private void Awake()
    {
        player = GameObject.FindGameObjectWithTag("Player");

        cameraFront = GameObject.FindGameObjectWithTag("CameraFront");  // ask professor Li how to find these objects in effecient way
        cameraBack = GameObject.FindGameObjectWithTag("CameraBack");
        cameraLookAt = GameObject.FindGameObjectWithTag("CameraLookAt");
        driftCamConstraintLeft = GameObject.FindGameObjectWithTag("DriftCamConstraintLeft");
        driftCamConstraintRight = GameObject.FindGameObjectWithTag("DriftCamConstraintRight");
        lookAtEmptyLeft = GameObject.FindGameObjectWithTag("LookAtEmptyLeft");
        lookAtEmptyRight = GameObject.FindGameObjectWithTag("LookAtEmptyRight");


        pController_FV = player.GetComponent<controller_FV>();
        inputManager = player.GetComponent<InputManager>();
        cameraShaker = GetComponent<CameraShaker>();

        defaultFOV = Camera.main.fieldOfView;
    }

    private void FixedUpdate()
    {
        //Follow();
        BoostFOV();
        DriftingCameraAction();
        ActivateRearViewCamera();
    }

    private void Follow()  // give leave way to have the camera move right left up or down so it looks like it's not fixed on the car.
    {
        if (pController_FV.driftingLeft || pController_FV.driftingRight)
            return;

        //if (speed <= 23)
        //{
        //    speed = Mathf.Lerp(speed, pController_FV.KPH / 3, Time.deltaTime);
        //}
        //else
        //{
        //    speed = 23;
        //}

        //speed = pController_FV.KPH / smoothTime;

        gameObject.transform.position = Vector3.Lerp(transform.position, cameraBack.transform.position, speed * Time.deltaTime);
        gameObject.transform.LookAt(cameraLookAt.transform.position);
    }

    private void DriftingCameraAction()
    {

        if (pController_FV.driftingRight) // && inputManager.isSlowing == false && RR.KPH > 50)
        {
            gameObject.transform.position = Vector3.SmoothDamp(transform.position, driftCamConstraintRight.transform.position, ref velocity, Time.deltaTime * smoother);
            //gameObject.transform.LookAt(lookEmptyRight.gameObject.transform.position);  //see if child works better than player or not
            Quaternion OriginalRot = transform.rotation;
            transform.LookAt(lookAtEmptyRight.transform.position);
            Quaternion newRot = transform.rotation;
            transform.rotation = OriginalRot;
            transform.rotation = Quaternion.Lerp(transform.rotation, newRot, Time.deltaTime * 1.4f);
            Quaternion nR = new Quaternion(gameObject.transform.localRotation.x, gameObject.transform.localRotation.y, .19f, gameObject.transform.localRotation.w);
            transform.localRotation = Quaternion.Slerp(transform.localRotation, nR, Time.deltaTime * .5f);
            cameraShaker.StartShake();

        }
        else if (pController_FV.driftingLeft)// && inputManager.isSlowing == false && RR.KPH > 50)
        {
            gameObject.transform.position = Vector3.SmoothDamp(transform.position, driftCamConstraintLeft.transform.position, ref velocity, Time.deltaTime * smoother);
            Quaternion OriginalRot = transform.rotation;
            transform.LookAt(lookAtEmptyLeft.transform.position);
            Quaternion newRot = transform.rotation;
            transform.rotation = OriginalRot;
            transform.rotation = Quaternion.Lerp(transform.rotation, newRot, Time.deltaTime * 1.4f);
            Quaternion nR = new Quaternion(gameObject.transform.localRotation.x, gameObject.transform.localRotation.y, -.19f, gameObject.transform.localRotation.w);
            transform.localRotation = Quaternion.Slerp(transform.localRotation, nR, Time.deltaTime * .5f);
            //gameObject.transform.LookAt(lookEmptyLeft.gameObject.transform.position);  //see if child works better than player or not
            cameraShaker.StartShake();
        }
        else //if (inputManager.isSlowing == false) // && inputManager.vertical > 0 && RR.KPH > 0 && RR.reverse == false)
        {
            gameObject.transform.position = Vector3.SmoothDamp(transform.position, cameraBack.transform.position, ref velocity, Time.deltaTime * smoother3);
            Quaternion OriginalRot = transform.rotation;
            transform.LookAt(cameraLookAt.transform.position);  //see if child works better than player or not
            Quaternion NewRot = transform.rotation;
            transform.rotation = OriginalRot;
            transform.rotation = Quaternion.Lerp(transform.rotation, NewRot, Time.deltaTime * .9f);
        }
        //else if (inputManager.isSlowing == false)
        //{
        //    gameObject.transform.position = Vector3.SmoothDamp(transform.position, camLepPos.transform.position, ref velocity, Time.deltaTime * smoother2);
        //    Quaternion OriginalRot = transform.rotation;
        //    transform.LookAt(lookEmpty.gameObject.transform.position);  //see if child works better than player or not
        //    Quaternion NewRot = transform.rotation;
        //    transform.rotation = OriginalRot;
        //    transform.rotation = Quaternion.Lerp(transform.rotation, NewRot, Time.deltaTime * .9f);
        //}


        //if (pController_FV.driftingLeft)
        //{
        //    gameObject.transform.position = Vector3.Lerp(transform.position, driftCamConstraintLeft.transform.position, driftCamSpeed * Time.deltaTime);
        //    gameObject.transform.LookAt(cameraLookAt.transform.position);
        //    cameraShaker.StartShake();

        //}
        //else if (pController_FV.driftingRight)
        //{
        //    gameObject.transform.position = Vector3.Lerp(transform.position, driftCamConstraintRight.transform.position, driftCamSpeed * Time.deltaTime);
        //    gameObject.transform.LookAt(cameraLookAt.transform.position);
        //    cameraShaker.StartShake();

        //}

    }

    private void CameraShiftWhileDriving()
    {
        if (pController_FV.driftingLeft)
        {
            gameObject.transform.position = Vector3.Lerp(transform.position, driftCamConstraintLeft.transform.position, driftCamSpeed * Time.deltaTime);
            gameObject.transform.LookAt(cameraLookAt.transform.position);
            cameraShaker.StartShake();

        }
        else if (pController_FV.driftingRight)
        {
            gameObject.transform.position = Vector3.Lerp(transform.position, driftCamConstraintRight.transform.position, driftCamSpeed * Time.deltaTime);
            gameObject.transform.LookAt(cameraLookAt.transform.position);
            cameraShaker.StartShake();

        }
    }

    private void BoostFOV()
    {
        if(GetMainCameraState() == false)
        {
            return;
        }

        if (pController_FV.nitrousFlag)
        {
            Camera.main.fieldOfView = Mathf.Lerp(Camera.main.fieldOfView, desiredFOV, smoothTime * Time.deltaTime);
            cameraShaker.StartShake();
        }
        else
        {
            Camera.main.fieldOfView = Mathf.Lerp(Camera.main.fieldOfView, defaultFOV, smoothTime * Time.deltaTime);
        }

    }

    private void ActivateRearViewCamera()
    {
        if (inputManager.rearViewCamera)
        {
            SetMainCamera(false);
            rearViewCamera.SetActive(true);
        }
        else
        {
            SetMainCamera(true);
            rearViewCamera.SetActive(false);
        }
    }

    private void SetMainCamera(bool state)
    {
        GetComponent<Camera>().enabled = state;
    }

    private bool GetMainCameraState()
    {
        if(Camera.main == null)
        {
            return false;
        }

        return GetComponent<Camera>().enabled;
    }
}
