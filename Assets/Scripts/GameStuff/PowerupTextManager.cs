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
    private float baseStat;

    private void Start()
    {
        playerInventory.speedPowerup = 0;
         playerInventory.rollCooldownPowerup = 0;
         playerInventory.damagePowerup = 0;
         baseStat=1;
         playerDamage.RuntimeValue = playerDamage.initialValue;
         specialDamage.RuntimeValue = specialDamage.initialValue;
    }

    public void UpdateSpeedPowerupCount()
    {
        float newStat = 0;
        newStat +=baseStat + playerInventory.speedPowerup * 0.1f;
        speedPowerupDisplay.text = newStat.ToString("F1");

    }
    public void UpdateRollCooldown()
    {
        float newStat = 0;
        newStat += baseStat - playerInventory.rollCooldownPowerup * 0.1f;
        if(newStat <= 0.1f){
            newStat = 0.1f;
        }
        rollCooldownDisplay.text = newStat.ToString("F1");
    }
    public void UpdateDamagePowerup()
    {
        playerDamage.RuntimeValue += playerDamage.initialValue * 1;
        specialDamage.RuntimeValue += specialDamage.initialValue * 1;
        damagePowerupDisplay.text = playerDamage.RuntimeValue + ".0";
        print(playerDamage.RuntimeValue);
        print(specialDamage.RuntimeValue);
    }

}