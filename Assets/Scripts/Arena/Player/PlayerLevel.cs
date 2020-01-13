using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerLevel : MonoBehaviour
{
    public float expPoints = 0;
    public float expPointMaxCap = 100;
    public int playerLevel = 0;
    private bool Ability1unlock;
    private bool Ability2unlock;

    public Text levelText;
    public Animator levelUpAnim;

    private bool unlockFirstAbility;
    private bool unlockSecondAbility;

    private void OnEnable()
    {
        DeathEvent.OnDeathExp += AddExp;
    }

    private void Start()
    {
        levelText.text = "Level " + playerLevel;
    }

    public void AddExp(float amount)
    {
        expPoints += amount;
        CheckLevel();
    }

    void LateUpdate()
    {
        levelUpAnim.transform.rotation = Quaternion.identity;
    }

    void CheckLevel()
    {
        if (expPoints >= expPointMaxCap)
        {

            levelUpAnim.Play(0);
            GetComponent<ISoundEffect>().PlayLevelUpSound();
            CalculateAdditionalExperience();
            levelText.text = "Level " + playerLevel;
            UnlockAbility();
        }
    }

    private void CalculateAdditionalExperience()
    {
        var restExp = expPoints - expPointMaxCap;
        playerLevel++;
        expPointMaxCap += 100;
        expPoints = restExp;
    }

    void UnlockAbility()
    {
        if(unlockFirstAbility == false && playerLevel == 4)
        {
            GetComponent<DivingElbowAbility>().disable = false;
            unlockFirstAbility = true;
            PlayerUI.instance.UnlockAbility1(GetComponent<DivingElbowAbility>().abilityCooldown);
        }

        if(unlockSecondAbility == false && playerLevel == 9)
        {
            GetComponent<TableChargeAbility>().disable = false;
            unlockSecondAbility = true;
            PlayerUI.instance.UnlockAbility2(GetComponent<TableChargeAbility>().abilityCooldown);
        }
    }
    [ContextMenu("AddLevel")]
    public void AddLevel()
    {
        playerLevel++;
        UnlockAbility();
    }

    [ContextMenu("Unlock first ability")]
    void Unlock1Ability()
    {
        GetComponent<DivingElbowAbility>().disable = false;
        unlockFirstAbility = true;
        PlayerUI.instance.UnlockAbility1(GetComponent<DivingElbowAbility>().abilityCooldown);
    }

    [ContextMenu("Unlock second ability")]
    void Unlock2Ability()
    {
        GetComponent<TableChargeAbility>().disable = false;
        unlockSecondAbility = true;
        PlayerUI.instance.UnlockAbility2(GetComponent<TableChargeAbility>().abilityCooldown);
    }

    private void OnDestroy()
    {
        DeathEvent.OnDeathExp -= AddExp;
    }
}
