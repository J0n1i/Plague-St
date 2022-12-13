using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomSoundPlayer : MonoBehaviour
{
    public static RandomSoundPlayer RandomSndP;

    public AudioClip[] boneSounds;
    public AudioClip[] playerSounds;
    public AudioClip[] weaponSounds;
    public AudioClip[] roomSounds;

    private AudioSource audioSrc;

    private int randomBoneSound;
    private int randomPlayerSound;
    private int randomWeaponSound;
    private int randomRoomSound;

    //private AudioSource source;
    //[Range(0.1f, 0.5f)]
    //public float volumeChangeMultiplier;
    //[Range(0.1f, 0.5f)]
    //public float pitchChangeMultiplier;


    public void Start()
    {
        RandomSndP = this;
        audioSrc = GetComponent<AudioSource>();
        boneSounds = Resources.LoadAll<AudioClip>("BoneSounds");
        playerSounds = Resources.LoadAll<AudioClip>("PlayerSounds");
        weaponSounds = Resources.LoadAll<AudioClip>("WeaponSounds");
        roomSounds = Resources.LoadAll<AudioClip>("RoomSounds");
    }

    public void PlayBoneSound()
    {
        randomBoneSound = Random.Range(0, boneSounds.Length);
        audioSrc.PlayOneShot(boneSounds[randomBoneSound]);
    }

    public void PlayPlayerSound()
    {
        randomPlayerSound = Random.Range(0, playerSounds.Length);
        audioSrc.PlayOneShot(playerSounds[randomPlayerSound]);
    }

    public void PlayWeaponSound()
    {
        randomWeaponSound = Random.Range(0, weaponSounds.Length);
        audioSrc.PlayOneShot(weaponSounds[randomWeaponSound]);
    }

    public void PlayRoomSound()
    {
        randomBoneSound = Random.Range(0, roomSounds.Length);
        audioSrc.PlayOneShot(roomSounds[randomRoomSound]);
    }


    //private void Update()
    //{
    //    if (Input.GetKeyDown(KeyCode.S))
    //    {

    //    }
    //}


    //public void SoundPlayer()
    //{
    //        source.clip = sounds[Random.Range(0, sounds.Length)];
    //        source.volume = Random.Range(1 - volumeChangeMultiplier, 1);
    //        source.pitch = Random.Range(1 - pitchChangeMultiplier, 1+pitchChangeMultiplier);
    //        source.PlayOneShot(source.clip);

    //}

}
