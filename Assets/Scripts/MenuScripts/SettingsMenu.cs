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

    [SerializeField] private GameObject MainMenu;
    [SerializeField] private GameObject PauseMenuUi;
    [SerializeField] private GameObject PauseMenuBg;
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

        if (buildIndex == 0)
        {
            MainMenu.SetActive(false);
            ModifyMenu.SetActive(true);
        }
        else if (buildIndex == 1 || buildIndex == 2)
        {
            PauseMenuUi.SetActive(false);
            ModifyMenu.SetActive(true);
            PauseMenuBg.SetActive(true);
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
        else if (buildIndex == 1 || buildIndex == 2)
        {
            PauseMenuUi.SetActive(true);
            ModifyMenu.SetActive(false);
            PauseMenuBg.SetActive(false);
            saveManager.saveData.sfxVolume = sfxSlider.value;
            saveManager.saveData.musicVolume = musicSlider.value;
            saveManager.SaveGame();
        }




       
    }

    public void VolumeBar()
    {
        VolumeText.text = (int)(sfxSlider.value * 100f) + "%";
        audioMixer.SetFloat("sfx", Mathf.Log10(sfxSlider.value) * 20);
    }

    public void MusicBar()
    {
        MusicText.text = (int)(musicSlider.value * 100f) + "%";
        audioMixer.SetFloat("music", Mathf.Log10(musicSlider.value) * 20);
    }




}
