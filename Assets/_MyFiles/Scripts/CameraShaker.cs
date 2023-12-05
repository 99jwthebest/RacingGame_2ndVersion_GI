using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraShaker : MonoBehaviour
{
    [SerializeField] float shakeDuration = 0.5f;
    [SerializeField] float shakeMagnitude = 2f;
    bool shaking;

    public void StartShake()
    {
        if (!shaking)
        {
            StartCoroutine(ShakeCoroutine());
        }
    }
    public void StopShake()
    {
        shaking = false;    
    }

    IEnumerator ShakeCoroutine()
    {
        shaking = true;
        yield return new WaitForSeconds(shakeDuration);
        shaking = false;
    }

    private void LateUpdate()
    {
        if (shaking) // ask professor Li how I can make this more fit to my game
        {
            Vector3 shakeAmt = new Vector3(Random.value, Random.value, Random.value) * shakeMagnitude * (Random.value > 0.5 ? 1 : -1);
            transform.localPosition += shakeAmt;
        }
        else
        {
            //transform.localPosition = Vector3.zero;
        }
    }
}
