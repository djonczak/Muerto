using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyHP : MonoBehaviour, IDamage
{
    [SerializeField]
    private float currentHP = 1f;
    public float xp;
    public bool isAlive = true;

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

    void Dead()
    {
        DeathEvent.EnemyDiedShake();
        DeathEvent.EnemyDied(xp);
        gameObject.SetActive(false);
    }
}
