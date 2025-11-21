using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game.Arena.Player 
{
    public class PlayerAttack : MonoBehaviour
    {
        [SerializeField] private float dashSpeed = 0.0001f;
        [SerializeField] private float dashCooldown = 0.5f;
        [SerializeField] private float dashRange = 2.5f;

        private float timer = 0;
        public bool isDashing = false;

        private Animator animator;
        private PlayerHP playerHP;
        private ArenaMovement arenaMovement;
        private Rigidbody2D rigidbody2D;

        private const string PunchKey = "Punch";

        private void Awake()
        {
            animator = GetComponent<Animator>();
            playerHP = GetComponent<PlayerHP>();
            arenaMovement = GetComponent<ArenaMovement>();
            rigidbody2D = GetComponent<Rigidbody2D>();
        }

        private void FixedUpdate()
        {
            Dash();
        }

        private void Dash()
        {
            if (Input.GetKeyDown(KeyCode.Mouse0))
            {
                if (timer > dashCooldown && !isDashing)
                {
                    isDashing = true;
                    animator.SetTrigger(PunchKey);
                    timer = 0;
                    playerHP.canBeHurt = false;
                    arenaMovement.canMove = false;
                }
            }

            if (isDashing == true)
            {
                playerHP.canBeHurt = false;
                float step = (1f * dashSpeed) * Time.fixedDeltaTime;
                transform.position = Vector3.MoveTowards(transform.position, Vector3Extension.MousePosition(), step / dashRange);
                transform.position.Normalize();
            }
            else
            {
                timer += Time.fixedDeltaTime;
            }

            rigidbody2D.velocity = Vector2.zero;
        }

        public void EndDash()
        {
            playerHP.canBeHurt = true;
            isDashing = false;
            arenaMovement.canMove = true;
        }
    }
}
