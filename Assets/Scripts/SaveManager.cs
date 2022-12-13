using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine.Audio;
using UnityEngine.UI;

[System.Serializable]
public class SaveData
{
    public float musicVolume, sfxVolume;

}
class SaveManager : MonoBehaviour
{

    public SaveData saveData = new SaveData();
    [SerializeField] private AudioMixer audioMixer; 
    [SerializeField] private Slider musicSlider, sfxSlider;

    BinaryFormatter bf = new BinaryFormatter();
    private void Start()
    {
        //SaveGame();
        LoadGame();
    }

    void Awake(){
        if(GameObject.FindGameObjectsWithTag("SaveManager").Length > 1){
            Destroy(this.gameObject);
        }


        DontDestroyOnLoad(this.gameObject);
    }

    public void SaveGame()
    {
        var path = Application.persistentDataPath + "/save.dat";
        FileStream file = File.Create(path);
        bf.Serialize(file, saveData);
        file.Close();

        Debug.Log("Game Saved" + path);

        Debug.Log("Music Volume: " + saveData.musicVolume);
        Debug.Log("SFX Volume: " + saveData.sfxVolume);
    }

    public void LoadGame()
    {
        var path = Application.persistentDataPath + "/save.dat";
        if (File.Exists(path))
        {
            FileStream file = File.Open(path, FileMode.Open);
            saveData = (SaveData)bf.Deserialize(file);
            file.Close();

            Debug.Log("Game Loaded" + path);
            Debug.Log("Music Volume: " + saveData.musicVolume);
            Debug.Log("SFX Volume: " + saveData.sfxVolume);

            audioMixer.SetFloat("music", saveData.musicVolume);
            audioMixer.SetFloat("sfx", saveData.sfxVolume);
            musicSlider.value = saveData.musicVolume;
            sfxSlider.value = saveData.sfxVolume;
        }
        else
        {
            Debug.Log("No Game Saved");
        }
    }
}
