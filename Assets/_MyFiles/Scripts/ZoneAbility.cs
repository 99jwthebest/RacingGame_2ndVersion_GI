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
    private float default_DownForceValue;
    [SerializeField]
    private float default_HandBrakeValue;
    [SerializeField]
    private float downForceValue;
    [SerializeField]
    private float handBrakeValue;
    [SerializeField]
    private float timeSlowDownValue;



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
        ActivateZoneAbility();

    }

    void SetDefaultValues()
    {
        default_DownForceValue = pController_FV.GetDownForceValue();
        default_HandBrakeValue = pController_FV.GetHandBrakeValue();
    }

    void ActivateZoneAbility()   // ask professor Li if there is a way to turn the car even more
    {
        if (inputManager.zoneActivated)
        {
            pController_FV.SetDownForceValue(downForceValue);
            pController_FV.SetHandBrakeValue(handBrakeValue);
            countupTimer.SetTimeScale(timeSlowDownValue);
        }
        else
        {
            DeactivateZoneAbility();
        }
    }

    void DeactivateZoneAbility()
    {
        countupTimer.SetTimeScale(countupTimer.GetStartTimeScale());

        pController_FV.SetDownForceValue(default_DownForceValue);
        pController_FV.SetHandBrakeValue(default_HandBrakeValue);
    }
}
