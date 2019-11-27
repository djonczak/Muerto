using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyMeleeCollider : MonoBehaviour
{
    [SerializeField]
    private float damage;

    private void Start()
    {
        damage = GetComponentInParent<EnemyMeleeAttack>().attackDamage;
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.tag == "Player" && GetComponentInParent<EnemyMeleeAttack>().isAttacking == true)
        {
            collision.GetComponent<IDamage>().TakeDamage(damage, DamageType.Normal);
            GetComponentInParent<EnemyMeleeAttack>().isAttacking = false;
        }
    }
}
