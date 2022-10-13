using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour
{
    private GameManager gameManager;
    public GameObject PauseMenuUi;
    public SignalSender healthResetSignal;

    void Start(){
        gameManager = GameObject.FindGameObjectWithTag("GameManager").GetComponent<GameManager>();
    }



    public void Resume ()
    {
        PauseMenuUi.SetActive(false);
        gameManager.PauseGame(false);
    }

    public void Pause ()
    {
        PauseMenuUi.SetActive(true);
        gameManager.PauseGame(true);
    }

    public void GoMenu ()
    {
        healthResetSignal.Raise();
        Time.timeScale = 1f;
        SceneManager.LoadScene(0);
    }



}
