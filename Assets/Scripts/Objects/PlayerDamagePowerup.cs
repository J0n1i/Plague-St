using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerDamagePowerup : Powerup
{
        public Inventory playerInventory;
        public AudioClip pickUp;
public void OnTriggerEnter2D(Collider2D other)
    { 

        if(other.gameObject.CompareTag("Player") && !other.isTrigger)
        {
            playerInventory.damagePowerup += 1;
            powerupSignal.Raise();
            powerupEffectSignal.Raise();
            AudioPlayer.instance.PlaySound(pickUp, 1f);
            Destroy(this.gameObject);
            
        }

    }
}
