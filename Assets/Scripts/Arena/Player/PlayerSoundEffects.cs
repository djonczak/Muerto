using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSoundEffects : MonoBehaviour, ISoundEffect
{
    public AudioClip levelUpSound;
    public AudioClip healSound;
    public AudioClip ability1Sound;
    public AudioClip ability2Sound;
    public AudioClip deathSound;

    private AudioSource sound;

    void Awake()
    {
        sound = GetComponent<AudioSource>();
    }

    public void PlayLevelUpSound()
    {
        sound.PlayOneShot(levelUpSound);
    }

    public void PlayHealSound()
    {
        sound.PlayOneShot(healSound);
    }
    
    public void PlayAbility1Sound()
    {
        sound.PlayOneShot(ability1Sound);
    }

    public void PlayAbility2Sound()
    {
        sound.PlayOneShot(ability2Sound);
    }

    public void PlayDeathSound()
    {
        sound.PlayOneShot(deathSound);
    }
}
