using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TableChargeAbility : MonoBehaviour
{
    public bool disable = false;
    [SerializeField] private float damage = 1f;
    [SerializeField] private float abilityRange = 1f;
    [SerializeField] private float abilityCooldown = 5f;
    [SerializeField] private float abilityDuration = 5f;
    [SerializeField] private float chargeSpeed = 10f;
    [SerializeField] private bool canUse = true;
    [SerializeField] private bool hasCharged = false;
    public bool isCharging = false;
    [SerializeField] private LayerMask enemyLayer = 11;
    private Animator anim;
    private Vector3 mousePosition;
    private Rigidbody2D rb;

    private void Start()
    {
        anim = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();
    }

    private void Update()
    {
        if (disable == false)
        {
            if (Input.GetKeyDown(KeyCode.W) && canUse == true)
            {
                canUse = false;
                Time.timeScale = 0.5f;
                GetComponent<ArenaMovement>().enabled = false;
                GetComponent<PlayerAttack>().enabled = false;
                GetComponent<DivingElbowAbility>().disable = true;
                anim.SetTrigger("ChargeIdle");
                anim.SetBool("Run", false);
                anim.SetBool("Idle", false);
                hasCharged = true;
            }

            if (hasCharged == true)
            {
                if (Input.GetKeyDown(KeyCode.Mouse0))
                {
                    Time.timeScale = 1f;
                    rb.velocity = CalculateStartDirection() * chargeSpeed;
                    StartCoroutine("ChargeDuration", abilityDuration);
                    anim.SetBool("Charge", true);
                    isCharging = true;
                    ArenaEvents.PlayerCharge();
                    hasCharged = false;
                }
            }

            if (isCharging == true)
            {
                Collider2D[] enemies = Physics2D.OverlapCircleAll(transform.position, abilityRange, enemyLayer);
                foreach (Collider2D enemy in enemies)
                {
                    Debug.Log(enemy.gameObject.name);
                    enemy.GetComponent<IDamage>().TakeDamage(damage, DamageType.Normal);
                }

                if(rb.velocity.x > 0.1f)
                {
                    transform.rotation = Quaternion.Euler(0, 0, 0);
                    transform.localScale = new Vector3(transform.localScale.x, transform.localScale.y, 1);
                }
                else
                {
                    transform.rotation = Quaternion.Euler(0, 180, 0);
                    transform.localScale = new Vector3(transform.localScale.x, transform.localScale.y, -1);
                }
            }
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.layer == 8)
        {
            rb.velocity = CalculateStartDirection();
        }
    }

    IEnumerator ChargeDuration(float time)
    {
        yield return new WaitForSeconds(time);
        anim.SetBool("Charge", false);
        isCharging = false;
        ArenaEvents.PlayerCharge();
        rb.velocity = Vector2.zero;
        GetComponent<ArenaMovement>().enabled = true;
        GetComponent<PlayerAttack>().enabled = true;
        GetComponent<DivingElbowAbility>().disable = false;
        StartCoroutine("AbilityCooldown", abilityCooldown);
    }

    IEnumerator AbilityCooldown(float time)
    {
        yield return new WaitForSeconds(time);
        canUse = true;
    }

    private Vector3 CalculateStartDirection()
    {
        var mouse = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        mouse.z = 0f;
        var direction = transform.position - mouse;
        return -direction;
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(transform.position, abilityRange);
    }
}
