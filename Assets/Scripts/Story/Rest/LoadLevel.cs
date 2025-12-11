using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Game.UI;

namespace Game.Interactable
{
    public class LoadLevel : MonoBehaviour
    {
        public string Lvl;
        public AppearButton button;
        public AudioSource ambientBackground;
        private bool isColliding = false;
        private bool canInteract = true;
        private AudioSource audioSource;
        private GameObject player;

        private const string PlayerTag = "Player";
        private const string SpeedKey = "Speed";

        private void Awake()
        {
            audioSource = GetComponent<AudioSource>();
        }

        private void Update()
        {
            if (isColliding == true && canInteract)
            {
                if (Input.GetKey(KeyCode.E))
                {
                    audioSource.Play();
                    player.GetComponent<Player.PlayerMovement>().CanMove = false;
                    player.GetComponent<Animator>().SetFloat(SpeedKey, 0f);
                    CameraManager.CameraFade.Instance.FadeIn(() => SceneManager.LoadScene(Lvl),2);
                    canInteract = false;
                    button.HideButton();
                    if (ambientBackground != null)
                    {
                        StartCoroutine(MuteAmbient());
                    }
                }
            }
        }

        private IEnumerator MuteAmbient()
        {
            var timer = 0f;
            var startValue = ambientBackground.volume;
            while(timer < 1.5f)
            {
                var value = Mathf.Lerp(startValue, 0, timer / 1.5f);
                ambientBackground.volume = value;
                timer += Time.deltaTime;
                yield return null;
            }
            ambientBackground.volume = 0f;
        }

        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (canInteract)
            {
                if (collision.gameObject.tag == PlayerTag)
                {
                    isColliding = true;
                    player = collision.gameObject;
                    button.ShowButton();
                }
            }
        }

        private void OnTriggerExit2D(Collider2D collision)
        {
            if (canInteract)
            {
                if (collision.gameObject.tag == PlayerTag)
                {
                    isColliding = false;
                    player = null;
                    button.HideButton();
                }
            }
        }
    }
}
