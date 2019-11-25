using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyMeleeAttack : MonoBehaviour
{
    [Header("Melee Attack Settings")]
    public float attackDamage;
    public float attackSpeed;
    public float attackRadius;
    [SerializeField]
    private GameObject player;

    private Animator anim;
    float timer;

    public void Start()
    {
        anim = GetComponent<Animator>();
        player = GetComponent<EnemyMovement>().target;
    }

    public void Update()
    {
        Attack();
    }

    private void Attack()
    {
        if (GetComponent<EnemyHP>().isAlive == true)
        {
            var distance = Vector3.Distance(transform.position, player.transform.position);
            if (distance <= attackRadius)
            {
                timer += Time.deltaTime;
                if (timer >= attackSpeed)
                {
                    anim.SetTrigger("Attack");
                    GetComponent<EnemyMovement>().canMove = false;
                    player.GetComponent<IDamage>().TakeDamage(attackDamage, DamageType.Normal);
                    timer = 0f;
                }
            }
            else
            {
                timer = 0f;
            }
        }      
    }

    public void EndAttack()
    {
        anim.SetBool("Run", true);
        GetComponent<EnemyMovement>().canMove = true;
    }

    void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, attackRadius);
    }
}
