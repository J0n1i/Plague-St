using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeartExtra : Powerup
{
     public FloatValue playerHealth;
      public FloatValue heartContainers;

public void OnTriggerEnter2D(Collider2D other)
    { 

        if(other.gameObject.CompareTag("Player") && !other.isTrigger)
        {
            if(heartContainers.RuntimeValue == 10){
                print("too many hearts");
                Destroy(this.gameObject);
                return;
            }
            heartContainers.RuntimeValue += 1;
            playerHealth.RuntimeValue += 2;
            print(heartContainers.RuntimeValue);
            powerupSignal.Raise();
            powerupEffectSignal.Raise();
            Destroy(this.gameObject);
        }

    }
}