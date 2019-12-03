using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerLevel : MonoBehaviour
{
    public float expPoints = 0;
    public float expPointMaxCap = 100;
    public int playerLevel = 0;
    bool Ability1unlock;
    bool Ability2unlock;

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
            var restExp = expPoints - expPointMaxCap;
            playerLevel++;
            expPointMaxCap += 100;
            expPoints = restExp;
            levelText.text = "Level " + playerLevel;
            UnlockAbility();
        }      
    }

    void UnlockAbility()
    {
        if(unlockFirstAbility == false && playerLevel == 4)
        {
            GetComponent<DivingElbowAbility>().disable = false;
            unlockFirstAbility = true;
        }

        if(unlockSecondAbility == false && playerLevel == 9)
        {
            GetComponent<TableChargeAbility>().disable = false;
            unlockSecondAbility = true;
        }
    }

    private void OnDestroy()
    {
        DeathEvent.OnDeathExp -= AddExp;
    }
}
