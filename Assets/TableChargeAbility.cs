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
    [SerializeField] private bool isCharging = false;
    [SerializeField] private LayerMask enemyLayer = 11;
    private Animator anim;
    private Vector3 mousePosition;

    private void Start()
    {
        anim = GetComponent<Animator>();
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
                hasCharged = true;

            }

            if (hasCharged == true)
            {
                if (Input.GetKeyDown(KeyCode.Mouse0))
                {
                    Time.timeScale = 1f;
                    GetComponent<Rigidbody2D>().velocity = CalculateStartDirection() * chargeSpeed;
                    StartCoroutine("ChargeDuration", abilityDuration);
                    anim.SetBool("Charge", true);
                    anim.SetBool("Run", false);
                    anim.SetBool("Idle", false);
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
            }
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.layer == 8)
        {
            Debug.Log("Cyk");
            GetComponent<Rigidbody2D>().velocity = CalculateStartDirection();
        }
    }

    IEnumerator ChargeDuration(float time)
    {
        yield return new WaitForSeconds(time);
        anim.SetBool("Charge", false);
        isCharging = false;
        ArenaEvents.PlayerCharge();
        GetComponent<Rigidbody2D>().velocity = Vector2.zero;
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
