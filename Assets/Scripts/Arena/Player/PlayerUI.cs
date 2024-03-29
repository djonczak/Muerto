﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Game.UI
{
    public class PlayerUI : MonoBehaviour
    {
        public static PlayerUI instance;
        public Image lockAbility1;
        public Image lockAbility2;

        public Image ability1Image;
        public Image ability2Image;

        public Image ability1CooldownImage;
        public Image ability2CooldownImage;

        public GameObject abilityDescriptionFrame;
        public GameObject ability1Description;
        public GameObject ability2Description;

        [SerializeField] private Color cooldownColor = new Color(255f, 0f, 0f, 184f);
        [SerializeField] private Color canUseColor = new Color(0f, 255f, 0f, 184f);

        [SerializeField] private float ability1Cooldown = 0f;
        [SerializeField] private float ability2Cooldown = 0f;

        private float _ability1End = 0f;
        private float _ability2End = 0f;

        private bool _unlockFirstAbility = false;
        private bool _unlockSecondAbility = false;

        private bool _isPaused = false;

        [SerializeField] private bool usedElbow = false;
        [SerializeField] private bool usedCharge = false;

        private void Awake()
        {
            if (instance == null)
            {
                instance = this;
            }
            else if (instance != this)
            {
                Destroy(gameObject);
            }
        }

        public void UnlockAbility1(float cooldown)
        {
            _unlockFirstAbility = true;
            lockAbility1.enabled = false;
            ability1Image.gameObject.SetActive(true);
            ability1Cooldown = cooldown;
            PauseGameAbility1();
        }

        public void UnlockAbility2(float cooldown)
        {
            _unlockSecondAbility = true;
            lockAbility2.gameObject.SetActive(false);
            ability2Image.gameObject.SetActive(true);
            ability2Cooldown = cooldown;
            PauseGameAbility2();
        }

        public void Used1Ability()
        {
            usedElbow = true;
            _ability1End = Time.time + ability1Cooldown;
            ability1CooldownImage.gameObject.SetActive(true);
        }

        public void Used2Ability()
        {
            usedCharge = true;
            _ability2End = Time.time + ability2Cooldown;
            ability2CooldownImage.gameObject.SetActive(true);
        }

        private void PauseGameAbility1()
        {
            abilityDescriptionFrame.SetActive(true);
            ability1Description.SetActive(true);
            _isPaused = true;
            ArenaEvents.StopCamera();
            Time.timeScale = 0.00001f;
        }

        private void PauseGameAbility2()
        {
            abilityDescriptionFrame.SetActive(true);
            ability2Description.SetActive(true);
            _isPaused = true;
            ArenaEvents.StopCamera();
            Time.timeScale = 0.00001f;
        }

        private void Update()
        {
            FirstAbilityCooldown();

            SecondAbilityCooldown();

            if (_isPaused)
            {
                if (Input.GetKeyDown(KeyCode.Mouse0))
                {
                    _isPaused = false;
                    Time.timeScale = 1;
                    abilityDescriptionFrame.SetActive(false);
                    ability1Description.SetActive(false);
                    ability2Description.SetActive(false);
                }
            }

        }

        private void SecondAbilityCooldown()
        {
            if (_unlockSecondAbility == true)
            {
                if (usedCharge)
                {
                    if (Time.time >= _ability2End)
                    {
                        usedCharge = false;
                        ability2CooldownImage.color = canUseColor;
                        ability2CooldownImage.fillAmount = 0f;
                    }
                    else
                    {
                        ability2CooldownImage.color = cooldownColor;
                        ability2CooldownImage.fillAmount = (_ability2End - Time.time) / ability2Cooldown;
                    }
                }
            }
        }

        private void FirstAbilityCooldown()
        {
            if (_unlockFirstAbility == true)
            {
                if (usedElbow)
                {
                    if (Time.time >= _ability1End)
                    {
                        usedElbow = false;
                        ability1CooldownImage.color = canUseColor;
                        ability1CooldownImage.fillAmount = 0f;
                    }
                    else
                    {
                        ability1CooldownImage.color = cooldownColor;
                        ability1CooldownImage.fillAmount = (_ability1End - Time.time) / ability1Cooldown;
                    }
                }
            }
        }
    }
}
