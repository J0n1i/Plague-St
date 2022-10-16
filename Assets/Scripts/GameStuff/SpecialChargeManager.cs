using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
public class SpecialChargeManager : MonoBehaviour
{
    public Inventory playerInventory;
    public TextMeshProUGUI specialChargeDisplay;
    public Image specialCooldownNotAvailable;

    private void Start()
    {
        playerInventory.specialCharge = 0;
    }
    public void DecreaseSpecialCharge()
    {
        playerInventory.specialCharge -= 1;
        UpdateSpecialCharge();
    }
     public void UpdateSpecialCharge()
    {
        specialChargeDisplay.text = "" + playerInventory.specialCharge;
        if(playerInventory.specialCharge == 0)
        {
            specialCooldownNotAvailable.enabled = true;
        } else
        {
            specialCooldownNotAvailable.enabled = false;
        }
    }
}