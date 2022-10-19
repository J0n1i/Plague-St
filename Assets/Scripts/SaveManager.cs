using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

[System.Serializable]
public class SaveData
{
    public float musicVolume, sfxVolume;

}
class SaveManager : MonoBehaviour
{


    public static SaveManager instance;

    public SaveData saveData = new SaveData();


    BinaryFormatter bf = new BinaryFormatter();
    private void Start()
    {
        //SaveGame();
        LoadGame();
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
        }
        else
        {
            Debug.Log("No Game Saved");
        }
    }
}
