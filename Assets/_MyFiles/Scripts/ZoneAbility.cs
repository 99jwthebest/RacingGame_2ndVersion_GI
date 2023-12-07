using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ZoneAbility : MonoBehaviour
{
    [SerializeField]
    private controller_FV pController_FV;
    [SerializeField]
    private InputManager inputManager;
    [SerializeField]
    private CountupTimer countupTimer;
    [SerializeField]
    private CameraController cameraController;
    [SerializeField]
    private float timeSlowDownValue;
    [SerializeField]
    private float default_DownForceValue;
    [SerializeField]
    private float default_HandBrakeValue;
    [SerializeField]
    private float downForceValue;
    [SerializeField]
    private float handBrakeValue;
    [SerializeField]
    private float zoneValue;
    [SerializeField]
    private float carSteeringHelp;



    // Start is called before the first frame update
    void Start()
    {
        pController_FV = GetComponent<controller_FV>();
        inputManager = GetComponent<InputManager>();
        countupTimer = CountupTimer.Instance;

        SetDefaultValues();


    }

    // Update is called once per frame
    void Update()
    {
        CheckForZoneValue();
        ActivateZoneAbility();

    }

    public void SetDefaultValues()
    {
        default_DownForceValue = pController_FV.GetDownForceValue();
        default_HandBrakeValue = pController_FV.GetHandBrakeValue();
    }

    public void CheckForZoneValue()
    {
        if (!inputManager.zoneActivated && zoneValue <= 10)
        {
            zoneValue += Time.deltaTime / 2;
        }
        else
        {
            zoneValue -= (zoneValue <= 0) ? 0 : Time.deltaTime * 5;
        }
    }

    public void ActivateZoneAbility()   // ask professor Li if there is a way to turn the car even more
    {
        if (inputManager.zoneActivated && zoneValue > 0)
        {
            pController_FV.SetDownForceValue(downForceValue);
            pController_FV.SetHandBrakeValue(handBrakeValue);
            bool zoneActivated = true;
            UIManager.instance.SetZoneImage(zoneActivated);
            countupTimer.SetTimeScale(timeSlowDownValue);

            if(inputManager.horizontal > 0)
                pController_FV.GetRigidbody().AddForce(transform.right * carSteeringHelp);
            else if(inputManager.horizontal < 0)
                pController_FV.GetRigidbody().AddForce(-transform.right * carSteeringHelp);
        }
        else
        {
            if (CountupTimer.Instance.TimeStopped() || cameraController.lookingAtCarCrash)
                return;
            DeactivateZoneAbility();
        }
    }

    void DeactivateZoneAbility()
    {
        countupTimer.SetTimeScale(countupTimer.GetStartTimeScale());
        bool zoneActivated = false;
        UIManager.instance.SetZoneImage(zoneActivated);
        pController_FV.SetDownForceValue(default_DownForceValue);
        pController_FV.SetHandBrakeValue(default_HandBrakeValue);
    }


    public float GetZoneValue()
    {
        return zoneValue;
    }
}
