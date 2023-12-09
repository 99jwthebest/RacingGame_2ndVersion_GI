using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonFunctions : MonoBehaviour
{
    public Transform trackImages;
    public int totalAmountOfTracks;
    public Transform carImages;
    public int totalAmountOfCars;


    private void Awake()
    {
        totalAmountOfTracks = trackImages.childCount;
        totalAmountOfCars = carImages.childCount;
    }

    void Update()
    {
        
    }

    public void IncrementLevelSelect()
    {
        GameplayStatics.instance.AddLevelSelect();
        if(GameplayStatics.instance.GetLevelSelect() > totalAmountOfTracks)
        {
            GameplayStatics.instance.SetLevelSelect(1);
            Debug.Log("What the flying FUCKER!!");
            Debug.Log(GameplayStatics.instance.GetLevelSelect());
        }

        foreach(Transform t in trackImages.transform)
        {
            t.gameObject.SetActive(false);
        }

        trackImages.GetChild(GameplayStatics.instance.GetLevelSelect() - 1).gameObject.SetActive(true);
    }

    public void DecrementLevelSelect()
    {
        GameplayStatics.instance.SubtractLevelSelect();
        if (GameplayStatics.instance.GetLevelSelect() < 1)
        {
            GameplayStatics.instance.SetLevelSelect(totalAmountOfTracks);
            Debug.Log(GameplayStatics.instance.GetLevelSelect());

        }

        foreach (Transform t in trackImages.transform)
        {
            t.gameObject.SetActive(false);
        }

        trackImages.GetChild(GameplayStatics.instance.GetLevelSelect() - 1).gameObject.SetActive(true);
    }

    public void IncrementCarSelect()
    {
        GameplayStatics.instance.AddCarSelect();
        if (GameplayStatics.instance.GetCarSelect() > totalAmountOfCars)
        {
            GameplayStatics.instance.SetCarSelect(1);
            Debug.Log("What the flying FUCKER!!");
            Debug.Log(GameplayStatics.instance.GetCarSelect());
        }

        foreach (Transform t in carImages.transform)
        {
            t.gameObject.SetActive(false);
        }

        carImages.GetChild(GameplayStatics.instance.GetCarSelect() - 1).gameObject.SetActive(true);
    }

    public void DecrementCarSelect()
    {
        GameplayStatics.instance.SubtractCarSelect();
        if (GameplayStatics.instance.GetCarSelect() < 1)
        {
            GameplayStatics.instance.SetCarSelect(totalAmountOfCars);
            Debug.Log(GameplayStatics.instance.GetCarSelect());

        }

        foreach (Transform t in carImages.transform)
        {
            t.gameObject.SetActive(false);
        }

        carImages.GetChild(GameplayStatics.instance.GetCarSelect() - 1).gameObject.SetActive(true);
    }
}
