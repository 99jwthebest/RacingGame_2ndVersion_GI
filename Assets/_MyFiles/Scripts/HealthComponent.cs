using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class HealthComponent : MonoBehaviour
{
    [SerializeField]
    CameraController cameraController;
    [SerializeField]
    controller_FV pController_FV;
    [SerializeField]
    Vector3 carCrashResetPosition;
    [SerializeField]
    Quaternion carCrashResetRotation;
    public int currentHealth;
    public int maxHealth;
    public Slider healthSlider;
    private Coroutine regen;
    public float regenHealthYieldTime = 3f;
    public float regenHealthRateTime = .1f;
    public float crashForce = 100f;
    public bool torque;


    // Start is called before the first frame update
    void Start()
    {
        SetMaxHealth(maxHealth);
        cameraController = FindObjectOfType<CameraController>();
        pController_FV = FindObjectOfType<controller_FV>();

    }

    // Update is called once per frame
    void Update()
    {
        if(pController_FV == null)
        {
            pController_FV = FindObjectOfType<controller_FV>();
        }
        if(cameraController == null)
        {
            cameraController = FindObjectOfType<CameraController>();
        }
    }

    private void OnTriggerEnter(Collider collider)
    {
        if (collider.transform.tag == "Player")
        {
            TakeDamage(20);
        }
    }


    public int GetCurrentHealth()
    {
        return currentHealth;
    }

    public void SetMaxHealth(int health)
    {
        healthSlider.maxValue = health;
        healthSlider.value = health;

    }

    public void SetHealth(int health)
    {
        healthSlider.value = health;
    }

    public void TakeDamage(int amountOfDam)
    {
        currentHealth -= amountOfDam;
        SetHealth(currentHealth);

        //if (regen != null)
        //{
        //    StopCoroutine(regen);
        //}

        //regen = StartCoroutine(RegenHealth());
        pController_FV.AddToNitrousValue(2);
        cameraController.carHitCamShake.StartShake();

        if (currentHealth <= 0)
        {
            currentHealth = 0;

            CarDeath();
            Debug.Log("Person is DEAD!!!");
        }
        Debug.Log(currentHealth);


    }

    public void CarDeath()
    {
        carCrashResetPosition = gameObject.transform.position;
        carCrashResetRotation = gameObject.transform.rotation;

        cameraController.carHitCamShake.StopShake();
        
        AIController aiC = GetComponent<AIController>();
        aiC.SetDownForceValue(0);
        Rigidbody rb = GetComponent<Rigidbody>();

        if(torque)
            rb.AddTorque(gameObject.transform.position * crashForce, ForceMode.Impulse);
        else
            rb.AddRelativeTorque(gameObject.transform.position * crashForce, ForceMode.Impulse);

        cameraController.CameraLookAtCarCrash(gameObject.transform);
        cameraController.GetResetAICarPosition(carCrashResetPosition);
        cameraController.GetResetAICarRotation(carCrashResetRotation);
        cameraController.GetAIComponents(aiC, this);

    }

    private IEnumerator RegenHealth()
    {
        yield return new WaitForSeconds(regenHealthYieldTime);

        while (currentHealth < maxHealth)
        {
            currentHealth += maxHealth / 100;
            SetHealth(currentHealth);
            yield return new WaitForSeconds(regenHealthRateTime);
        }
        regen = null;
    }
}
