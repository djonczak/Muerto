using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Game.Interactable
{
    public class LoadLevel : MonoBehaviour
    {
        public string Lvl;
        public GameObject button;

        private bool _isColliding = false;
        private bool _canInteract = true;
        private const string PlayerTag = "Player";

        private void Update()
        {
            if (_isColliding == true && _canInteract)
            {
                if (Input.GetKey(KeyCode.E))
                {
                    CameraManager.CameraFade.Instance.FadeIn(() => SceneManager.LoadScene(Lvl),2);
                    _canInteract = false;
                }
            }
        }

        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (collision.gameObject.tag == PlayerTag)
            {
                _isColliding = true;
                button.SetActive(true);
            }
        }

        private void OnTriggerExit2D(Collider2D collision)
        {
            if (collision.gameObject.tag == PlayerTag)
            {
                _isColliding = false;
                button.SetActive(false);
            }
        }
    }
}
