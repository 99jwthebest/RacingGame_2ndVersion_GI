using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UIManager : MonoBehaviour
{
    public static UIManager instance;

    public controller_FV pController_FV;

    public GameObject needle;
    public TextMeshProUGUI kphText;
    public TextMeshProUGUI gearNum;
    public Slider nitrousSlider;
    private float startPosition = 199f, endPosition = -19f;
    private float desiredPosition;


    private void Awake()
    {
        instance = this;
    }

    void Start()
    {

    }

    private void FixedUpdate()
    {
        kphText.text = pController_FV.KPH.ToString("0");
        UpdateNeedle();
        NitrousUI();
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
        nitrousSlider.value = pController_FV.nitrousValue / 15.6f;
    }
}
