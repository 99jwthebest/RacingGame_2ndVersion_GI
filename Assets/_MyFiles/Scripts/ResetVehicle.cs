using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResetVehicle : MonoBehaviour
{
    public controller_FV pController_FV;
    public InputManager inputManager;
    public PositionHolder positionHolder;
    public TrackWaypoints waypoints;
    public float timeUntilAbleToReset;
    [SerializeField]
    Quaternion carResetRotation;


    void Start()
    {
        pController_FV = FindObjectOfType<controller_FV>();
        inputManager = pController_FV.GetComponent<InputManager>();
        positionHolder = pController_FV.GetComponent<PositionHolder>();
        waypoints = FindObjectOfType<TrackWaypoints>();
    }

    // Update is called once per frame
    void Update()
    {
        if (pController_FV.IsGrounded())
        {
            return;
        }
        else if(!pController_FV.IsGrounded())
        {
            timeUntilAbleToReset += 1 * Time.deltaTime;

            if(inputManager.resetVehicle && timeUntilAbleToReset > 1.5f)
            {
                pController_FV.transform.position = waypoints.nodes[positionHolder.currentWaypoint].position;

                float carResetRotX = pController_FV.transform.rotation.x;
                float carResetRotY = pController_FV.transform.rotation.y;
                float carResetRotZ = 0;
                carResetRotation =  new Quaternion(0, carResetRotY, 0, 0);
                pController_FV.transform.rotation = carResetRotation;
                Debug.Log("WHaT THE ACTRUAL TRUCKERSA??!!!!");
                pController_FV.GetRigidbody().Sleep();
                timeUntilAbleToReset = 0;
            }
        }
    }
}
