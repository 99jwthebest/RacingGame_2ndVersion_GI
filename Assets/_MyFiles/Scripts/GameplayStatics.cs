using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameplayStatics : MonoBehaviour
{
    public static GameplayStatics instance;

    public static int levelSelect = 1;
    public static int carSelect = 1;

    private void Awake()
    {
        instance = this;
        DontDestroyOnLoad(gameObject);
    }

    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    public void AddLevelSelect()
    {
        levelSelect++;
        Debug.Log(levelSelect);
    }

    public void SubtractLevelSelect()
    {
        levelSelect--;
        Debug.Log(levelSelect);
    }

    public int GetLevelSelect() { return levelSelect; }
    public int SetLevelSelect(int value) 
    { 
        levelSelect = value;
        return levelSelect; 
    }


    public void AddCarSelect()
    {
        carSelect++;
        Debug.Log(carSelect);
    }

    public void SubtractCarSelect()
    {
        carSelect--;
        Debug.Log(carSelect);
    }

    public int GetCarSelect() { return carSelect; }
    public int SetCarSelect(int value)
    {
        carSelect = value;
        return carSelect;
    }

}
