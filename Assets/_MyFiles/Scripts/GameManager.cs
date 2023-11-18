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
            CheckTimeElapsed();
            EndRace();
            CountupTimer.Instance.StopTime();
        }
    }

    void EndRace()
    {
        Debug.Log("You cleared Checkpoints, boy!!!!!");
        winMenuUI.SetActive(true);
    }

    void CheckTimeElapsed()
    {

        if(CountupTimer.Instance.GetCurrentTime() < 15)
        {
            UIManager.instance.SetWinMenuResults("You got Gold!!");
            Debug.Log("You got Gold!!");
        }
        else
        {
            UIManager.instance.SetWinMenuResults("You FAILED!!!");
            Debug.Log("You FAILED!!!");
        }
    }

}
