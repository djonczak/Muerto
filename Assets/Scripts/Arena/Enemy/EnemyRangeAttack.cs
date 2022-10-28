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

        private Animator _animator;
        private float _timer;
        private bool _canAttack = true;
        private AudioSource _audioSource;

        private const string AttackKey = "Attack";
        private const string IdleKey = "Idle";
        private const string RunKey = "Run";
        private const string BulletKey = "Bullet";
        private const string MuzzleFlashKey = "MuzzleFlash";

        private void Awake()
        {
            _animator = GetComponent<Animator>();
            _audioSource = GetComponent<AudioSource>();
        }

        private void Start()
        {
            target = PlayerObject.GetPlayerObject();
        }

        private void Update()
        {
            CheckAttack();
        }

        private void CheckAttack()
        {
            if (GetComponent<EnemyHP>().isAlive == true)
            {
                if (_canAttack == true)
                {
                    var distance = Vector3.Distance(transform.position, target.transform.position);
                    if (distance <= attackRadius)
                    {
                        _timer += Time.deltaTime;
                        GetComponent<EnemyMovement>().canMove = false;
                        _animator.SetBool(IdleKey, true);
                        _animator.SetBool(RunKey, false);

                        if (_timer >= attackSpeed)
                        {
                            _animator.SetTrigger(AttackKey);
                            _canAttack = false;
                            _timer = 0f;
                        }
                    }
                    else
                    {
                        _timer = 0f;
                        _animator.SetBool(IdleKey, false);
                        _animator.SetBool(RunKey, true);
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
            _audioSource.PlayOneShot(_audioSource.clip);
        }

        private Quaternion CalculateDirection()
        {
            var targetToShoot = target.transform.position;
            float rad = Mathf.Atan2(targetToShoot.y - barrel.transform.position.y, targetToShoot.x - barrel.transform.position.x) * Mathf.Rad2Deg;
            return Quaternion.Euler(new Vector3(0, 0, rad));
        }

        public void EndAttack()
        {
            _animator.SetBool(RunKey, true);
            _animator.SetBool(IdleKey, false);
            _canAttack = true;
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
            _timer = 0f;
            _animator.SetBool(RunKey, true);
            _animator.SetBool(IdleKey, false);
            _canAttack = true;
            GetComponent<EnemyMovement>().canMove = true;
        }
    }
}
