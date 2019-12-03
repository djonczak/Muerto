using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyDashAttack : MonoBehaviour, IReset
{
    [Header("Dash Attack Settings")]
    public float attackDamage = 1;
    [SerializeField] private float dashCooldown = 2.5f;
    [SerializeField] private float dashRadius = 2.7f;
    [SerializeField] private float dashRange = 2.5f;
    [SerializeField] private float dashSpeed = 0.5f;
    [SerializeField] private GameObject target;

    private Animator anim;
    private float timer;
    private bool canDash = true;
    public bool isDashing = false;
    private Vector3 jumpPosition;

    public void Start()
    {
        anim = GetComponent<Animator>();
        target = GetComponent<EnemyMovement>().target;
    }

    public void FixedUpdate()
    {
        DashCooldown();
        Dash();
    }

    private void DashCooldown()
    {
        if (GetComponent<EnemyHP>().isAlive == true)
        {
            if (isDashing == false)
            {
                timer += Time.deltaTime;
                if (timer >= dashCooldown && canDash == true)
                {
                    var distance = Vector3.Distance(transform.position, target.transform.position);
                    if (distance <= dashRadius)
                    {
                        StartDash();
                    }
                }
            }
        }
    }

    private void StartDash()
    {
        anim.SetTrigger("Attack");
        isDashing = true;
        jumpPosition = new Vector3(target.transform.position.x, target.transform.position.y, 0);
        Debug.Log("Dash");
        timer = 0f;
        GetComponent<EnemyMovement>().canMove = false;
        canDash = false;
    }

    private void Dash()
    {
        if (isDashing == true)
        {
            float step = (dashSpeed) * Time.fixedDeltaTime;
            transform.position = Vector3.MoveTowards(transform.position, jumpPosition, step / dashRange);
            transform.position.Normalize();
        }
    }

    public void DashEnd()
    {
        Debug.Log("Dash End");
        isDashing = false;
        canDash = true;
        anim.SetBool("Run", true);
        GetComponent<EnemyMovement>().canMove = true;
    }

    void OnDrawGizmos()
    {
        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(transform.position, dashRadius);
    }

    public void OnDeathReset()
    {
        timer = 0f;
        anim.SetBool("Run", true);
        GetComponent<EnemyMovement>().canMove = true;
        isDashing = false;
        canDash = true;
    }
}
