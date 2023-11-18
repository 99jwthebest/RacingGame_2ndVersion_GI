using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    public controller_FV pController_FV;


    public int checkPoint;

    private void Awake()
    {
        instance = this;
    }

    void Start()
    {
        
    }

    private void FixedUpdate()
    {

    }

}
