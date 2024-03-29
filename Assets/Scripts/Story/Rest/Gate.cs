﻿using System.Collections;
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

        private bool _canInteract = true;

        private const string PlayerTag = "Player";
        private const string CemeteryLevel = "03_Cementery";
        private const string SpeedKey = "Speed";

        private void Start()
        {
            text.enabled = false;
        }

        private void OnCollisionEnter2D(Collision2D collision)
        {
            if (_canInteract)
            {
                if (collision.collider.tag == PlayerTag)
                {
                    if (PlayerData.Instance.CanPass)
                    {
                        _canInteract = false;
                        collision.gameObject.GetComponent<Player.PlayerMovement>().CanMove = false;
                        collision.gameObject.GetComponent<Animator>().SetFloat(SpeedKey, 0f);
                        CameraManager.CameraFade.Instance.FadeIn(() => SceneManager.LoadScene(CemeteryLevel), 2f);
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
            if (_canInteract)
            {
                if (collision.collider.tag == PlayerTag)
                {
                    text.enabled = false;
                }
            }
        }
    }
}