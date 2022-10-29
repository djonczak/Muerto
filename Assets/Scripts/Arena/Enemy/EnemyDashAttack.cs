using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game.Arena.AI
{
    public class EnemyDashAttack : MonoBehaviour, IReset
    {
        [Header("Dash Attack Settings")]
        public float attackDamage = 1;
        [SerializeField] private float dashCooldown = 2.5f;
        [SerializeField] private float dashRadius = 2.7f;
        [SerializeField] private float dashRange = 2.5f;
        [SerializeField] private float dashSpeed = 0.5f;
        [SerializeField] private GameObject target;

        private Animator _animator;
        private float _timer;
        private bool _canDash = true;
        public bool isDashing = false;
        private Vector3 _jumpPosition;

        private const string AttackKey = "Attack";
        private const string RunKey = "Run";

        private EnemyHP _enemyHP;

        private void Awake()
        {
            _animator = GetComponent<Animator>();
            _enemyHP = GetComponent<EnemyHP>();
        }

        private void Start()
        {
            target = PlayerObject.GetPlayerObject();
        }

        private void FixedUpdate()
        {
            DashCooldown();
            Dash();
        }

        private void DashCooldown()
        {
            if (_enemyHP.isAlive == true)
            {
                if (isDashing == false)
                {
                    _timer += Time.deltaTime;
                    if (_timer >= dashCooldown && _canDash == true)
                    {
                        var distance = Vector3.Distance(transform.position, target.transform.position);
                        if (distance <= dashRadius)
                        {
                            PrepareForDash();
                        }
                    }
                }
            }
        }

        private void PrepareForDash()
        {
            _animator.SetTrigger(AttackKey);
            _jumpPosition = new Vector3(target.transform.position.x, target.transform.position.y, 0);
            _timer = 0f;
            GetComponent<EnemyMovement>().canMove = false;
            _animator.SetBool(RunKey, false);
            _canDash = false;
        }

        public void StartDash()
        {
           isDashing = true;
        }

        private void Dash()
        {
            if (isDashing == true)
            {
                float step = (dashSpeed) * Time.fixedDeltaTime;
                transform.position = Vector3.MoveTowards(transform.position, _jumpPosition, step / dashRange);
                transform.position.Normalize();

                if (0.1f > DistanceBetween(transform.position, _jumpPosition))
                {
                    DashEnd();
                }
            }
        }

        public void DashEnd()
        {
            _animator.ResetTrigger(AttackKey);
            isDashing = false;
            _canDash = true;
            _animator.SetBool(RunKey, true);
            GetComponent<EnemyMovement>().canMove = true;
        }

        private float DistanceBetween(Vector3 enemy, Vector3 placeToJump)
        {
            var distance = Vector3.Distance(enemy, placeToJump);
            return distance;
        }

        private void OnDrawGizmos()
        {
            Gizmos.color = Color.blue;
            Gizmos.DrawWireSphere(transform.position, dashRadius);
        }

        public void OnDeathReset()
        {
            _timer = 0f;
            _animator.SetBool(RunKey, true);
            GetComponent<EnemyMovement>().canMove = true;
            isDashing = false;
            _canDash = true;
        }
    }
}
