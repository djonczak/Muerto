using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TableChargeAbility : MonoBehaviour
{
    [SerializeField] private float damage = 1f;
    [SerializeField] private float abilityRange = 1f;
    [SerializeField] private float abilityCooldown = 5f;
    [SerializeField] private float abilityDuration = 5f;
    [SerializeField] private float chargeSpeed = 10f;
    [SerializeField] private bool canUse = true;
    [SerializeField] private bool hasCharged = false;
    [SerializeField] private bool isCharging = false;
    [SerializeField] private LayerMask enemyLayer = 11;
    //private Animator anim;
    private Vector3 mousePosition;

    private void Start()
    {
        //anim = GetComponent<Animator>();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.W) && canUse == true)
        {
            canUse = false;
            Time.timeScale = 0.5f;
            GetComponent<ArenaMovement>().enabled = false;
            GetComponent<PlayerAttack>().enabled = false;
            GetComponent<DivingElbowAbility>().enabled = false;
            hasCharged = true;

        }

        if(hasCharged == true)
        {
            if (Input.GetKeyDown(KeyCode.Mouse0))
            {
                Time.timeScale = 1f;
                GetComponent<Rigidbody2D>().velocity = CalculateStartDirection() * chargeSpeed;
                StartCoroutine("ChargeDuration", abilityDuration);
                isCharging = true;
                hasCharged = false;
                ArenaEvents.PlayerCharge();
            }
        }

        if(isCharging == true)
        {
            Collider2D[] enemies = Physics2D.OverlapCircleAll(transform.position, abilityRange, enemyLayer);
            foreach (Collider2D enemy in enemies)
            {
                Debug.Log(enemy.gameObject.name);
                enemy.GetComponent<IDamage>().TakeDamage(damage, DamageType.Normal);
            }
        }
    }

    //private void OnCollisionEnter2D(Collision2D collision)
    //{
    //    if(collision.gameObject.layer == 8)
    //    {
    //        Debug.Log("Cyk");
    //        var direction = collision.contacts[0].normal;
    //        direction = Random.Range(-5,5) * direction;
    //        GetComponent<Rigidbody2D>().velocity = direction;
    //        // GetComponent<Rigidbody2D>().velocity = new Vector2 (collision.relativeVelocity.x,collision.relativeVelocity.y);
    //    }
    //}

    IEnumerator ChargeDuration(float time)
    {
        yield return new WaitForSeconds(time);
        isCharging = false;
        ArenaEvents.PlayerCharge();
        GetComponent<Rigidbody2D>().velocity = Vector2.zero;
        GetComponent<ArenaMovement>().enabled = true;
        GetComponent<PlayerAttack>().enabled = true;
        GetComponent<DivingElbowAbility>().enabled = true;
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
