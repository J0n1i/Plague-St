using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{

    public Animator anime;


    public void PlayGame()
    {
        anime.SetBool("Onclick", true);
        StartCoroutine(Startgame());
        
           
    }

    private IEnumerator Startgame()
    {
        yield return new WaitForSeconds(.5f);
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }
}
