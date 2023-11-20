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
    public int currentLaps;
    public int totalLaps;

    [Space(10f)]
    [Header("Times For Race")]
    public int goldTime;
    public int silverTime;
    public int bronzeTime;


    private void Awake()
    {
        instance = this;
    }

    void Start()
    {
        
    }

    private void Update()
    {
        CheckGameState();
    }

    void CheckGameState()
    {
        if(currentCheckPoint >= totalAmountCheckpoints)
        {
            currentLaps++;
            currentCheckPoint = 0;

            CheckLaps();
        }
    }

    void CheckLaps()
    {
        if(currentLaps >= totalLaps)
        {
            CheckTimeElapsed();
            EndRace();
            CountupTimer.Instance.StopTime();
        }
    }

    void CheckTimeElapsed()
    {

        if(CountupTimer.Instance.GetCurrentTime() < goldTime)
        {
            UIManager.instance.SetWinMenuResults("You got Gold!!");
        }
        else if(CountupTimer.Instance.GetCurrentTime() < silverTime)
        {
            UIManager.instance.SetWinMenuResults("You got Silver!!");
        }
        else if(CountupTimer.Instance.GetCurrentTime() < bronzeTime)
        {
            UIManager.instance.SetWinMenuResults("You got Bronze!!");
        }
        else
        {
            UIManager.instance.SetWinMenuResults("You FAILED!!!");
        }
    }

    void EndRace()
    {
        Debug.Log("You cleared Checkpoints, boy!!!!!");
        winMenuUI.SetActive(true);
    }

}
