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

        private float _timer = 0;
        public bool isDashing = false;
        private Animator _animator;
        private PlayerHP _playerHP;
        private const string PunchKey = "Punch";

        private void Awake()
        {
            _animator = GetComponent<Animator>();
            _playerHP = GetComponent<PlayerHP>();
        }

        private void FixedUpdate()
        {
            Dash();
        }

        private void Dash()
        {
            if (Input.GetKeyDown(KeyCode.Mouse0) && _timer > dashCooldown && !isDashing)
            {
                isDashing = true;
                _animator.SetTrigger(PunchKey);
                _timer = 0;
                _playerHP.canBeHurt = false;
                GetComponent<ArenaMovement>().canMove = false;
            }

            if (isDashing == true)
            {
                _playerHP.canBeHurt = false;
                float step = (1f * dashSpeed) * Time.fixedDeltaTime;
                transform.position = Vector3.MoveTowards(transform.position, Vector3Extension.MousePosition(), step / dashRange);
                transform.position.Normalize();
            }
            else
            {
                _timer += Time.fixedDeltaTime;
            }
        }

        public void EndDash()
        {
            _playerHP.canBeHurt = true;
            isDashing = false;
            GetComponent<ArenaMovement>().canMove = true;
        }
    }
}
