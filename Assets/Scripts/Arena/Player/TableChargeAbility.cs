using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TableChargeAbility : MonoBehaviour
{
    public bool disable = false;
    public float abilityCooldown = 15f;
    [SerializeField] private float damage = 1f;
    [SerializeField] private float abilityRange = 1f;
    [SerializeField] private float abilityDuration = 5f;
    [SerializeField] private float chargeSpeed = 10f;
    [SerializeField] private bool canUse = true;
    [SerializeField] private bool hasCharged = false;
    public bool isCharging = false;
    [SerializeField] private LayerMask enemyLayer = 11;
    private Animator anim;
    private Vector3 mousePosition;
    private Rigidbody2D rb;

    private void Awake()
    {
        anim = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();
    }

    private void Update()
    {
        if (disable == false)
        {
            Input();

            Charge();
        }
    }

    private void Input()
    {
        if (UnityEngine.Input.GetKeyDown(KeyCode.W) && canUse == true)
        {
            PrepareForCharge();
        }

        if (hasCharged == true)
        {
            if (UnityEngine.Input.GetKeyDown(KeyCode.Mouse0))
            {
                StartCharge();
            }
        }
    }

    private void StartCharge()
    {
        Time.timeScale = 1f;
        rb.velocity = CalculateStartDirection() * chargeSpeed;
        StartCoroutine("ChargeDuration", abilityDuration);
        anim.SetBool("Charge", true);
        isCharging = true;
        ArenaEvents.PlayerCharge();
        hasCharged = false;
    }

    private void PrepareForCharge()
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
        GetComponent<PlayerHP>().canBeHurt = false;
    }

    private void Charge()
    {
        if (isCharging == true)
        {
            Collider2D[] enemies = Physics2D.OverlapCircleAll(transform.position, abilityRange, enemyLayer);
            if (enemies != null)
            {
                foreach (Collider2D enemy in enemies)
                {
                    enemy.GetComponent<IDamage>().TakeDamage(damage, DamageType.Normal);
                    GetComponent<ISoundEffect>().PlayAbility2Sound();
                }
            }

            ChargeRotation();
        }
    }

    private void ChargeRotation()
    {
        if (rb.velocity.x > 0.1f)
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
        GetComponent<PlayerHP>().canBeHurt = true;
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
        PlayerUI.instance.Used2Ability();
        yield return new WaitForSeconds(time);
        Debug.Log("You can use your second ability.");
        canUse = true;
    }

    private Vector3 CalculateStartDirection()
    {
        var mouse = Camera.main.ScreenToWorldPoint(UnityEngine.Input.mousePosition);
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
