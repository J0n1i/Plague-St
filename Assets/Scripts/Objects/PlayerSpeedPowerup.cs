using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSpeedPowerup : Powerup
{
    public Inventory playerInventory;

public void OnTriggerEnter2D(Collider2D other)
    {
        if(other.CompareTag("Player") && !other.isTrigger)
        {
            playerInventory.speedPowerup += 1;
            powerupSignal.Raise();
            powerupEffectSignal.Raise();
            Destroy(this.gameObject);
        }
    }

}