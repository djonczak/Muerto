using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game.Audio 
{
    public class AudioFade : MonoBehaviour
    {
        [SerializeField] private float fadeTime;
        private AudioSource audioSource;

        private float startValue;

        private void Awake()
        {
            audioSource = GetComponent<AudioSource>();
        }

        private void Start()
        {
            startValue = audioSource.volume;
            audioSource.volume = 0;
            StartCoroutine(FadeAudio());
        }

        private IEnumerator FadeAudio()
        {
            var time = 0f;
            var startVolume = audioSource.volume;
            while (time < 2f)
            {
                var value = Mathf.Lerp(startVolume, startValue, time / 2f);
                audioSource.volume = value;
                time += Time.deltaTime;
                yield return null;
            }
        }
    }
}
