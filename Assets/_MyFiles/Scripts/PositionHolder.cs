using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PositionHolder : MonoBehaviour
{
    public InputManager inputManager;
    public AIController aiController;
    public int currentWaypoint;
    public int currentPosition;
    public int totalWaypoint = 0;

    private void Awake()
    {
        if (gameObject.tag != "Player")
        {
            aiController = GetComponent<AIController>();
            currentWaypoint = aiController.currentNode;
        }
        else
        {
            inputManager = GetComponent<InputManager>();
            currentWaypoint = inputManager.currentNode;

        }

        StartCoroutine(TimedLoop());

    }


    // Update is called once per frame
    void Update()
    {

    }


    private IEnumerator TimedLoop()
    {
        while (true)
        {
            yield return new WaitForSeconds(.7f);

            if (gameObject.tag != "Player")
            {
                int previousWaypoint = currentWaypoint;
                currentWaypoint = aiController.currentNode;

                if(currentWaypoint > previousWaypoint)
                {
                    totalWaypoint++;
                }

            }
            else
            {
                int previousWaypoint = currentWaypoint;
                currentWaypoint = inputManager.currentNode;

                if (currentWaypoint > previousWaypoint)
                {
                    totalWaypoint++;
                }
            }

        }
    }

}
