using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game.Arena.Player {
    public class PlayerSoundEffects : MonoBehaviour, ISoundEffect
    {
        public AudioClip levelUpSound;
        public AudioClip healSound;
        public AudioClip ability1Sound;
        public AudioClip ability2Sound;
        public AudioClip deathSound;

        private AudioSource _audioSource;

        void Awake()
        {
            _audioSource = GetComponent<AudioSource>();
        }

        public void PlayLevelUpSound()
        {
            _audioSource.PlayOneShot(levelUpSound);
        }

        public void PlayHealSound()
        {
            _audioSource.PlayOneShot(healSound);
        }

        public void PlayAbility1Sound()
        {
            _audioSource.PlayOneShot(ability1Sound);
        }

        public void PlayAbility2Sound()
        {
            _audioSource.PlayOneShot(ability2Sound);
        }

        public void PlayDeathSound()
        {
            _audioSource.PlayOneShot(deathSound);
        }
    }
}
