using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class controller_FV : MonoBehaviour
{
    internal enum DriveType
    {
        frontWheelDrive,
        rearWheelDrive,
        allWheelDrive
    }

    [SerializeField]
    private DriveType driveType;

    internal enum GearBox
    {
        automatic,
        manual
    }

    [SerializeField]
    private GearBox gearChange;


    private InputManager inputManager;

    [HideInInspector] public float nitrousValue;
    [HideInInspector] public bool nitrousFlag;
    [HideInInspector] public bool test; //engine sound boolean

    [Header("Variables")]
    public GameObject go_wheelColliders, go_wheelMeshes;
    public WheelCollider[] wheelColliders = new WheelCollider[4];
    public GameObject[] wheelMeshes = new GameObject[4];
    public GameObject centerOfMass;
    private Rigidbody rigidBody;
    public float handBrakeFrictionMultiplier = 2f; // lower value to drift more, higher value to drift less
    public AnimationCurve enginePower;
    public float maxRPM, minRPM;
    public float[] gears;
    public float[] gearChangeSpeed;

    [HideInInspector] public int gearNum = 1;
    public float KPH;
    [HideInInspector] public float engineRPM;
    [HideInInspector] public bool reverse = false;
    private float smoothTime = 0.09f;
    public float downForceValue = 50f;
    public int motorTorque = 200;
    public float steeringMax = 3f;
    public ParticleSystem[] nitrousSmoke;


    [Header("DEBUG")]
    public float[] slip = new float[4];

    // Handbrake / Drifting values
    private WheelFrictionCurve forwardFriction, sidewaysFriction;
    private float radius = 6, brakePower = 0, wheelsRPM, driftFactor, lastValue, horizontal, vertical, totalPower;
    private bool flag = false;
    [HideInInspector] public bool playPauseSmoke = false, hasFinished;
    
    // Start is called before the first frame update
    void Start()
    {
        GetObjects();
        StartCoroutine(TimedLoop());
    }

    void FixedUpdate()
    {

        horizontal = inputManager.horizontal;
        vertical = inputManager.vertical;



        lastValue = engineRPM;


        AddDownForce(); //
        AnimateWheels(); //
        SteerVehicle(); //
        GetFriction();
        CalculateEnginePower(); //
        AdjustTraction(); //
        ActivateNitrous(); //
    }


    private void GetObjects()
    {
        inputManager = GetComponent<InputManager>();
        rigidBody = GetComponent<Rigidbody>();
        //gameManager = GameObject.FindGameObjectWithTag("GameManager").GetComponent<GameManager>();

        // Get Wheel Function
        go_wheelColliders = GameObject.FindGameObjectWithTag("wheelColliderTrans");
        wheelColliders[0] = go_wheelColliders.transform.Find("wc_Front_Wheel_Left").gameObject.GetComponent<WheelCollider>();
        wheelColliders[1] = go_wheelColliders.transform.Find("wc_Front_Wheel_Right").gameObject.GetComponent<WheelCollider>();
        wheelColliders[2] = go_wheelColliders.transform.Find("wc_Rear_Wheel_Left").gameObject.GetComponent<WheelCollider>();
        wheelColliders[3] = go_wheelColliders.transform.Find("wc_Rear_Wheel_Right").gameObject.GetComponent<WheelCollider>();

        go_wheelMeshes = GameObject.FindGameObjectWithTag("wheelMeshesTrans");
        wheelMeshes[0] = go_wheelMeshes.transform.Find("Front_Wheel_Left").gameObject;
        wheelMeshes[1] = go_wheelMeshes.transform.Find("Front_Wheel_Right").gameObject;
        wheelMeshes[2] = go_wheelMeshes.transform.Find("Rear_Wheel_Left").gameObject;
        wheelMeshes[3] = go_wheelMeshes.transform.Find("Rear_Wheel_Right").gameObject;


        centerOfMass = GameObject.FindGameObjectWithTag("CenterOfMass");
        rigidBody.centerOfMass = centerOfMass.transform.localPosition;
    }

    private void CalculateEnginePower()
    {
        WheelRPM();


        if (vertical != 0)
        {
            rigidBody.drag = 0.005f;
        }
        if (vertical == 0)
        {
            rigidBody.drag = 0.1f;
        }
        totalPower = 3.6f * enginePower.Evaluate(engineRPM) * (vertical);



        float velocity = 0.0f;
        if (engineRPM >= maxRPM || flag)
        {
            engineRPM = Mathf.SmoothDamp(engineRPM, maxRPM - 500, ref velocity, 0.05f);

            flag = (engineRPM >= maxRPM - 450) ? true : false;
            test = (lastValue > engineRPM) ? true : false;
        }
        else
        {
            engineRPM = Mathf.SmoothDamp(engineRPM, 1000 + (Mathf.Abs(wheelsRPM) * 3.6f * (gears[gearNum])), ref velocity, smoothTime);
            test = false;
        }

        if (engineRPM >= maxRPM + 1000) 
            engineRPM = maxRPM + 1000; // clamp at max


        MoveVehicle();
        Shifter();
    }

    private void WheelRPM()
    {
        float sum = 0;
        int r = 0;
        for(int i = 0; i < 4; i++)
        {
            sum += wheelColliders[i].rpm;
            r++;
        }
        wheelsRPM = (r != 0) ? sum / r : 0;

        if (wheelsRPM < 0 && !reverse)
        {
            reverse = true;

            if (gameObject.tag != "AI")
                UIManager.instance.ChangeGear();
        }
        else if (wheelsRPM > 0 && reverse)
        {
            reverse = false;

            if (gameObject.tag != "AI")
                UIManager.instance.ChangeGear();
        }
    }

    private bool CheckGears()
    {
        if (KPH >= gearChangeSpeed[gearNum]) 
            return true;
        else 
            return false;
    }

    private void Shifter()
    {
        if (!IsGrounded()) 
            return;

        //automatic
        if (engineRPM > maxRPM && gearNum < gears.Length - 1 && !reverse && CheckGears())
        {
            gearNum++;

            if (gameObject.tag != "AI")
                UIManager.instance.ChangeGear();

            return;
        }
        if (engineRPM < minRPM && gearNum > 0)
        {
            gearNum--;

            if (gameObject.tag != "AI")
                UIManager.instance.ChangeGear();
        }
    }

    private bool IsGrounded()
    {
        if (wheelColliders[0].isGrounded && wheelColliders[1].isGrounded && wheelColliders[2].isGrounded && wheelColliders[3].isGrounded) // ask Li if this is okay or what's a better way to write this
            return true;
        else
            return false;
    }

    private void AddDownForce()
    {
        rigidBody.AddForce(-transform.up * downForceValue * rigidBody.velocity.magnitude);
    }

    private void MoveVehicle()
    {

        BrakeVehicle();

        if (driveType == DriveType.allWheelDrive)
        {
            for (int i = 0; i < wheelColliders.Length; i++)
            {
                wheelColliders[i].motorTorque = totalPower / 4;
                wheelColliders[i].brakeTorque = brakePower;
            }
        }
        else if (driveType == DriveType.rearWheelDrive)
        {
            wheelColliders[2].motorTorque = totalPower / 2;
            wheelColliders[3].motorTorque = totalPower / 2;

            for (int i = 0; i < wheelColliders.Length; i++)
            {
                wheelColliders[i].brakeTorque = brakePower;
            }
        }
        else
        {
            wheelColliders[0].motorTorque = totalPower / 2;
            wheelColliders[1].motorTorque = totalPower / 2;

            for (int i = 0; i < wheelColliders.Length; i++)
            {
                wheelColliders[i].brakeTorque = brakePower;
            }
        }

        KPH = rigidBody.velocity.magnitude * 3.6f;


    }

    private void BrakeVehicle()
    {

        if (vertical < 0)
        {
            brakePower = (KPH >= 10) ? 500 : 0;
        }
        else if (vertical == 0 && (KPH <= 10 || KPH >= -10))
        {
            brakePower = 10;
        }
        else
        {
            brakePower = 0;
        }


    }

    private void SteerVehicle()
    {

        //acerman steering formula
        //steerAngle = Mathf.Rad2Deg * Mathf.Atan(2.55f / (radius + (1.5f / 2))) * horizontalInput;
        // it turns the wheel that is on the side of the turn more than the wheel that is not the side that is being turned
        // so that it can make a complete turn or circle instead of just slightly turning the car each time
        if (inputManager.horizontal > 0)
        {
            //rear tracks size is set to 1.5f       wheel base has been set to 2.55f
            wheelColliders[0].steerAngle = Mathf.Rad2Deg * Mathf.Atan(2.55f / (radius + (1.5f / 2))) * inputManager.horizontal;
            wheelColliders[1].steerAngle = Mathf.Rad2Deg * Mathf.Atan(2.55f / (radius - (1.5f / 2))) * inputManager.horizontal;
        }
        else if (inputManager.horizontal < 0)
        {
            wheelColliders[0].steerAngle = Mathf.Rad2Deg * Mathf.Atan(2.55f / (radius - (1.5f / 2))) * inputManager.horizontal;
            wheelColliders[1].steerAngle = Mathf.Rad2Deg * Mathf.Atan(2.55f / (radius + (1.5f / 2))) * inputManager.horizontal;
            //transform.Rotate(Vector3.up * steerHelping);

        }
        else
        {
            wheelColliders[0].steerAngle = 0;
            wheelColliders[1].steerAngle = 0;
        }

    }

    private void AnimateWheels()
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

    private void GetFriction()
    {
        for(int i = 0; i < wheelColliders.Length; i++)
        {
            WheelHit wheelHit;
            wheelColliders[i].GetGroundHit(out wheelHit);

            slip[i] = wheelHit.sidewaysSlip;

        }
    }


    private void AdjustTraction()
    {
        //time it takes to go from normal drive to drift 
        float driftSmoothFactor = .7f * Time.deltaTime;

        if (inputManager.handbrake)
        {
            sidewaysFriction = wheelColliders[0].sidewaysFriction;
            forwardFriction = wheelColliders[0].forwardFriction;

            float velocity = 0;
            sidewaysFriction.extremumValue = sidewaysFriction.asymptoteValue = forwardFriction.extremumValue = forwardFriction.asymptoteValue =
                Mathf.SmoothDamp(forwardFriction.asymptoteValue, driftFactor * handBrakeFrictionMultiplier, ref velocity, driftSmoothFactor);

            for (int i = 0; i < 4; i++)
            {
                wheelColliders[i].sidewaysFriction = sidewaysFriction;
                wheelColliders[i].forwardFriction = forwardFriction;
            }

            sidewaysFriction.extremumValue = sidewaysFriction.asymptoteValue = forwardFriction.extremumValue = forwardFriction.asymptoteValue = 1.1f;
            //extra grip for the front wheels
            for (int i = 0; i < 2; i++)
            {
                wheelColliders[i].sidewaysFriction = sidewaysFriction;
                wheelColliders[i].forwardFriction = forwardFriction;
            }
            rigidBody.AddForce(transform.forward * (KPH / 400) * 10000);
            //rigidBody.AddForce(transform.forward * 10000);


            //executed when handbrake is being held
        }
        else
        {

            forwardFriction = wheelColliders[0].forwardFriction;
            sidewaysFriction = wheelColliders[0].sidewaysFriction;

            forwardFriction.extremumValue = forwardFriction.asymptoteValue = sidewaysFriction.extremumValue = sidewaysFriction.asymptoteValue =
                ((KPH * handBrakeFrictionMultiplier) / 300) + 1;

            for (int i = 0; i < 4; i++)
            {
                wheelColliders[i].forwardFriction = forwardFriction;
                wheelColliders[i].sidewaysFriction = sidewaysFriction;

            }
        }

        //checks the amount of slip to control the drift
        for (int i = 2; i < 4; i++)
        {

            WheelHit wheelHit;

            wheelColliders[i].GetGroundHit(out wheelHit);
            //smoke
            if (wheelHit.sidewaysSlip >= 0.3f || wheelHit.sidewaysSlip <= -0.3f || wheelHit.forwardSlip >= .3f || wheelHit.forwardSlip <= -0.3f)
            {
                playPauseSmoke = true;
                Debug.Log("I am DRIFTING!!!!");

                if(vertical > 0)
                {
                    //rigidBody.AddForce(transform.forward * 1000);
                    Debug.Log("Adding MY PERSONAL FORce!!");
                }
            }
            else
            {
                playPauseSmoke = false;
                Debug.Log("I am NOT drifting!!!!");

            }


            if (wheelHit.sidewaysSlip < 0) driftFactor = (1 + -inputManager.horizontal) * Mathf.Abs(wheelHit.sidewaysSlip);

            if (wheelHit.sidewaysSlip > 0) driftFactor = (1 + inputManager.horizontal) * Mathf.Abs(wheelHit.sidewaysSlip);
        }
    }

    private IEnumerator TimedLoop()
    {
        while (true)
        {
            yield return new WaitForSeconds(.7f);
            radius = 6 + KPH / 20;

        }
    }

    public void ActivateNitrous()
    {
        if(!inputManager.boosting && nitrousValue <= 10)
        {
            nitrousValue += Time.deltaTime;
        }
        else
        {
            nitrousValue -= (nitrousValue <= 0) ? 0 : Time.deltaTime;
        }

        if(inputManager.boosting)
        {
            if(nitrousValue > 0)
            {
                StartNitrousEmitter();
                rigidBody.AddForce(transform.forward * 10000);
            }
            else
            {
                StopNitrousEmitter();
            }
        }
        else
        {
            StopNitrousEmitter();
        }
    }

    public void StartNitrousEmitter()
    {
        if (nitrousFlag)
            return;

        for(int i = 0; i < nitrousSmoke.Length; i++)
        {
            nitrousSmoke[i].Play();
        }

        nitrousFlag = true;
    }

    public void StopNitrousEmitter()
    {
        if (!nitrousFlag)
            return;

        for (int i = 0; i < nitrousSmoke.Length; i++)
        {
            nitrousSmoke[i].Stop();
        }

        nitrousFlag = false;
    }

}
