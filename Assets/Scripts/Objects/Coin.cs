using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Coin : Powerup
{
    public Inventory playerInventory;
    public AudioClip coinPickUp;
    //public AudioSource coinSound;
    // Start is called before the first frame update
    void Start()
    {
        powerupSignal.Raise();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player") && !other.isTrigger)
        {
            //coinSound.Play();
            playerInventory.coins += 1;
            powerupSignal.Raise();
            Destroy(this.gameObject);
            AudioPlayer.instance.PlaySound(coinPickUp, 1f);
        }
    }
}