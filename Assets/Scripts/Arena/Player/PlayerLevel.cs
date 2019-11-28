using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerLevel : MonoBehaviour
{
    public float expPoints = 0;
    public float expPointCap = 100;
    public int lvl = 0;
    bool Ability1unlock;
    bool Ability2unlock;

    public Text levelText;
    public Animator levelUpAnim;

    private void OnEnable()
    {
        DeathEvent.OnDeathExp += AddExp;
    }

    private void Start()
    {
        levelText.text = "Level " + lvl;
    }

    public void AddExp(float amount)
    {
        expPoints += amount;
        CheckLevel();
    }

    void CheckLevel()
    {
        if (expPoints >= expPointCap)
        {
            levelUpAnim.Play(0);
            var restExp = expPoints - expPointCap;
            lvl++;
            expPointCap += 100;
            expPoints = restExp;
            levelText.text = "Level " + lvl;
        }      
    }

    private void OnDestroy()
    {
        DeathEvent.OnDeathExp -= AddExp;
    }
}
