using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Tutorial : MonoBehaviour
{
    public GameObject[] popUpBox;
    public Animator animator;
    public TMP_Text popUpText;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    void OnTriggerEnter2D (Collider2D other)
    {
        if(other.gameObject.CompareTag("Player") && !other.isTrigger)
        {
            popUpBox[0].SetActive(true);
        }
    }
    void OnTriggerExit2D (Collider2D other)
    {
        if(other.gameObject.CompareTag("Player") && !other.isTrigger)
        {
            popUpBox[0].SetActive(false);
        }
    }
}
