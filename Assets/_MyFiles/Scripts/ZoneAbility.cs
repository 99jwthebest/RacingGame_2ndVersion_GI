using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ZoneAbility : MonoBehaviour
{
    [SerializeField]
    private controller_FV pController_FV;
    [SerializeField]
    private InputManager inputManager;

    // Start is called before the first frame update
    void Start()
    {
        pController_FV = GetComponent<controller_FV>();
        inputManager = GetComponent<InputManager>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void ActivateZoneAbility()
    {
        if (inputManager.zoneActivated)
        {

        }
        else
        {

        }
    }
}
