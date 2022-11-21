using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour
{
    private GameManager gameManager;
    public GameObject PauseMenuUi;
    public SignalSender healthResetSignal;
    public GameObject MenuWarningUi;
    public GameObject[] Controls;

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


    public void MenuWarning ()
    {
        PauseMenuUi.SetActive(false);
        MenuWarningUi.SetActive(true);

        for (int i = 0; i < Controls.Length; i++)
        {
            Controls[i].SetActive(false);
        }


    }

    public void MenuWarningGoBack ()
    {
        PauseMenuUi.SetActive(true);
        MenuWarningUi.SetActive(false);


        for (int i = 0; i < Controls.Length; i++)
        {
            Controls[i].SetActive(true);
        }

    }



}
