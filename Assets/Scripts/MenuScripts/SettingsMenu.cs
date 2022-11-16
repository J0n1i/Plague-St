using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Audio;
using UnityEngine.UI;
using TMPro;

public class SettingsMenu : MonoBehaviour
{
    [SerializeField] private AudioMixer audioMixer;
    [SerializeField] private Slider musicSlider, sfxSlider;

    private SaveManager saveManager;

    [SerializeField] private GameObject PauseMenuUi;
    [SerializeField] private GameObject SettingsBg;

    [SerializeField] private GameObject MainMenu;
    [SerializeField] private GameObject ModifyMenu;
    [SerializeField] private TMP_Text VolumeText;
    [SerializeField] private TMP_Text MusicText;

    void Awake()
    {
        saveManager = GameObject.Find("SaveManager").GetComponent<SaveManager>();
    }

    public void ShowSettings()
    {
        Scene currentScene = SceneManager.GetActiveScene();
        int buildIndex = currentScene.buildIndex;
        Debug.Log(buildIndex);

        if (buildIndex == 0)
        {
            MainMenu.SetActive(false);
            ModifyMenu.SetActive(true);
        }
        else if (buildIndex == 1)
        {
            PauseMenuUi.SetActive(false);
            ModifyMenu.SetActive(true);
            SettingsBg.SetActive(true);
        }

        
    }

    public void HideSettings()
    {

        Scene currentScene = SceneManager.GetActiveScene();
        int buildIndex = currentScene.buildIndex;

        if (buildIndex == 0)
        {
            MainMenu.SetActive(true);
            ModifyMenu.SetActive(false);
            saveManager.saveData.sfxVolume = sfxSlider.value;
            saveManager.saveData.musicVolume = musicSlider.value;
            saveManager.SaveGame();
        }
        else if (buildIndex == 1)
        {
            PauseMenuUi.SetActive(true);
            ModifyMenu.SetActive(false);
            SettingsBg.SetActive(false);
            saveManager.saveData.sfxVolume = sfxSlider.value;
            saveManager.saveData.musicVolume = musicSlider.value;
            saveManager.SaveGame();
        }


     
    }

    public void VolumeBar()
    {
        VolumeText.text = (int)(sfxSlider.value * 100f) + "%";
    }

    public void MusicBar()
    {
        MusicText.text = (int)(musicSlider.value * 100f) + "%";
    }




}
