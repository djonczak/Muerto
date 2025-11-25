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
        public AudioClip specialSound;

        private AudioSource audioSource;
        private bool canPlayAbility2 = true;
        private bool isPlayingSpecial;

        void Awake()
        {
            audioSource = GetComponent<AudioSource>();
        }

        private void Start()
        {
            OnAnimationSwitch.EndAnimation += StopPlayingSpecial;
        }

        public void PlayLevelUpSound()
        {
            audioSource.PlayOneShot(levelUpSound);
        }

        public void PlayHealSound()
        {
            audioSource.PlayOneShot(healSound);
        }

        public void PlayAbility1Sound()
        {
            audioSource.PlayOneShot(ability1Sound);
        }

        public void PlayAbility2Sound()
        {
            if (canPlayAbility2)
            {
                audioSource.PlayOneShot(ability2Sound);
                canPlayAbility2 = false;
                StartCoroutine(Reset2Cooldown());
            }
        }

        public void PlayDeathSound()
        {
            audioSource.PlayOneShot(deathSound);
        }

        private IEnumerator Reset2Cooldown()
        {
            yield return new WaitForSeconds(1.5f);
            canPlayAbility2 = true;
        }

        public void PlaySpecialSound()
        {
            if (isPlayingSpecial == false)
            {
                isPlayingSpecial = true;
                audioSource.clip = specialSound;
                audioSource.Play();
            }
        }

        public void StopPlayingSpecial()
        {
            isPlayingSpecial = false;
            audioSource.Stop();
        }

        private void OnDestroy()
        {
            OnAnimationSwitch.EndAnimation -= StopPlayingSpecial;
        }
    }
}
