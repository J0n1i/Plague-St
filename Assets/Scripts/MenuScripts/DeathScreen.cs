using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class DeathScreen : MonoBehaviour
{

    public GameObject deathScreenUi;
    public SignalSender healthResetSignal;
    public GameObject[] DeathControls;
    public GameObject PauseButton;


    public void Awake()
    {
        DeathControls = GameObject.Find("PauseMenuCanvas").GetComponent<PauseMenu>().Controls;
    }

        public void HideDeathScreen ()
    {
        Debug.Log("A");
        Time.timeScale = 1f;

        StartCoroutine(Revive());
    }


    public void ShowDeathScreen ()
    {
        StartCoroutine(Death());
    }

    private IEnumerator Death()
    {
        yield return new WaitForSeconds(.5f);
        Time.timeScale = 0f;
        deathScreenUi.SetActive(true);
        PauseButton.SetActive(false);

        for (int i = 0; i < DeathControls.Length; i++)
        {
            DeathControls[i].SetActive(false);
        }

    }


        private IEnumerator Revive()
    {
        Debug.Log("B"); 
        yield return new WaitForSeconds(.5f);
        deathScreenUi.SetActive(false);
        PauseButton.SetActive(true);

        for (int i = 0; i < DeathControls.Length; i++)
        {
            DeathControls[i].SetActive(true);
        }

    }

    public void Menu()
    {
        healthResetSignal.Raise();
        Time.timeScale = 1f;
        SceneManager.LoadScene(0);
    }

    public void Restart()
    {
        FindObjectOfType<LevelMusic>().SceneMusic();
        healthResetSignal.Raise();
        Time.timeScale = 1f;
        SceneManager.LoadScene(1);
        deathScreenUi.SetActive(false);




    }

    void Update()
    {
        
    }
}
