using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyDashAttack : MonoBehaviour
{
    [Header("Dash Attack Settings")]
    public float attackDamage;
    public float dashCooldown;
    public float dashRadius;
    [SerializeField]
    private float dashRange = 2.5f;
    [SerializeField]
    private float dashSpeed = 0.5f;
    public GameObject player;

    private Animator anim;
    float timer;
    private bool canDash = true;
    public bool isDashing = false;
    private Vector3 target;

    public void Start()
    {
        anim = GetComponent<Animator>();
        player = GetComponent<EnemyMovement>().target;
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
                    var distance = Vector3.Distance(transform.position, player.transform.position);
                    if (distance <= dashRadius)
                    {
                        anim.SetTrigger("Attack");
                        isDashing = true;
                        target = new Vector3(player.transform.position.x, player.transform.position.y, 0);
                        Debug.Log("Dash");
                        timer = 0f;
                        GetComponent<EnemyMovement>().canMove = false;
                    }
                }
            }
        }
    }

    private void Dash()
    {
        if (isDashing == true)
        {
            float step = (dashSpeed) * Time.fixedDeltaTime;
            transform.position = Vector3.MoveTowards(transform.position, target, step / dashRange);
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
}
