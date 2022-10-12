using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{

    public Animator anime, fading;
    

    public void PlayGame()
    {
        anime.SetBool("Onclick", true);
        fading.SetBool("Fade", true);
        StartCoroutine(Startgame());
        
           
    }

    private IEnumerator Startgame()
    {
        yield return new WaitForSeconds(.7f);
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
        
        
    }
    
}
