using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour
{
    bool pauseMenuActive = false;
    [SerializeField]
    GameObject pauseMenuUI;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyUp(KeyCode.Escape))
        {
            pauseMenuActive = !pauseMenuActive;
            pauseMenuUI.SetActive(pauseMenuActive);
            CountupTimer.Instance.SetTime(pauseMenuActive);
        }
    }

    public void ResumeButton()
    {
        //SceneManager.LoadScene(1);
    }

    public void RestartButton()
    {
        SceneManager.LoadScene(1);
        Time.timeScale = CountupTimer.Instance.GetStartTimeScale();

    }

    public void MainMenuButton()
    {
        SceneManager.LoadScene(0);
        Time.timeScale = CountupTimer.Instance.GetStartTimeScale();

    }

    public void QuitButton()
    {
        Application.Quit();
        Debug.Log("Qutting Game...");
    }

}
