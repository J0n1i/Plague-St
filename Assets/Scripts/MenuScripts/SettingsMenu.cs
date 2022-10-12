using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SettingsMenu : MonoBehaviour
{

    public GameObject MainMenu;
    public GameObject ModifyMenu;

    public void ShowSettings ()
    {
        MainMenu.SetActive(false);
        ModifyMenu.SetActive(true);
    }

    public void HideSettings ()
    {
        MainMenu.SetActive(true);
        ModifyMenu.SetActive(false);
    }





}
