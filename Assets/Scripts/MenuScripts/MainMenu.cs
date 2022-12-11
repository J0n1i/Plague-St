using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{

    public Animator anime, fading;
    public Inventory playerInventory;
    private GameObject player;
    

    public void PlayGame()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        anime.SetBool("Onclick", true);
        fading.SetBool("Fade", true);
        if(player != null){
            Destroy(player);
        }
        
        StartCoroutine(Startgame());
        
           
    }

    private IEnumerator Startgame()
    {
        yield return new WaitForSeconds(.7f);
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
        
        
    }
    
}
