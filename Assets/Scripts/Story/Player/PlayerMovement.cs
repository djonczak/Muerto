using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(BoxCollider2D))]

public class PlayerMovement : MonoBehaviour
{
    public bool Scene = false;

    [SerializeField] private float moveSpeed = 20f;

    private SpriteRenderer sprite;
    private Rigidbody2D rb;
    private Animator anim;

    private void Awake()
    {
        sprite = GetComponent<SpriteRenderer>();
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
    }

    private void Start()
    {
        Cursor.SetCursor(null, Vector2.zero, CursorMode.Auto);
    }

    private void FixedUpdate()
    {
        Movement();
    }

    private void Movement()
    {
        if (Scene == false)
        {
            float axisX = Input.GetAxisRaw("Horizontal");
            Vector2 movement = new Vector2(axisX, rb.velocity.y);
            var moveVelocity = movement * moveSpeed;
            rb.MovePosition(rb.position + moveVelocity * Time.fixedDeltaTime);

            if (movement.x < 0)
            {
                sprite.flipX = true;
            }
            else if (movement.x > 0)
            {
                sprite.flipX = false;
            }
            anim.SetFloat("Speed", Mathf.Abs(movement.magnitude));
        }
    }
}
 