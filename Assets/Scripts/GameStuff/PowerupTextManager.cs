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
    private GameObject player;

    private void Start()
    {
        playerInventory.speedPowerup = 0;
         playerInventory.rollCooldownPowerup = 0;
         playerInventory.damagePowerup = 0;
          player = GameObject.FindGameObjectWithTag("Player");
          baseStat=1;
         playerDamage.RuntimeValue = playerDamage.initialValue;
         specialDamage.RuntimeValue = specialDamage.initialValue;
         if(player.GetComponent<PlayerMovement>().spd != 0 || player.GetComponent<PlayerMovement>().cd != 0 || player.GetComponent<PlayerMovement>().dmg != 0)
         {
            playerInventory.speedPowerup =  player.GetComponent<PlayerMovement>().spd;
            playerInventory.rollCooldownPowerup =  player.GetComponent<PlayerMovement>().cd;
            playerInventory.damagePowerup =  player.GetComponent<PlayerMovement>().dmg;
            playerDamage.RuntimeValue = playerDamage.initialValue + playerInventory.damagePowerup * 0.5f;    
            speedPowerupDisplay.text = (baseStat + playerInventory.speedPowerup * 0.2).ToString("F1");
            rollCooldownDisplay.text = (baseStat - playerInventory.rollCooldownPowerup * 0.2).ToString("F1");
            damagePowerupDisplay.text = (playerDamage.RuntimeValue).ToString("F1");
            
         }
         
         
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