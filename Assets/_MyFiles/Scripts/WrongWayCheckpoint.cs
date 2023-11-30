using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WrongWayCheckpoint : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider collider)
    {
        if (collider.transform.tag == "Player")
        {

            this.gameObject.SetActive(false);
        }
    }
}
