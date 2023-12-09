using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{

    void Start()
    {
        GameplayStatics.instance.SetCarSelect(1);
        GameplayStatics.instance.SetLevelSelect(1);
    }

    void Update()
    {
        
    }

    public void PlayButton()
    {
        SceneManager.LoadScene(GameplayStatics.instance.GetLevelSelect());
    }

    public void QuitButton()
    {
        Application.Quit();
        Debug.Log("Qutting Game...");
    }

}
