using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossMeleeAttack : MonoBehaviour
{
    [Header("Melee Attack Settings")]
    [SerializeField] private float attackDamage = 1f;
    [SerializeField] private float attackSpeed = 0.5f;
    [SerializeField] private float attackRange = 0.5f;
    [SerializeField] private GameObject target;
    [SerializeField] private LayerMask playerLayer = 10;

    private Animator anim;
    private Animator attackWaveEffect;
    private AudioSource sound;
    private float timer;
    private bool isAttacking;

    private void Awake()
    {
        anim = GetComponent<Animator>();
        sound = GetComponent<AudioSource>();
        attackWaveEffect = GetComponentInChildren<Animator>();
    }

    private void Start()
    {
        target = GetComponent<BossMovement>().target;
    }

    private void Update()
    {
        CheckAttack();
    }

    private void CheckAttack()
    {
        if (target.GetComponent<PlayerHP>().isAlive == true && isAttacking == false)
        {
            var distance = Vector3.Distance(transform.position, target.transform.position);
            if (distance <= attackRange)
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
       // anim.SetTrigger("Attack");
        GetComponent<BossMovement>().canMove = false;
        isAttacking = true;
        timer = 0f;
    }

    private void ShowEffect()
    {
        attackWaveEffect.gameObject.SetActive(true);
    }

    public void EndAttack()
    {
       // anim.SetBool("Run", true);
        isAttacking = false;
        GetComponent<BossMovement>().canMove = true;
    }

    public void CastAttack()
    {
        Collider2D player = Physics2D.OverlapCircle(transform.position, attackRange, playerLayer);
        if (player != null)
        {
            if (player.GetComponent<IDamage>() != null)
            {
                sound.PlayOneShot(sound.clip);
                player.GetComponent<IDamage>().TakeDamage(attackDamage, DamageType.Normal);
            }
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, attackRange);
    }
}
