using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    public static SoundManager instance;
    public AudioSource audioSource;
    [SerializeField]
    private AudioClip playerSpeedUp, bossSpeedUp, windSound, mirrorBreak;

    public void Awake()
    {
        instance = this;
    }

    public void MirrorBreak()
    {
        audioSource.clip = mirrorBreak;
        audioSource.Play();                                                              
    } 

    public void PlayerSpeedUp()
    {
        audioSource.clip = playerSpeedUp;
        audioSource.Play();                                                              
    } 
}
