using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.UIElements.Experimental;

public class CameraController : MonoBehaviour
{

    public GameObject player;
    private controller_FV pController_FV;
    private InputManager inputManager;
    [SerializeField] 
    CameraShaker nitrousCamShake;
    [SerializeField]
    CameraShaker driftCamShake;
    public CameraShaker carHitCamShake;

    [Space(10f)]
    [Header("Camera Constraints")]
    public GameObject cameraRestStop;
    public GameObject cameraRegular;
    public GameObject cameraBraking;
    public GameObject cameraNitrous;

    public GameObject cameraLookAt;
    public GameObject driftCamConstraintLeft;
    public GameObject driftCamConstraintRight;
    public GameObject brakingWhileDriftingLeftPosition;
    public GameObject brakingWhileDriftingRightPosition;
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
    //[SerializeField]
    //Vector3 offsetBrakingConstraints = new Vector3(0, 0, 2);
    //Vector3 brakingWhileDriftingLeftPosition;
    //Vector3 brakingWhileDriftingRightPosition;
    float default_DownForceValue;

    [Space(10f)]
    [Header("Camera Crash Settings")]
    public bool lookingAtCarCrash;
    public Transform otherCar;
    public float timeForLookingAtCrash = 4f;
    float timeForLookingAtCrashStart;
    public float enemyCrashFOV = 40f;
    public float crashSlowDownValue = 0.4f;
    public float crashDownForceValue = 400f;

    private void Awake()
    {
        player = GameObject.FindGameObjectWithTag("Player");

        driftCamConstraintLeft = GameObject.FindGameObjectWithTag("DriftCamConstraintLeft");
        driftCamConstraintRight = GameObject.FindGameObjectWithTag("DriftCamConstraintRight");
        lookAtEmptyLeft = GameObject.FindGameObjectWithTag("LookAtEmptyLeft");
        lookAtEmptyRight = GameObject.FindGameObjectWithTag("LookAtEmptyRight");


        pController_FV = player.GetComponent<controller_FV>();
        inputManager = player.GetComponent<InputManager>();

        defaultFOV = Camera.main.fieldOfView;


        timeForLookingAtCrashStart = timeForLookingAtCrash;


        //brakingWhileDriftingLeftPosition = driftCamConstraintLeft.transform.position + offsetBrakingConstraints;
        //brakingWhileDriftingRightPosition = driftCamConstraintRight.transform.position + offsetBrakingConstraints;
        SetDefaultValues();
    }

    private void FixedUpdate()
    {



    }

    private void LateUpdate()
    {
        
        if (lookingAtCarCrash)
        {
            timeForLookingAtCrash -= 1 * Time.deltaTime;
            CameraLookAtCarCrash(otherCar);
            //return;

        }
        else
        {
            //Follow();
            BoostFOV();
            //DriftConstraintsFollowCamera();
            
            //CameraMoveWhileTurning();
            DriftingCameraAction();
            ActivateRearViewCamera();


        }
    }

    public void SetDefaultValues()
    {
        default_DownForceValue = pController_FV.GetDownForceValue();
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

        gameObject.transform.position = Vector3.Lerp(transform.position, cameraRegular.transform.position, speed * Time.deltaTime);
        gameObject.transform.LookAt(cameraLookAt.transform.position);
    }

    void CameraMoveWhileTurning()
    {
        if (inputManager.horizontal > 0)
            gameObject.transform.position = Vector3.Lerp(transform.position, driftCamConstraintRight.transform.position, speed * Time.deltaTime);
        else if(inputManager.horizontal < 0)
            gameObject.transform.position = Vector3.Lerp(transform.position, driftCamConstraintLeft.transform.position, speed * Time.deltaTime);


    }

