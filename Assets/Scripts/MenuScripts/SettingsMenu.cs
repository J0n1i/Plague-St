using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SettingsMenu : MonoBehaviour
{

    public GameObject MainMenu;
    public GameObject ModifyMenu;
    public Slider sliderVolume;
    public float VolumeLevel;
    public Text VolumeText;
    public string Volume;


    void Start()
    {
        VolumeText = GetComponent<Text>();
        VolumeBar();
    }
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

    public void VolumeBar ()
    {
        VolumeLevel = sliderVolume.value;
        Debug.Log(VolumeLevel);
        Volume = VolumeLevel.ToString();
    }

    public void MusicBar()
    {

    }




}
