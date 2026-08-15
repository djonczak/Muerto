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

        private Animator animator;
        private float timer;
        public bool canDash = true;
        public bool isDashing = false;
        private Vector3 jumpPosition;
        
        private const string AttackKey = "Attack";
        private const string RunKey = "Run";

        private EnemyHP enemyHP;
        private EnemyMovement enemyMovement;

        private void Awake()
        {
            animator = GetComponent<Animator>();
            enemyHP = GetComponent<EnemyHP>();
            enemyMovement = GetComponent<EnemyMovement>();
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
            if (enemyHP.isAlive == true)
            {
                if (isDashing == false)
                {
                    timer += Time.deltaTime;
                    if (timer >= dashCooldown && canDash == true)
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
            animator.SetTrigger(AttackKey);
            jumpPosition = new Vector3(target.transform.position.x, target.transform.position.y, 0);
            timer = 0f;
            enemyMovement.canMove = false;
            animator.SetBool(RunKey, false);
            canDash = false;
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
                transform.position = Vector3.MoveTowards(transform.position, jumpPosition, step / dashRange);
                transform.position.Normalize();

                if (0.1f > DistanceBetween(transform.position, jumpPosition))
                {
                    DashEnd();
                }
            }
        }

        public void DashEnd()
        {
            animator.ResetTrigger(AttackKey);
            isDashing = false;
            canDash = true;
            animator.SetBool(RunKey, true);
            enemyMovement.canMove = true;
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
            timer = 0f;
            animator.SetBool(RunKey, true);
            enemyMovement.canMove = true;
            isDashing = false;
            canDash = true;
        }
    }
}
