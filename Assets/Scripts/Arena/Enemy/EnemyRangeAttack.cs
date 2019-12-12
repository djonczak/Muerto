using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyRangeAttack : MonoBehaviour, IReset
{
    [Header("Range Attack Options")]
    [SerializeField] private float attackDamage = 1;
    [SerializeField] private float attackSpeed = 0.8f;
    [SerializeField] private float attackRadius = 1.76f;
    public Transform barrel;
    [SerializeField] private GameObject target;

    private Animator anim;
    private float timer;
    private bool canAttack = true;
    private AudioSource sound;

    private void Awake()
    {
        anim = GetComponent<Animator>();
        sound = GetComponent<AudioSource>();
    }

    private void Start()
    {
        target = GetComponent<EnemyMovement>().target;
    }

    private void Update()
    {
        CheckAttack();
    }

    private void CheckAttack()
    {
        if (GetComponent<EnemyHP>().isAlive == true)
        {
            if (canAttack == true)
            {
                var distance = Vector3.Distance(transform.position, target.transform.position);
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
            var targetToShoot = target.transform.position;
            float rad = Mathf.Atan2(targetToShoot.y - barrel.transform.position.y, targetToShoot.x - barrel.transform.position.x) * Mathf.Rad2Deg;
            projectile.transform.rotation = Quaternion.Euler(new Vector3(0, 0, rad));
      
            projectile.SetActive(true);
            projectile.GetComponent<EnemyBullet>().damage = attackDamage;
        }
        sound.PlayOneShot(sound.clip);
    }

    public void EndAttack()
    {
        anim.SetBool("Run", true);
        anim.SetBool("Idle", false);
        canAttack = true;
        GetComponent<EnemyMovement>().canMove = true;
    }

    private void OnDrawGizmos()
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
