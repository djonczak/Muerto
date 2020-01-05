using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossSecondAbility : MonoBehaviour
{
    public bool unlock;

    [SerializeField] private float abilityCooldown = 5f;

    private float timer;

    private void Update()
    {
        CheckTimer();
    }

    private void CheckTimer()
    {
        if (unlock == true)
        {
            timer += Time.deltaTime;
            if (timer >= abilityCooldown)
            {
                timer = 0f;
                CastAbility();
            }
        }
    }

    private void CastAbility()
    {
        GameObject waveAttack = ObjectPooler.instance.GetPooledObject("BossPoolAttack");
        if (waveAttack != null)
        {
            waveAttack.SetActive(true);
            waveAttack.GetComponent<BloodPillar>().UsePillar(GetComponent<BossMovement>().target.transform);
        }
    }
}
