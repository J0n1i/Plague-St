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

    void Start(){
        saveManager = GameObject.Find("SaveManager").GetComponent<SaveManager>();

        saveManager.LoadGame();
        musicSlider.value = saveManager.saveData.musicVolume;
        sfxSlider.value = saveManager.saveData.sfxVolume;
    }

    public void SetMusicVolume(float volume)
    {
        audioMixer.SetFloat("Music", Mathf.Log(volume) * 20);
    }

    public void SetSFXVolume(float volume)
    {
        audioMixer.SetFloat("SFX", Mathf.Log(volume) * 20);
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
        saveManager.saveData.sfxVolume = sfxSlider.value;
        saveManager.saveData.musicVolume = musicSlider.value;
        saveManager.SaveGame();
    }





}
