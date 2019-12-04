using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DivingElbowAbility : MonoBehaviour
{
    public bool disable = false;
    [SerializeField] private float damage = 1f;
    [SerializeField] private float abilityCooldown = 5f;
    [SerializeField] private float abilityRange = 5f;
    [SerializeField] private float fallSpeed = 10f;
    [SerializeField] private bool canUse = true;
    [SerializeField] private bool hasJumped = false;
    [SerializeField] private bool fallDown = false;
    [SerializeField] private LayerMask enemyLayer = 11;
    private Animator anim;
    private Vector3 mousePosition;

    private void Start()
    {
        anim = GetComponent<Animator>();
    }

    public void Update()
    {
        if (disable == false)
        {
            Input();

            FallDawn();
        }
    }

    private void Input()
    {
        if (UnityEngine.Input.GetKeyDown(KeyCode.Q) && canUse == true)
        {
            PrepareToJump();
        }

        if (hasJumped == true)
        {
            if (UnityEngine.Input.GetKeyDown(KeyCode.Mouse0))
            {
                Jump();
            }
        }
    }

    private void PrepareToJump()
    {
        canUse = false;
        hasJumped = true;
        Time.timeScale = 0.5f;
        GetComponent<PlayerAttack>().enabled = false;
    }

    private void Jump()
    {
        mousePosition = CalculateMousePosition();
        transform.position = new Vector3(mousePosition.x, transform.position.y + 1.7f, transform.position.z);
        Time.timeScale = 1f;
        GetComponent<BoxCollider2D>().isTrigger = true;
        GetComponent<ArenaMovement>().enabled = false;
        GetComponent<TableChargeAbility>().disable = true;
        fallDown = true;
        anim.SetBool("FallAttack", true);
        anim.SetBool("Run", false);
        anim.SetBool("Idle", false);
        hasJumped = false;
    }

    private void FallDawn()
    {
        if (fallDown == true)
        {
            transform.position = Vector3.MoveTowards(transform.position, mousePosition, fallSpeed * Time.deltaTime);
            if (0.01f > DistanceBetween(transform.position, mousePosition))
            {
                PoundAttack();
            }
        }
    }

    void PoundAttack()
    {
        Collider2D[] enemies = Physics2D.OverlapCircleAll(transform.position, abilityRange, enemyLayer);
        foreach(Collider2D enemy in enemies)
        {
            if(enemy.GetComponent<IDamage>() != null)
            {
                enemy.GetComponent<IDamage>().TakeDamage(damage, DamageType.Normal);
            }
        }
        fallDown = false;
        anim.SetBool("FallAttack", false);
        StartCoroutine("Cooldown", abilityCooldown);
    }

    IEnumerator Cooldown(float time)
    {
        GetComponent<TableChargeAbility>().disable = false;
        GetComponent<BoxCollider2D>().isTrigger = false;
        GetComponent<PlayerAttack>().enabled = true;
        GetComponent<ArenaMovement>().enabled = true;
        yield return new WaitForSeconds(time);
        canUse = true;
    }

    private float DistanceBetween(Vector3 player, Vector3 mouse)
    {
        return Vector3.Distance(player, mouse);
    }

    private Vector3 CalculateMousePosition()
    {
        var mouse = Camera.main.ScreenToWorldPoint(UnityEngine.Input.mousePosition);
        mouse.z = 0f;
        return mouse;
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.cyan;
        Gizmos.DrawWireSphere(transform.position, abilityRange);
    }
}
