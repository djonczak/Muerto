using System.Collections;
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

        public GameObject abilityFrame;
        public Text abilityInputText;
        public Text abilityDescriptionText;

        [SerializeField] private Color cooldownColor = new Color(255f, 0f, 0f, 184f);
        [SerializeField] private Color canUseColor = new Color(0f, 255f, 0f, 184f);
        [SerializeField] private Color activeColor = new Color(0f, 255f, 0f, 184f);

        [SerializeField] private float ability1Cooldown = 0f;
        [SerializeField] private float ability2Cooldown = 0f;

        private float ability1End = 0f;
        private float ability2End = 0f;

        private bool unlockFirstAbility = false;
        private bool unlockSecondAbility = false;

        private bool isPaused = false;

        [SerializeField] private bool usedFirst = false;
        [SerializeField] private bool usedSecond = false;

        private const string InputFirst = "Press Q to activate";
        private const string InputSecond = "Press W to activate";

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

        private void Start()
        {
            ArenaEvents.ActivatedFirstAbility += Pressed1Ability;
            ArenaEvents.ActivatedSecondAbility += Pressed2Ability;
        }

        public void UnlockAbility1(float cooldown, Sprite icon, string text)
        {
            unlockFirstAbility = true;
            lockAbility1.enabled = false;
            ability1Image.sprite = icon;
            abilityDescriptionText.text = text;
            abilityInputText.text = InputFirst;
            ability1Image.gameObject.SetActive(true);
            ability1Cooldown = cooldown;
            PauseGameAbility1();
        }

        public void UnlockAbility2(float cooldown, Sprite icon, string text)
        {
            unlockSecondAbility = true;
            lockAbility2.enabled = false;
            ability2Image.sprite = icon;
            abilityDescriptionText.text = text;
            abilityInputText.text = InputSecond;
            ability2Image.gameObject.SetActive(true);
            ability2Cooldown = cooldown;
            PauseGameAbility2();
        }

        private void Pressed1Ability(bool activation)
        {
            if (activation)
            {
                ability1CooldownImage.gameObject.SetActive(true);
                ability1CooldownImage.fillAmount = 1f;
                ability1CooldownImage.color = activeColor;
            }
            else
            {
                ability1CooldownImage.gameObject.SetActive(false);
            }
        }

        private void Pressed2Ability(bool activation)
        {
            if (activation)
            {
                ability2CooldownImage.gameObject.SetActive(true);
                ability2CooldownImage.fillAmount = 1f;
                ability2CooldownImage.color = activeColor;
            }
            else
            {
                ability2CooldownImage.gameObject.SetActive(false);
            }
        }


        public void Used1Ability()
        {
            usedFirst = true;
            ability1End = Time.time + ability1Cooldown;
            ability1CooldownImage.gameObject.SetActive(true);
        }

        public void Used2Ability()
        {
            usedSecond = true;
            ability2End = Time.time + ability2Cooldown;
            ability2CooldownImage.gameObject.SetActive(true);
        }

        private void PauseGameAbility1()
        {
            abilityFrame.SetActive(true);
            isPaused = true;
            ArenaEvents.StopCamera();
            Time.timeScale = 0.00001f;
        }

        private void PauseGameAbility2()
        {
            abilityFrame.SetActive(true);
            isPaused = true;
            ArenaEvents.StopCamera();
            Time.timeScale = 0.00001f;
        }

        private void Update()
        {
            FirstAbilityCooldown();

            SecondAbilityCooldown();

            if (isPaused)
            {
                if (Input.GetKeyDown(KeyCode.Mouse0))
                {
                    isPaused = false;
                    Time.timeScale = 1;
                    abilityFrame.SetActive(false);
                }
            }
        }

        private void SecondAbilityCooldown()
        {
            if (unlockSecondAbility == true)
            {
                if (usedSecond)
                {
                    if (Time.time >= ability2End)
                    {
                        usedSecond = false;
                        ability2CooldownImage.color = canUseColor;
                        ability2CooldownImage.fillAmount = 0f;
                    }
                    else
                    {
                        ability2CooldownImage.color = cooldownColor;
                        ability2CooldownImage.fillAmount = (ability2End - Time.time) / ability2Cooldown;
                    }
                }
            }
        }

        private void FirstAbilityCooldown()
        {
            if (unlockFirstAbility == true)
            {
                if (usedFirst)
                {
                    if (Time.time >= ability1End)
                    {
                        usedFirst = false;
                        ability1CooldownImage.color = canUseColor;
                        ability1CooldownImage.fillAmount = 0f;
                    }
                    else
                    {
                        ability1CooldownImage.color = cooldownColor;
                        ability1CooldownImage.fillAmount = (ability1End - Time.time) / ability1Cooldown;
                    }
                }
            }
        }

        private void OnDisable()
        {
            ArenaEvents.ActivatedFirstAbility -= Pressed1Ability;
            ArenaEvents.ActivatedSecondAbility -= Pressed2Ability;
        }
    }
}
