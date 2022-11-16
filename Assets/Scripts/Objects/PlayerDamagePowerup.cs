using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerDamagePowerup : Powerup
{
 public Inventory playerInventory;
public void OnTriggerEnter2D(Collider2D other)
    { 

        if(other.gameObject.CompareTag("Player") && !other.isTrigger)
        {
            playerInventory.damagePowerup += 1;
            powerupSignal.Raise();
            powerupEffectSignal.Raise();
            Destroy(this.gameObject);
        }

    }
}
