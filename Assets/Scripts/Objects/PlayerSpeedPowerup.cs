using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSpeedPowerup : Powerup
{

public void OnTriggerEnter2D(Collider2D other)
    {
        if(other.CompareTag("Player") && !other.isTrigger)
        {
            powerupSignal.Raise();
            Destroy(this.gameObject);
        }
    }

}