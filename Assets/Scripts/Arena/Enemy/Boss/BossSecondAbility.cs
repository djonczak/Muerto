using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossSecondAbility : MonoBehaviour
{
    public bool unlock;

    [SerializeField] private float abilityCooldown = 5f;

    private Transform target;
    private float timer;

    private void Start()
    {
        target = GetComponent<BossMovement>().target.transform;
    }

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
            waveAttack.GetComponent<BloodPillar>().target = target;
            waveAttack.SetActive(true);
        }
    }
}
