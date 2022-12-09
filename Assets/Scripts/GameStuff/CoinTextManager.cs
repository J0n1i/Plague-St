using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class CoinTextManager : MonoBehaviour
{
    public Inventory playerInventory;
    public TextMeshProUGUI coinDisplay;
    private GameObject player;

    private void Start()
    {
        playerInventory.coins = 0;
        player = GameObject.FindGameObjectWithTag("Player");
        if(player.GetComponent<PlayerMovement>().coins != 0)
        {
            playerInventory.coins = player.GetComponent<PlayerMovement>().coins;
        }
    }

    public void UpdateCoinCount()
    {
        coinDisplay.text = "" + playerInventory.coins;
    }
}