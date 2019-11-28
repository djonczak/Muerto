using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAttack : MonoBehaviour
{
    [SerializeField]
    private float dashSpeed = 0.0001f;
    [SerializeField]
    private float dashCooldown = 0.5f;
    float timer = 0;
    [SerializeField]
    private float dashRange = 2.5f;
    private ArenaMovement movement;

    public bool isDashing = false;
    Animator anim;

    private void Start()
    {
        movement = GetComponent<ArenaMovement>();
        anim = GetComponent<Animator>();
    }

    private void FixedUpdate()
    {
        Dash();
    }

    private void Dash()
    {
        if (Input.GetKeyDown(KeyCode.Mouse0) && timer > dashCooldown && !isDashing)
        {
            isDashing = true;
            anim.SetTrigger("Punch");
            timer = 0;
        }

        if (isDashing == true)
        {
            float step = (movement.moveSpeed * dashSpeed) * Time.fixedDeltaTime;
            transform.position = Vector3.MoveTowards(transform.position, movement.mousePosition, step / dashRange);
            transform.position.Normalize();
        }
        else
        {
            timer += Time.fixedDeltaTime;
        }
    }

    public void EndDash()
    {
        isDashing = false;
    }
}
