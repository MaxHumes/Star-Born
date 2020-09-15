using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public AudioSource jumpAudio;
    public AudioSource shotAudio;
    public AudioSource enemyDeathAudio;
    public AudioSource playerDeathAudio;
    public AudioSource winAudio;

    public static AudioManager Instance;
    private void Start()
    {
        Instance = this;
    }

    public void PlayJump()
    {
        jumpAudio.Play();
    }
    public void PlayShot()
    {
        shotAudio.Play();
    }
    public void PlayEnemyDeath()
    {
        enemyDeathAudio.Play();
    }
    public void PlayPlayerDeath()
    {
        playerDeathAudio.Play();
    }
    public void PlayWin()
    {
        winAudio.Play();
    }
}
