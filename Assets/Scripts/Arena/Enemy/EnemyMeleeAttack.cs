using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyMeleeAttack : MonoBehaviour, IReset
{
    [Header("Melee Attack Settings")]
    [SerializeField] private float attackDamage = 1f;
    [SerializeField] private float attackSpeed = 0.5f;
    [SerializeField] private float attackRadius = 0.5f;
    [SerializeField] private GameObject target;
    [SerializeField] private LayerMask playerLayer = 10;

    private Animator anim;
    private float timer;
    private bool isAttacking;

    public void Start()
    {
        anim = GetComponent<Animator>();
        target = GetComponent<EnemyMovement>().target;
    }

    public void Update()
    {
        CheckAttack();
    }

    private void CheckAttack()
    {
        if (GetComponent<EnemyHP>().isAlive == true && isAttacking == false)
        {
            var distance = Vector3.Distance(transform.position, target.transform.position);
            if (distance <= attackRadius)
            {
                timer += Time.deltaTime;
                if (timer >= attackSpeed)
                {
                    StartAttack();
                }
            }
            else
            {
                timer = 0f;
            }
        }      
    }

    private void StartAttack()
    {
        anim.SetTrigger("Attack");
        GetComponent<EnemyMovement>().canMove = false;
        isAttacking = true;
        timer = 0f;
    }

    public void EndAttack()
    {
        anim.SetBool("Run", true);
        isAttacking = false;
        GetComponent<EnemyMovement>().canMove = true;
    }

    public void CastAttack()
    {
        Collider2D player = Physics2D.OverlapCircle(transform.position, attackRadius, playerLayer);
        if (player != null)
        {
            if (player.GetComponent<IDamage>() != null)
            {
                player.GetComponent<IDamage>().TakeDamage(attackDamage, DamageType.Normal);
            }
        }
    }

    void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, attackRadius);
    }

    public void OnDeathReset()
    {
        timer = 0f;
        GetComponent<EnemyMovement>().canMove = true;
        anim.SetBool("Run", true);
        isAttacking = false;
    }
}
