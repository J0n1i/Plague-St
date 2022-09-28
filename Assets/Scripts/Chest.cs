using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Chest : MonoBehaviour
{
   public Inventory playerInventory;
   public SignalSender chestSignal;
   private Animator animator;
   private bool isOpen;
   public GameObject HeartDrop;
   void Start()
    {
        animator = GetComponent<Animator>();
        chestSignal.Raise();
        isOpen=false;
    }
 //OnTriggerEnter2D is called when the Collider2D other enters the trigger (2D physics only)
    void OnTriggerEnter2D(Collider2D other)
    {
    if (other.CompareTag("Player") && !other.isTrigger)
    {
        if(playerInventory.coins > 9 && isOpen==false)
        {
          StartCoroutine(ChestOpenCo());
        } else {
            return;
        }
    
    }
    }
    private IEnumerator ChestOpenCo()
    {

        animator.SetBool("isOpened", true);
         Instantiate(HeartDrop, transform.position, Quaternion.identity);
        isOpen=true;
            playerInventory.coins -= 10;
            chestSignal.Raise();
            yield return new WaitForSeconds(.3f);
            this.GetComponent<BoxCollider2D>().enabled = false;
            
        
      
    }
}
