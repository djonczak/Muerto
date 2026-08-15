using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game.Arena.AI
{
    public class EnemyRangeAttack : MonoBehaviour, IReset
    {
        [Header("Range Attack Options")]

        [SerializeField] private float attackDamage = 1;
        [SerializeField] private float attackSpeed = 0.8f;
        [SerializeField] private float attackRadius = 1.76f;
        [SerializeField] private GameObject target;

        public Transform barrel;

        private Animator animator;
        private float timer;
        public bool canAttack = true;
        private AudioSource audioSource;
        private Player.PlayerHP playerHP;
        private EnemyHP enemyHP;

        private const string AttackKey = "Attack";
        private const string IdleKey = "Idle";
        private const string RunKey = "Run";
        private const string BulletKey = "Bullet";
        private const string MuzzleFlashKey = "MuzzleFlash";

        private void Awake()
        {
            animator = GetComponent<Animator>();
            audioSource = GetComponent<AudioSource>();
            enemyHP = GetComponent<EnemyHP>();
        }

        private void Start()
        {
            target = PlayerObject.GetPlayerObject();
            playerHP = target.GetComponent<Player.PlayerHP>();
        }

        private void Update()
        {
            CheckAttack();
        }

        private void CheckAttack()
        {
            if (playerHP.isAlive == true && enemyHP.isAlive)
            {
                if (canAttack == true)
                {
                    var distance = Vector3.Distance(transform.position, target.transform.position);
                    if (distance <= attackRadius)
                    {
                        timer += Time.deltaTime;
                        GetComponent<EnemyMovement>().canMove = false;
                        animator.SetBool(IdleKey, true);
                        animator.SetBool(RunKey, false);

                        if (timer >= attackSpeed)
                        {
                            animator.SetTrigger(AttackKey);
                            canAttack = false;
                            timer = 0f;
                        }
                    }
                    else
                    {
                        timer = 0f;
                        animator.SetBool(IdleKey, false);
                        animator.SetBool(RunKey, true);
                        GetComponent<EnemyMovement>().canMove = true;
                    }
                }
            }
        }

        public void ShootProjectile()
        {
            GameObject projectile = Pooler.ObjectPooler.instance.GetPooledObject(BulletKey);
            if (projectile != null)
            {
                projectile.transform.position = barrel.transform.position;
                projectile.transform.rotation = barrel.transform.rotation;
                projectile.transform.rotation = CalculateDirection();
                MuzzleFlashEffect();
                projectile.SetActive(true);
                projectile.GetComponent<EnemyBullet>().damage = attackDamage;
            }
            audioSource.PlayOneShot(audioSource.clip);
        }

        private Quaternion CalculateDirection()
        {
            var targetToShoot = target.transform.position;
            float rad = Mathf.Atan2(targetToShoot.y - barrel.transform.position.y, targetToShoot.x - barrel.transform.position.x) * Mathf.Rad2Deg;
            return Quaternion.Euler(new Vector3(0, 0, rad));
        }

        public void EndAttack()
        {
            animator.SetBool(RunKey, true);
            animator.SetBool(IdleKey, false);
            canAttack = true;
            GetComponent<EnemyMovement>().canMove = true;
        }

        private void MuzzleFlashEffect()
        {
            GameObject flash = Pooler.ObjectPooler.instance.GetPooledObject(MuzzleFlashKey);
            if (flash != null)
            {
                flash.transform.position = barrel.transform.position;
                flash.SetActive(true);
            }
        }

        private void OnDrawGizmos()
        {
            Gizmos.color = Color.yellow;
            Gizmos.DrawWireSphere(transform.position, attackRadius);
        }

        public void OnDeathReset()
        {
            timer = 0f;
            animator.SetBool(RunKey, true);
            animator.SetBool(IdleKey, false);
            canAttack = true;
            GetComponent<EnemyMovement>().canMove = true;
        }
    }
}
