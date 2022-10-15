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
    public TextMeshProUGUI damagePowerupDisplay;
    public FloatValue playerDamage;
    public FloatValue specialDamage;

    private void Start()
    {
        playerInventory.speedPowerup = 0;
         playerInventory.rollCooldownPowerup = 0;
         playerInventory.damagePowerup = 0;
         playerDamage.RuntimeValue = playerDamage.initialValue;
         specialDamage.RuntimeValue = specialDamage.initialValue;
    }

    public void UpdateSpeedPowerupCount()
    {
        speedPowerupDisplay.text = "+" + playerInventory.speedPowerup * 10 + "%";
    }
    public void UpdateRollCooldown()
    {
        rollCooldownDisplay.text = "-" + playerInventory.rollCooldownPowerup * 10 + "%";
    }
    public void UpdateDamagePowerup()
    {
        damagePowerupDisplay.text = "+" + playerInventory.damagePowerup + "x";
        playerDamage.RuntimeValue += playerDamage.initialValue * 1;
        specialDamage.RuntimeValue += specialDamage.initialValue * 1;
        print(playerDamage.RuntimeValue);
        print(specialDamage.RuntimeValue);
    }
}