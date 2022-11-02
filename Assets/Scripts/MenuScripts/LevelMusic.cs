using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelMusic : MonoBehaviour
{
    public AudioSource levelMusic;
    public AudioSource deathMusic;

    public bool levelSong;
    public bool deathSong;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SceneMusic()
    {
        levelSong = true;
        deathSong = false;
        levelMusic.Play();
    }

    public void DeathMusic()
    {
        if (levelMusic.isPlaying)
            levelSong = false;
        {
            levelMusic.Stop();
        }
        if(!deathMusic.isPlaying && deathSong == false)
        {
            deathMusic.Play(); 
            deathSong = true;
        }
    }
}
