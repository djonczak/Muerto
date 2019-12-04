using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerUI : MonoBehaviour
{
    public static PlayerUI instance;
    public Image lockAbility1;
    public Image lockAbility2;

    public Image ability1Image;
    public Image ability2Image;

    public Image ability1CooldownImage;
    public Image ability2CooldownImage;

    [SerializeField] private Color cooldownColor = new Color(255f, 0f, 0f, 184f);
    [SerializeField] private Color canUseColor = new Color(0f, 255f, 0f, 184f);

    [SerializeField] private float ability1Cooldown = 0f;
    [SerializeField] private float ability2Cooldown = 0f;

    private float ability1End = 0f;
    private float ability2End = 0f;

    private bool unlockFirstAbility = false;
    private bool unlockSecondAbility = false;

    [SerializeField] private bool usedElbow = false;
    [SerializeField] private bool usedCharge = false;

    private void Start()
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
        unlockFirstAbility = true;
        lockAbility1.enabled = false;
        ability1Image.gameObject.SetActive(true);
        ability1Cooldown = cooldown;
    }

    public void UnlockAbility2(float cooldown)
    {
        unlockSecondAbility = true;
        lockAbility2.gameObject.SetActive(false);
        ability2Image.gameObject.SetActive(true);
        ability2Cooldown = cooldown;
    }

    public void Used1Ability()
    {
        usedElbow = true;
        ability1End = Time.time + ability1Cooldown;
        ability1CooldownImage.gameObject.SetActive(true);
    }

    public void Used2Ability()
    {
        usedCharge = true;
        ability2End = Time.time + ability2Cooldown;
        ability2CooldownImage.gameObject.SetActive(true);
    }

    private void Update()
    {
        if (unlockFirstAbility == true)
        {
            if (usedElbow)
            {
                if (Time.time >= ability1End)
                {
                    usedElbow = false;
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

        if (unlockSecondAbility == true)
        {
            if (usedCharge)
            {
                if (Time.time >= ability2End)
                {
                    usedCharge = false;
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
}
