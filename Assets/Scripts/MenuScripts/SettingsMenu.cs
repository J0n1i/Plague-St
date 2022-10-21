using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;
using TMPro;

public class SettingsMenu : MonoBehaviour
{
    [SerializeField] private AudioMixer audioMixer;
    [SerializeField] private Slider musicSlider, sfxSlider;

    private SaveManager saveManager;

    [SerializeField] private GameObject MainMenu;
    [SerializeField] private GameObject ModifyMenu;
    [SerializeField] private TMP_Text VolumeText;
    [SerializeField] private TMP_Text MusicText;


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
        VolumeText.text = (int)(sfxSlider.value * 100f) + "%";
    }

    public void MusicBar()
    {
        MusicText.text = (int)(musicSlider.value * 100f) + "%";
    }




}
