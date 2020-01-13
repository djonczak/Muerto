using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAttack : MonoBehaviour
{
    [SerializeField] private float dashSpeed = 0.0001f;
    [SerializeField] private float dashCooldown = 0.5f;
    [SerializeField] private float dashRange = 2.5f;

    private float timer = 0;
    public bool isDashing = false;
    private Animator anim;

    private void Awake()
    {
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
            GetComponent<ArenaMovement>().canMove = false;
        }

        if (isDashing == true)
        {
            float step = (1f * dashSpeed) * Time.fixedDeltaTime;
            transform.position = Vector3.MoveTowards(transform.position, Vector3Extension.MousePosition(), step / dashRange);
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
        GetComponent<ArenaMovement>().canMove = true;
    }
}
