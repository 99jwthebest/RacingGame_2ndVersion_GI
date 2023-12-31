using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UIManager : MonoBehaviour
{
    public static UIManager instance;

    [SerializeField]
    CarPositions carPositions;
    [SerializeField] 
    controller_FV pController_FV;
    [SerializeField]
    PositionHolder pPositionHolder;
    [SerializeField]
    GameManager gameManager;
    [SerializeField]
    ZoneAbility zoneAbility;

    public GameObject needle;
    public TextMeshProUGUI kphText;
    public TextMeshProUGUI gearNum;
    public Slider nitrousSlider;
    public Slider zoneSlider;
    [SerializeField]
    Image zoneImage;
    private float startPosition = 199f, endPosition = -19f;
    private float desiredPosition;
    public TextMeshProUGUI carPositionText;
    public TextMeshProUGUI lapNumberText;


    public TextMeshProUGUI winMenuText;


    private void Awake()
    {
        instance = this;
    }

    void Start()
    {
        pController_FV = FindObjectOfType<controller_FV>();
        pPositionHolder = pController_FV.GetComponent<PositionHolder>();
        zoneAbility = pController_FV.GetComponent<ZoneAbility>();

    }

    private void FixedUpdate()
    {
        kphText.text = pController_FV.KPH.ToString("0");
        UpdateNeedle();
        NitrousUI();
        ZoneUI();
        CarPositionUI();
        LapNumberUI();
    }

    public void UpdateNeedle()
    {
        desiredPosition = startPosition - endPosition;
        float temp = pController_FV.engineRPM / 10000;
        needle.transform.eulerAngles = new Vector3(0, 0, (startPosition - temp * desiredPosition));

    }

    public void ChangeGear()
    {
        gearNum.text = (!pController_FV.reverse) ? (pController_FV.gearNum + 1).ToString() : "R";

    }

    public void NitrousUI()
    {
        nitrousSlider.value = pController_FV.GetNitrousValue() / 15.6f;
    }

    public void ZoneUI()
    {
        zoneSlider.value = zoneAbility.GetZoneValue() / 46.75f;
    }

    public Image SetZoneImage(bool zoneActivated)
    {
        if(zoneActivated)
        {
            zoneImage.enabled = true;
            return zoneImage;
        }

        zoneImage.enabled = false;
        return zoneImage;
    }

    public void CarPositionUI()
    {
        carPositionText.text = pPositionHolder.currentPosition + " / " + carPositions.positionHolders.Count;
    }

    public void LapNumberUI()
    {
        lapNumberText.text = gameManager.currentLap + " / " + gameManager.totalLaps;
    }


    public void SetWinMenuResults(string winText)
    {
        winMenuText.text = winText;
    }

}
