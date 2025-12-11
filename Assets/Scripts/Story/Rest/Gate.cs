using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using Game.Story;

namespace Game.Interactable
{
    public class Gate : MonoBehaviour
    {
        public Text text;
        public AudioSource treeAudio;
        public AudioSource mariachiAudio;
        private bool canInteract = true;

        private const string PlayerTag = "Player";
        private const string CemeteryLevel = "03_Cementery";
        private const string SpeedKey = "Speed";

        private void Start()
        {
            text.enabled = false;
        }

        private void OnCollisionEnter2D(Collision2D collision)
        {
            if (canInteract)
            {
                if (collision.collider.tag == PlayerTag)
                {
                    if (PlayerData.Instance.CanPass)
                    {
                        canInteract = false;
                        collision.gameObject.GetComponent<Player.PlayerMovement>().CanMove = false;
                        collision.gameObject.GetComponent<Animator>().SetFloat(SpeedKey, 0f);
                        CameraManager.CameraFade.Instance.FadeIn(() => SceneManager.LoadScene(CemeteryLevel), 2f);
                        StartCoroutine(FadeAudio(treeAudio));
                        StartCoroutine(FadeAudio(mariachiAudio));
                    }
                    else
                    {
                        text.enabled = true;
                    }
                }
            }
        }

        private void OnCollisionExit2D(Collision2D collision)
        {
            if (canInteract)
            {
                if (collision.collider.tag == PlayerTag)
                {
                    text.enabled = false;
                }
            }
        }

        private IEnumerator FadeAudio(AudioSource audioSource)
        {
            var time = 0f;
            var startVolume = audioSource.volume;
            while (time < 2f)
            {
                var value = Mathf.Lerp(startVolume, 0, time / 1.5f);
                audioSource.volume = value;
                time += Time.deltaTime;
                yield return null;
            }
            audioSource.volume = 0;
        }
    }
}