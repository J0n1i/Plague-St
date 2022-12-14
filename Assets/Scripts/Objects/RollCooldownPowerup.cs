using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RollCooldownPowerup : Powerup
{
 public Inventory playerInventory;
public void OnTriggerEnter2D(Collider2D other)
    { 

        if(other.gameObject.CompareTag("Player") && !other.isTrigger)
        {
            playerInventory.rollCooldownPowerup += 1;
            powerupSignal.Raise();
            powerupEffectSignal.Raise();
            Destroy(this.gameObject);
        }

    }
}