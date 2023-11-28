using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarPositions : MonoBehaviour
{
    public GameObject player;
    public GameObject[] aiVehicles;
    public List<GameObject> allVehiclesRacing;
    public List<PositionHolder> positionHolders;


    private void Awake()
    {
        player = FindObjectOfType<controller_FV>().gameObject;

        aiVehicles = GameObject.FindGameObjectsWithTag("AI"); 

        allVehiclesRacing = new List<GameObject>();

        positionHolders = new List<PositionHolder>();



        foreach (GameObject vehicle in aiVehicles) 
        { 
            allVehiclesRacing.Add(vehicle);
        }
        allVehiclesRacing.Add(player);

        function();

        StartCoroutine(TimedLoop());
    }


    // Update is called once per frame
    void Update()
    {
        
    }


    void function()
    {
        //int i = 0;
        foreach (GameObject vehicle in allVehiclesRacing)
        {
            PositionHolder positionHolder = vehicle.GetComponent<PositionHolder>();
            
            positionHolders.Add(positionHolder);
            //numbers[i] = positionHolder.currentPosition;
            //i++;
        }



    }


    private void sortArray()
    {

        //for (int i = 0; i < positionHolders.Count; i++)
        //{
        //    //presentVehicles[i].hasFinished = fullArray[i].GetComponent<controller>().hasFinished;
        //    //presentVehicles[i].name = fullArray[i].GetComponent<controller>().carName;
        //    numbers[i].node = positionHolders[i].GetComponent<PositionHolder>().currentPosition;
        //}

            for (int i = 0; i < positionHolders.Count; i++)
            {
                for (int j = i + 1; j < positionHolders.Count; j++)
                {
                    if (positionHolders[j].totalWaypoint < positionHolders[i].totalWaypoint)
                    {
                        PositionHolder QQ = positionHolders[i];
                        positionHolders[i] = positionHolders[j];
                        positionHolders[j] = QQ;
                    }
                }
            }


        //if (arrarDisplayed)
        //    for (int i = 0; i < temporaryArray.Length; i++)
        //    {
        //        temporaryArray[i].transform.Find("vehicle node").gameObject.GetComponent<Text>().text = (presentVehicles[i].hasFinished) ? "finished" : "";
        //        temporaryArray[i].transform.Find("vehicle name").gameObject.GetComponent<Text>().text = presentVehicles[i].name.ToString();
        //    }

        positionHolders.Reverse();

        for(int i = 0; i < positionHolders.Count;i++)
        {
            positionHolders[i].currentPosition = i + 1;
        }


        //for (int i = 0; i < temporaryArray.Length; i++)
        //{
        //    if (RR.carName == presentVehicles[i].name)
        //        currentPosition.text = ((i + 1) + "/" + presentVehicles.Count).ToString();
        //}



    }




    private IEnumerator TimedLoop()
    {
        while (true)
        {
            yield return new WaitForSeconds(.7f);

            sortArray();

        }
    }
}
