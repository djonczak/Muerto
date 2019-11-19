using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundEffectStory : MonoBehaviour
{
    private AudioSource soundSource;

    [Header("Story Mode Sounds")]
    public AudioClip walkClip;
    public AudioClip pickItemClip;

    private void Start()
    {
        ItemPickEvent.OnItemPick += PickItemSound;
        soundSource = GetComponent<AudioSource>();
    }

    public void WalkSound()
    {
        soundSource.PlayOneShot(walkClip);
    }

    public void PickItemSound(int i)
    {
        soundSource.PlayOneShot(pickItemClip);
    }

}
