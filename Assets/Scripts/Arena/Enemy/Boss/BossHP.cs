using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BossHP : MonoBehaviour, IDamage
{
    [SerializeField] private float maxHP = 15f;
    [SerializeField] private float currentHP = 0f;
    private bool isAlive = true;

    public Image healthBar;

    private void Start()
    {
        currentHP = maxHP;
    }

    public void TakeDamage(float amount, DamageType type)
    {
        if (isAlive == true)
        {
            currentHP -= amount;
            GetComponent<SpriteEffect>().DamageEffect();
            CheckSecondPhase();
            if (currentHP <= 0)
            {
                Death();
            }
        }
    }

    private void Death()
    {
        GetComponent<BossMeleeAttack>().enabled = false;
        GetComponent<BossMovement>().enabled = false;
        GetComponent<BossFirstAbility>().enabled = false;
        GetComponent<BossSecondAbility>().enabled = false;
        enabled = false;  
    }

    private void CheckSecondPhase()
    {
        if(currentHP <= 8)
        {
            GetComponent<BossSecondAbility>().unlock = true;
        }
    }
}
