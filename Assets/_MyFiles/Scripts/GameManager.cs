using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    public controller_FV pController_FV;

    public GameObject winMenuUI;

    public int currentCheckPoint;
    public int totalAmountCheckpoints;

    private void Awake()
    {
        instance = this;
    }

    void Start()
    {
        
    }

    private void Update()
    {
        CheckIfCheckpointsCleared();
    }

    void CheckIfCheckpointsCleared()
    {
        if(currentCheckPoint >= totalAmountCheckpoints)
        {
            EndRace();
        }
    }

    void EndRace()
    {
        Debug.Log("You win, boy!!!!!");
        winMenuUI.SetActive(true);
        Time.timeScale = 0f;
    }

}