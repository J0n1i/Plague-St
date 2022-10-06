using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour
{

    public static bool gameIsPaused = false;
    public GameObject PauseMenuUi;
    public SignalSender healthResetSignal;

    // Update is called once per frame
    void Update()
    {
        
    }


    public void Resume ()
    {
        PauseMenuUi.SetActive(false);
        Time.timeScale = 1f;
        gameIsPaused = false;
    }

    public void Pause ()
    {
        PauseMenuUi.SetActive(true);
        Time.timeScale = 0f;
        gameIsPaused = true;
    }

    public void GoMenu ()
    {
        healthResetSignal.Raise();
        Time.timeScale = 1f;
        SceneManager.LoadScene(0);
    }



}
