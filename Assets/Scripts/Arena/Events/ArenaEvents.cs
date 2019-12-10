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

    //Triggering on player charge ability
    public delegate void PlayerChargedEventHandler();
    public static event PlayerChargedEventHandler OnPlayerCharge;

    // Turning off camera shake
    public delegate void TurnOffCameraShakeEventHandler();
    public static event TurnOffCameraShakeEventHandler OnCameraStop;

    public static void PlayerDeath()
    {
        OnPlayerDeath?.Invoke();
    }

    public static void BossArrive()
    {
        OnBossShow?.Invoke();
    }

    public static void PlayerCharge()
    {
        OnPlayerCharge?.Invoke();
    }

    public static void StopCamera()
    {
        OnCameraStop?.Invoke();
    }
}
