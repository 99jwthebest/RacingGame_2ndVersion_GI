using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    public GameObject carPrefabToSpawn;
    public Transform carSpawnPoint;
    public CarsToSpawn carsToSpawn;
    public controller_FV pController_FV;
    [SerializeField] 
    PositionHolder positionHolder;

    public GameObject winMenuUI;

    public int currentCheckPoint;
    public int totalAmountCheckpoints;
    public int currentLap;
    public int totalLaps;
    public TrackWaypoints waypoints;
    [SerializeField]
    Transform checkpointParent;
    public List<Transform> nodes = new List<Transform>();

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
        carsToSpawn = FindObjectOfType<CarsToSpawn>();

        SpawnVehicle();
        totalAmountCheckpoints = checkpointParent.transform.childCount;
        pController_FV = FindObjectOfType<controller_FV>();
        positionHolder = pController_FV.GetComponent<PositionHolder>();
    }
    
    void SpawnVehicle()
    {
        GameplayStatics.instance.GetCarSelect();
        switch (GameplayStatics.instance.GetCarSelect())
        {
            case 1:
                carPrefabToSpawn = carsToSpawn.carsToSpawnArray[0];
                break;
            case 2:
                carPrefabToSpawn = carsToSpawn.carsToSpawnArray[1];
                break;
        }

        Instantiate(carPrefabToSpawn, carSpawnPoint.transform.position, carSpawnPoint.transform.rotation);
    }


    private void Update()
    {
        CheckGameState();
    }

    void CheckGameState()
    {
        if(currentCheckPoint >= totalAmountCheckpoints)
        {
            currentCheckPoint = 0;

            TurnOnNextCheckpoint();

            currentLap++;
            CheckLaps();
        }
    }

    void CheckLaps()
    {
        if(currentLap >= totalLaps)
        {
            //TimeTrialMode();
            RaceMode();
            EndRace();
            CountupTimer.Instance.SetTime(true);
        }
    }

    void TimeTrialMode()
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

    private IEnumerator TurnOnCheckpoints()
    {
        yield return new WaitForSeconds(1f);

        foreach (Transform checkpoint in checkpointParent)
        {
            checkpoint.transform.gameObject.SetActive(true);
        }
    }

    public void TurnOnNextCheckpoint()
    {
        checkpointParent.GetChild(currentCheckPoint).gameObject.SetActive(true);
    }

    void RaceMode()
    {
        if (positionHolder.currentPosition == 1)
        {
            UIManager.instance.SetWinMenuResults("1st Place!!");
        }
        else if (positionHolder.currentPosition == 2)
        {
            UIManager.instance.SetWinMenuResults("2nd Place!!");
        }
        else if (positionHolder.currentPosition == 3)
        {
            UIManager.instance.SetWinMenuResults("3rd Place!!");
        }
        else
        {
            UIManager.instance.SetWinMenuResults("You FAILED!!!");
        }
    }

    void EndRace()
    {
        Debug.Log("You cleared Checkpoints, boy!!!!!");
        GameObject player = pController_FV.gameObject;
        ZoneAbility zoneAbility = player.GetComponent<ZoneAbility>();
        zoneAbility.enabled = false;
        winMenuUI.SetActive(true);
    }


}
