using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class SettingsMenu : MonoBehaviour
{
    [SerializeField] private AudioMixer audioMixer;
    [SerializeField] private Slider musicSlider, sfxSlider;

    private SaveManager saveManager;

    public GameObject MainMenu;
    public GameObject ModifyMenu;
    public Slider sliderVolume;
    public float VolumeLevel;
    public Text VolumeText;
    public string Volume;


    public void ShowSettings ()
    {
        MainMenu.SetActive(false);
        ModifyMenu.SetActive(true);
    }

    public void HideSettings ()
    {
        MainMenu.SetActive(true);
        ModifyMenu.SetActive(false);
        saveManager.saveData.sfxVolume = sfxSlider.value;
        saveManager.saveData.musicVolume = musicSlider.value;
        saveManager.SaveGame();
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
