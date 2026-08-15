using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IDamage
{
    void TakeDamage(float amount, DamageType effect);
    void DeathOverTime(float duration);
}

public enum DamageType
{
    Normal,
    KnockBack,
    Stun,
};
