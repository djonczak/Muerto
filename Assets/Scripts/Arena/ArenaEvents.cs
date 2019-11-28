using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArenaEvents : MonoBehaviour
{
    //Triggering on player death
    public delegate void PlayerDeathEventHandler();
    public static event PlayerDeathEventHandler OnPlayerDeath;

    //Triggering when its wave 15 on boss summon
    public delegate void BossSummoningEventHandler();
    public static event BossSummoningEventHandler OnBossShow;

    public static void PlayerDeath()
    {
        OnPlayerDeath?.Invoke();
    }

    public static void BossArrive()
    {
        OnBossShow?.Invoke();
    }
}
