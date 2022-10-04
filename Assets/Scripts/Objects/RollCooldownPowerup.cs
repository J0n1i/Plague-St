using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RollCooldownPowerup : Powerup
{
 
public void OnTriggerEnter2D(Collider2D other)
    { 

        if(other.gameObject.CompareTag("Player") && !other.isTrigger)
        {
            
            powerupSignal.Raise();
            Destroy(this.gameObject);
        }

    }
}