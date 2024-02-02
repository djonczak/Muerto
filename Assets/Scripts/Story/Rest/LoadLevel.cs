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

        private bool _isColliding = false;
        private bool _canInteract = true;
        private AudioSource _audioSource;
        private GameObject _player;

        private const string PlayerTag = "Player";
        private const string SpeedKey = "Speed";

        private void Awake()
        {
            _audioSource = GetComponent<AudioSource>();
        }

        private void Update()
        {
            if (_isColliding == true && _canInteract)
            {
                if (Input.GetKey(KeyCode.E))
                {
                    _audioSource.Play();
                    _player.GetComponent<Player.PlayerMovement>().CanMove = false;
                    _player.GetComponent<Animator>().SetFloat(SpeedKey, 0f);
                    CameraManager.CameraFade.Instance.FadeIn(() => SceneManager.LoadScene(Lvl),2);
                    _canInteract = false;
                }
            }
        }

        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (_canInteract)
            {
                if (collision.gameObject.tag == PlayerTag)
                {
                    _isColliding = true;
                    _player = collision.gameObject;
                    button.ShowButton();
                }
            }
        }

        private void OnTriggerExit2D(Collider2D collision)
        {
            if (_canInteract)
            {
                if (collision.gameObject.tag == PlayerTag)
                {
                    _isColliding = false;
                    _player = null;
                    button.HideButton();
                }
            }
        }
    }
}
