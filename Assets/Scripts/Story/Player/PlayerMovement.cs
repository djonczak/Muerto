using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(BoxCollider2D))]

public class PlayerMovement : MonoBehaviour
{
    SpriteRenderer sprite;
    Rigidbody2D rb;
    [SerializeField]
    private float moveSpeed = 20f;
    Animator anim;
    public bool Scene = false;

    void Start()
    {
        sprite = GetComponent<SpriteRenderer>();
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        Cursor.SetCursor(null, Vector2.zero, CursorMode.Auto);
    }

    void FixedUpdate()
    {
        Movement();
    }

    private void Movement()
    {
        if (Scene == false)
        {
            float x = Input.GetAxisRaw("Horizontal");
            Vector2 movement = new Vector2(x, rb.velocity.y);
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
 