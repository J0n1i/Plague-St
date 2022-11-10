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
        newStat +=baseStat + playerInventory.speedPowerup * 0.2f;
        speedPowerupDisplay.text = newStat.ToString("F1");

    }
    public void UpdateRollCooldown()
    {
        float newStat = 0;
        newStat += baseStat - playerInventory.rollCooldownPowerup * 0.2f;
        if(newStat <= 0.2f){
            newStat = 0.2f;
        }
        rollCooldownDisplay.text = newStat.ToString("F1");
    }
    public void UpdateDamagePowerup()
    {
        playerDamage.RuntimeValue += 0.5f;
        specialDamage.RuntimeValue += specialDamage.initialValue * 1;
        damagePowerupDisplay.text = playerDamage.RuntimeValue + "";
        print(playerDamage.RuntimeValue);
        print(specialDamage.RuntimeValue);
    }

}