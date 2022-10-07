using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class DeathScreen : MonoBehaviour
{

    public GameObject deathScreenUi;
    public SignalSender healthResetSignal;

    public void ShowDeathScreen ()
    {
        StartCoroutine(Death());
    }

    private IEnumerator Death()
    {
        yield return new WaitForSeconds(.5f);
        Time.timeScale = 0f;
        deathScreenUi.SetActive(true);
    }

    public void Menu()
    {
        healthResetSignal.Raise();
        Time.timeScale = 1f;
        SceneManager.LoadScene(0);
    }

    public void Restart()
    {
        healthResetSignal.Raise();
        Time.timeScale = 1f;
        SceneManager.LoadScene(1);
    }

    void Update()
    {
        
    }
}
