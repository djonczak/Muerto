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

        private SpriteRenderer spriteRenderer;
        private Rigidbody2D rigidbody2D;
        private Animator animator;

        private const string SpeedKey = "Speed";
        private const string HorizontalInput = "Horizontal";

        private void Awake()
        {
            spriteRenderer = GetComponent<SpriteRenderer>();
            rigidbody2D = GetComponent<Rigidbody2D>();
            animator = GetComponent<Animator>();
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
                Vector2 movement = new Vector2(axisX, rigidbody2D.velocity.y);
                var moveVelocity = movement * moveSpeed;
                rigidbody2D.MovePosition(rigidbody2D.position + moveVelocity * Time.fixedDeltaTime);

                if (movement.x < 0)
                {
                    spriteRenderer.flipX = true;
                }
                else if (movement.x > 0)
                {
                    spriteRenderer.flipX = false;
                }
                animator.SetFloat(SpeedKey, Mathf.Abs(movement.magnitude));
            }
        }
    }
}
 