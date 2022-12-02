using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class tutorialScript : MonoBehaviour
{
    public GameObject enemy;
    public GameObject enemy2;
    public Image image;
    public Image image2;
    public Image image3;
    public GameObject coinholder;
    public GameObject chest;
    public GameObject chestSquare;
    public Image image4;
    public Image image8;
    public Image image9;
    public Image image10;
    public GameObject door;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
       
        if(enemy.GetComponent<BoxCollider2D>().enabled == false && enemy2.activeSelf == false){
              image.enabled = false;
                image2.enabled = false;
                image3.enabled = true;
                coinholder.SetActive(true);
                chest.SetActive(true);
                chestSquare.SetActive(true);
                image4.enabled = true;
        }
        if(chest.GetComponent<BoxCollider2D>().enabled == false){
            chest.SetActive(false);
            image3.enabled = false;
                coinholder.SetActive(false);
                chestSquare.SetActive(false);
                image4.enabled = false;
                image8.enabled = true;
                image9.enabled = true;
                image10.enabled = true;
                door.SetActive(false);
        }
    }
}
