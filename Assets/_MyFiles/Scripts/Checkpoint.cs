using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Checkpoint : MonoBehaviour
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
        if(collider.transform.tag == "Player")
        {
            GameManager.instance.currentCheckPoint++;

            if(GameManager.instance.currentCheckPoint < GameManager.instance.totalAmountCheckpoints)
                GameManager.instance.TurnOnNextCheckpoint();    
            
            this.gameObject.SetActive(false);
        }
    }
}
