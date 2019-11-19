using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeathEvent : MonoBehaviour
{
    public delegate void EnemyEventHandler(float i);
    public delegate void EnemyEventHandlerShake();
    public static event EnemyEventHandlerShake OnEnemyDeath;
    public static event EnemyEventHandler OnDeathExp;

    public static void EnemyDied(float i)
    {
        OnDeathExp?.Invoke(i);
    }

    public static void EnemyDiedShake()
    {
        OnEnemyDeath?.Invoke();
    }
}