    private void DriftingCameraAction()
    {

        if (pController_FV.driveState == DriveState.driftingLeft && inputManager.vertical < 0 && pController_FV.reverse == false)// && inputManager.isSlowing == false && RR.KPH > 50)
        {
            gameObject.transform.position = Vector3.SmoothDamp(transform.position, brakingWhileDriftingLeftPosition.transform.position, ref velocity, Time.deltaTime * smoother);
            Quaternion OriginalRot = transform.rotation;
            transform.LookAt(lookAtEmptyLeft.transform.position);
            Quaternion newRot = transform.rotation;
            transform.rotation = OriginalRot;
            transform.rotation = Quaternion.Lerp(transform.rotation, newRot, Time.deltaTime * 1.4f);
            Quaternion nR = new Quaternion(gameObject.transform.localRotation.x, gameObject.transform.localRotation.y, -.19f, gameObject.transform.localRotation.w);
            transform.localRotation = Quaternion.Slerp(transform.localRotation, nR, Time.deltaTime * .5f);
            //gameObject.transform.LookAt(lookEmptyLeft.gameObject.transform.position);  //see if child works better than player or not
            driftCamShake.StartShake();

        }
        else if (pController_FV.driveState == DriveState.driftingRight && inputManager.vertical < 0 && pController_FV.reverse == false)// && inputManager.isSlowing == false && RR.KPH > 50)
        {
            gameObject.transform.position = Vector3.SmoothDamp(transform.position, brakingWhileDriftingRightPosition.transform.position, ref velocity, Time.deltaTime * smoother);
            //gameObject.transform.LookAt(lookEmptyRight.gameObject.transform.position);  //see if child works better than player or not
            Quaternion OriginalRot = transform.rotation;
            transform.LookAt(lookAtEmptyRight.transform.position);
            Quaternion newRot = transform.rotation;
            transform.rotation = OriginalRot;
            transform.rotation = Quaternion.Lerp(transform.rotation, newRot, Time.deltaTime * 1.4f);
            Quaternion nR = new Quaternion(gameObject.transform.localRotation.x, gameObject.transform.localRotation.y, .19f, gameObject.transform.localRotation.w);
            transform.localRotation = Quaternion.Slerp(transform.localRotation, nR, Time.deltaTime * .5f);
            driftCamShake.StartShake();

        }
        else if (pController_FV.driveState == DriveState.driftingLeft)// && inputManager.isSlowing == false && RR.KPH > 50)
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
            driftCamShake.StartShake();

        }
        else if (pController_FV.driveState == DriveState.driftingRight) // && inputManager.isSlowing == false && RR.KPH > 50)
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
            driftCamShake.StartShake();

        }
        else if (inputManager.vertical >= 0 && pController_FV.KPH > 25 && pController_FV.reverse == false) // && inputManager.vertical > 0 && RR.KPH > 0 && RR.reverse == false)
        {
            gameObject.transform.position = Vector3.SmoothDamp(transform.position, cameraRegular.transform.position, ref velocity, Time.deltaTime * smoother3);
            Quaternion OriginalRot = transform.rotation;
            transform.LookAt(cameraLookAt.transform.position);  //see if child works better than player or not
            Quaternion NewRot = transform.rotation;
            transform.rotation = OriginalRot;
            transform.rotation = Quaternion.Lerp(transform.rotation, NewRot, Time.deltaTime * .9f);
        }
        else if (pController_FV.KPH < 25 && pController_FV.KPH > 0 && pController_FV.reverse == false) // && inputManager.vertical > 0 && RR.KPH > 0 && RR.reverse == false)
        {
            gameObject.transform.position = Vector3.SmoothDamp(transform.position, cameraRestStop.transform.position, ref velocity, Time.deltaTime * smoother3);
            Quaternion OriginalRot = transform.rotation;
            transform.LookAt(cameraLookAt.transform.position);  //see if child works better than player or not
            Quaternion NewRot = transform.rotation;
            transform.rotation = OriginalRot;
            transform.rotation = Quaternion.Lerp(transform.rotation, NewRot, Time.deltaTime * .9f);
        }
        else if (inputManager.vertical < 0 && pController_FV.reverse == false) // && inputManager.vertical > 0 && RR.KPH > 0 && RR.reverse == false)
        {
           // Vector3 brakingPosition = transform.position + offsetBrakingConstraints;
            gameObject.transform.position = Vector3.SmoothDamp(transform.position, cameraBraking.transform.position, ref velocity, Time.deltaTime * smoother3);
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
            driftCamShake.StartShake();

        }
        else if (pController_FV.driftingRight)
        {
            gameObject.transform.position = Vector3.Lerp(transform.position, driftCamConstraintRight.transform.position, driftCamSpeed * Time.deltaTime);
            gameObject.transform.LookAt(cameraLookAt.transform.position);
            driftCamShake.StartShake();

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
            nitrousCamShake.StartShake();
        }
        else
        {
            Camera.main.fieldOfView = Mathf.Lerp(Camera.main.fieldOfView, defaultFOV, smoothTime * Time.deltaTime);
        }

    }

    public void CameraLookAtCarCrash(Transform car)
    {
        //GameObject player = FindObjectOfType<CameraController>().gameObject;
        otherCar = car;

        inputManager.driveController = driver.AI;

        lookingAtCarCrash = true;
        //gameObject.transform.position = Vector3.Lerp(transform.position, driftCamConstraintLeft.transform.position, driftCamSpeed * Time.deltaTime);
        Camera.main.transform.LookAt(otherCar.transform.position);
        //cameraShaker.StartShake();

        pController_FV.SetDownForceValue(crashDownForceValue);
        Camera.main.fieldOfView = Mathf.Lerp(Camera.main.fieldOfView, enemyCrashFOV, smoothTime * Time.deltaTime);
        CountupTimer.Instance.SetTimeScale(crashSlowDownValue);



        if (timeForLookingAtCrash <= 0)
        {
            pController_FV.SetDownForceValue(default_DownForceValue);
            inputManager.driveController = driver.keyboard;


            timeForLookingAtCrash = timeForLookingAtCrashStart;
            Camera.main.fieldOfView = Mathf.Lerp(Camera.main.fieldOfView, defaultFOV, smoothTime * Time.deltaTime);
            lookingAtCarCrash = false;
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
