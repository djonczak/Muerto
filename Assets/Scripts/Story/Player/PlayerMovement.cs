using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game.Player
{
    [RequireComponent(typeof(Rigidbody2D))]
    [RequireComponent(typeof(BoxCollider2D))]

    public class PlayerMovement : MonoBehaviour
    {
        public bool CanMove;

        [SerializeField] private float moveSpeed = 20f;

        private SpriteRenderer _spriteRenderer;
        private Rigidbody2D _rigidbody;
        private Animator _animator;

        private const string SpeedKey = "Speed";
        private const string HorizontalInput = "Horizontal";

        private void Awake()
        {
            _spriteRenderer = GetComponent<SpriteRenderer>();
            _rigidbody = GetComponent<Rigidbody2D>();
            _animator = GetComponent<Animator>();
        }

        private void Start()
        {
            Cursor.SetCursor(null, Vector2.zero, CursorMode.Auto);
            CameraManager.CameraFade.Instance.FadedScreen += ActivateMovement;
        }

        private void FixedUpdate()
        {
            Movement();
        }

        private void ActivateMovement()
        {
            CameraManager.CameraFade.Instance.FadedScreen -= ActivateMovement;
            CanMove = true;
        }

        private void Movement()
        {
            if (CanMove)
            {
                float axisX = Input.GetAxisRaw(HorizontalInput);
                Vector2 movement = new Vector2(axisX, _rigidbody.velocity.y);
                var moveVelocity = movement * moveSpeed;
                _rigidbody.MovePosition(_rigidbody.position + moveVelocity * Time.fixedDeltaTime);

                if (movement.x < 0)
                {
                    _spriteRenderer.flipX = true;
                }
                else if (movement.x > 0)
                {
                    _spriteRenderer.flipX = false;
                }
                _animator.SetFloat(SpeedKey, Mathf.Abs(movement.magnitude));
            }
        }
    }
}
 