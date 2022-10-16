using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpecialCharge : Powerup
{
 public Inventory playerInventory;
public void OnTriggerEnter2D(Collider2D other)
    { 

        if(other.gameObject.CompareTag("Player") && !other.isTrigger)
        {
            playerInventory.specialCharge += 1;
            powerupSignal.Raise();
            Destroy(this.gameObject);
        }

    }
}