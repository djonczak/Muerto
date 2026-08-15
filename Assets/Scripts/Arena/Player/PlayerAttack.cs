using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Game.Arena.Extensions;

namespace Game.Arena.Player 
{
    public class PlayerAttack : MonoBehaviour
    {
        [SerializeField] private float dashSpeed = 0.0001f;
        [SerializeField] private float dashCooldown = 0.5f;
        [SerializeField] private float dashRange = 2.5f;
        [SerializeField] private float dashTime = 0.9f;
        [SerializeField] private Image cooldownBar;
        private bool canDash = true;
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
            if (Input.GetKey(KeyCode.Mouse0))
            {
                if (canDash && isDashing == false)
                {
                    StartCoroutine(DashTimer());
                }
            }
        }

        private IEnumerator DashTimer()
        {
            canDash = false;
            isDashing = true;
            playerHP.canBeHurt = false;
            arenaMovement.canMove = false;
            animator.SetTrigger(PunchKey);

            Vector2 mousePosition = Vector3Extension.MousePosition();
            Vector2 currentPosition = transform.position;
            Vector2 direction = (mousePosition - currentPosition).normalized;
            Vector2 targetPosition = currentPosition + direction * dashRange;
            float timer = 0f;
            while (timer < dashTime)
            {
                rigidbody2D.MovePosition(Vector2.MoveTowards(rigidbody2D.position, targetPosition, dashSpeed * Time.fixedDeltaTime));
                timer += Time.deltaTime;
                yield return new WaitForFixedUpdate(); 
            }

            rigidbody2D.velocity = Vector2.zero;
            isDashing = false;
            playerHP.canBeHurt = true;
            arenaMovement.canMove = true;
            cooldownBar.fillAmount = 0;
            cooldownBar.enabled = true;
            timer = 0f;
            while(timer <= dashCooldown)
            {
                var value = Mathf.Lerp(0f, 1f, timer / dashCooldown);
                cooldownBar.fillAmount = value;
                timer += Time.deltaTime;
                yield return null;
            }
            cooldownBar.enabled = false;
           canDash = true;
        }
    }
}
