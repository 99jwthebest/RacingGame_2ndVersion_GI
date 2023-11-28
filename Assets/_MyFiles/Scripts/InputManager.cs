using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

public class InputManager : MonoBehaviour
{

    internal enum driver
    {
        AI,
        keyboard,
        mobile
    }
    [SerializeField] driver driveController;

    public float vertical;
    public float horizontal;
    public bool handbrake;
    public bool boosting;
    public bool rearViewCamera;
    public bool zoneActivated;

    public TrackWaypoints waypoints;
    public Transform currentWaypoint;
    public List<Transform> nodes = new List<Transform>();
    public int distanceOffset = 5;
    public float steerForce = 5;
    [Header("AI acceleration value")]
    [Range(0, 1)] public float acceleration = 0.5f;
    public int currentNode;


    private void Awake()
    {
        waypoints = GameObject.FindGameObjectWithTag("Path").GetComponent<TrackWaypoints>();
        currentWaypoint = gameObject.transform;

        nodes = waypoints.nodes;
    }


    private void FixedUpdate()
    {
        switch (driveController)
        {
            case driver.AI:
                AIDrive();
                break;
            case driver.keyboard:
                CalculateDistanceOfWaypoints();
                keyboardDrive();

                break; 
            case driver.mobile:  
                break;
        }

    }

    private void AIDrive()
    {
        CalculateDistanceOfWaypoints();
        AISteer();
        vertical = acceleration;
    }

    private void keyboardDrive()
    {
        vertical = Input.GetAxis("Vertical");
        horizontal = Input.GetAxis("Horizontal");
        handbrake = (Input.GetAxis("Jump") != 0) ? true : false;
        if (Input.GetKey(KeyCode.LeftShift))
            boosting = true;
        else
            boosting = false;

        if (Input.GetKey(KeyCode.R))
            rearViewCamera = true;
        else
            rearViewCamera = false;

        if (Input.GetMouseButton(0))
            zoneActivated = true;
        else
            zoneActivated = false;
    }

    private void mobileDrive()
    {

    }

    void CalculateDistanceOfWaypoints()
    {
        Vector3 position = gameObject.transform.position;
        float distance = Mathf.Infinity;

        for (int i = 0; i < nodes.Count; i++)
        {
            Vector3 difference = nodes[i].transform.position - position;
            float currentDistance = difference.magnitude;
            if (currentDistance < distance)
            {
                if ((i + distanceOffset) >= nodes.Count)
                {
                    Debug.Log("Is is it 1111??!?!?");

                    currentWaypoint = nodes[0];
                    distance = currentDistance;
                }
                else
                {
                    Debug.Log("Is is ELSE??!?!?");
                    currentWaypoint = nodes[i + distanceOffset];
                    distance = currentDistance;
                }
                currentNode = i;
            }

        }

    }

    void AISteer()
    {
        Vector3 relative = transform.InverseTransformPoint(currentWaypoint.transform.position);
        relative /= relative.magnitude;

        horizontal = (relative.x / relative.magnitude) * steerForce;
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(currentWaypoint.position, 3);
    }

}
