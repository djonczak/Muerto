﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyHP : MonoBehaviour, IDamage
{
    [SerializeField] private float maxHP = 1f;
    [SerializeField] private float currentHP;
    public float xp;
    public bool isAlive;

    void OnEnable()
    {
        isAlive = true;
        currentHP = maxHP;
    }

    public void TakeDamage(float amount, DamageType type)
    {
        if(type == DamageType.Normal)
        {
            currentHP -= amount;
        }
        else if(type == DamageType.KnockBack)
        {
            KnockBack();
        }

        if (currentHP <= 0)
        {
            Dead();
        }
    }

    private void KnockBack()
    {

    }

    private void Dead()
    {
        DeathEffect();
        DeathEvent.EnemyDiedShake();
        DeathEvent.EnemyDied(xp);
        GetComponent<IReset>().OnDeathReset();
        gameObject.SetActive(false);
    }

    private void DeathEffect()
    {
        GameObject effect = ObjectPooler.instance.GetPooledObject("DeathEffect");
        if(effect != null)
        {
            effect.transform.position = transform.position;
            effect.SetActive(true);
        }
    }
}
