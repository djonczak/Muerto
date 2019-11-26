using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyRangeAttack : MonoBehaviour, IReset
{
    [Header("Range Attack Options")]

    public float attackDamage;
    public float attackSpeed;
    public float attackRadius;
    public Transform barrel;
    [SerializeField]
    private GameObject player;

    private Animator anim;
    float timer;
    private bool canAttack = true;

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
            if (canAttack == true)
            {
                var distance = Vector3.Distance(transform.position, player.transform.position);
                if (distance <= attackRadius)
                {
                    timer += Time.deltaTime;
                    GetComponent<EnemyMovement>().canMove = false;
                    anim.SetBool("Idle", true);
                    anim.SetBool("Run", false);

                    if (timer >= attackSpeed)
                    {
                        anim.SetTrigger("Attack");
                        canAttack = false;
                        timer = 0f;
                    }
                }
                else
                {
                    timer = 0f;
                    anim.SetBool("Idle", false);
                    anim.SetBool("Run", true);
                    GetComponent<EnemyMovement>().canMove = true;
                }
            }
        }
    }

    public void ShootProjectile()
    {
        GameObject projectile = ObjectPooler.instance.GetPooledObject("Bullet");
        if(projectile != null)
        {
            projectile.transform.position = barrel.transform.position;
            projectile.transform.rotation = barrel.transform.rotation;
            var target = player.transform.position;
            float rad = Mathf.Atan2(target.y - barrel.transform.position.y, target.x - barrel.transform.position.x) * Mathf.Rad2Deg;
            projectile.transform.rotation = Quaternion.Euler(new Vector3(0, 0, rad));
      
            projectile.SetActive(true);
            projectile.GetComponent<EnemyBullet>().damage = attackDamage;
        }
    }

    public void EndAttack()
    {
        anim.SetBool("Run", true);
        anim.SetBool("Idle", false);
        canAttack = true;
        GetComponent<EnemyMovement>().canMove = true;
    }

    void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, attackRadius);
    }

    public void OnDeathReset()
    {
        timer = 0f;
        anim.SetBool("Run", true);
        anim.SetBool("Idle", false);
        canAttack = true;
        GetComponent<EnemyMovement>().canMove = true;
    }
}
