using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArenaEvents : MonoBehaviour
{

    public delegate void PlayerHurtEventHandler();
    public static event PlayerHurtEventHandler OnPlayerHurt;

    //Triggering on player death
    public delegate void PlayerDeathEventHandler();
    public static event PlayerDeathEventHandler OnPlayerDeath;

    //On boss defeat
    public delegate void PlayerVictoryEventHandler();
    public static event PlayerVictoryEventHandler OnPlayerVictory;

    //Triggering when its wave 15 on boss summon
    public delegate void BossSummoningEventHandler();
    public static event BossSummoningEventHandler OnBossShow;

    //Triggering on player charge ability
    public delegate void PlayerChargedEventHandler();
    public static event PlayerChargedEventHandler OnPlayerCharge;

    // Turning off camera shake
    public delegate void TurnOffCameraShakeEventHandler();
    public static event TurnOffCameraShakeEventHandler OnCameraStop;

    // Spawning taco 
    public delegate void SpawningTacoEventHandler();
    public static event SpawningTacoEventHandler OnSpawnTaco;

    public delegate void PressedFirstAbility(bool activate);
    public static event PressedFirstAbility ActivatedFirstAbility;

    public delegate void PressedSecondAbility(bool activate);
    public static event PressedSecondAbility ActivatedSecondAbility;

    public static void PlayerDeath()
    {
        OnPlayerDeath?.Invoke();
    }

    public static void PlayerVictory()
    {
        OnPlayerVictory?.Invoke();
    }

    public static void PlayerGotHurt()
    {
        OnPlayerHurt?.Invoke();
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

    public static void SpawnTaco()
    {
        OnSpawnTaco?.Invoke();
    }

    public static void FirstAbility(bool value)
    {
        ActivatedFirstAbility?.Invoke(value);
    }

    public static void SecondAbility(bool value)
    {
        ActivatedSecondAbility?.Invoke(value);
    }
}

