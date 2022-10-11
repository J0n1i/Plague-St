using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class PowerupTextManager : MonoBehaviour
{
    public Inventory playerInventory;
    public TextMeshProUGUI speedPowerupDisplay;
    public TextMeshProUGUI rollCooldownDisplay;

    private void Start()
    {
        playerInventory.speedPowerup = 0;
         playerInventory.rollCooldownPowerup = 0;
    }

    public void UpdateSpeedPowerupCount()
    {
        speedPowerupDisplay.text = "+" + playerInventory.speedPowerup * 10 + "%";
    }
    public void UpdateRollCooldown()
    {
        rollCooldownDisplay.text = "" + playerInventory.rollCooldownPowerup;
    }
}