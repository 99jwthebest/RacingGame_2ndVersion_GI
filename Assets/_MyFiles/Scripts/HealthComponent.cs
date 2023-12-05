using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class HealthComponent : MonoBehaviour
{
    [SerializeField]
    CameraController cameraController;
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
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void CarHit(ColliderHit collider)
    {
        if (collider.collider.transform.tag == "Player")
        {
            TakeDamage(20);
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
        cameraController.carHitCamShake.StopShake();
        
        AIController aiC = GetComponent<AIController>();
        aiC.SetDownForceValue(0);
        Rigidbody rb = GetComponent<Rigidbody>();

        if(torque)
            rb.AddTorque(gameObject.transform.position * crashForce, ForceMode.Impulse);
        else
            rb.AddRelativeTorque(gameObject.transform.position * crashForce, ForceMode.Impulse);

        CameraLookAtCarCrash();

    }

    void CameraLookAtCarCrash()
    {
        //GameObject player = FindObjectOfType<CameraController>().gameObject;
        Camera.main.GetComponent<CameraController>().CameraLookAtCarCrash(gameObject.transform);


        //gameObject.transform.position = Vector3.Lerp(transform.position, driftCamConstraintLeft.transform.position, driftCamSpeed * Time.deltaTime);
        //Camera.main.transform.LookAt(gameObject.transform.position);
        //cameraShaker.StartShake();
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
