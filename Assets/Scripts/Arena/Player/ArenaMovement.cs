using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]
[RequireComponent(typeof(AudioSource))]
public class ArenaMovement : MonoBehaviour
{
    public float moveSpeed = 0.5f;
    public float dashSpeed = 0.0001f;
    public float dashCooldown;
    float timer = 0;
    [SerializeField]
    float dashRange = 2.5f;

    SpriteRenderer character;
    Animator anim;
    Vector3 mousePosition;

    public bool isDashing;

    void Start()
    {
        character = GetComponent<SpriteRenderer>();
        anim = GetComponent<Animator>();
    }

    void FixedUpdate()
    {
        Move();
        Dash();
    }

    void Move()
    {
        mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        mousePosition.z = 0;
        transform.position = Vector3.MoveTowards(transform.position, mousePosition, moveSpeed * Time.deltaTime);

        if (mousePosition.x > transform.position.x)
        {
            character.flipX = false;
        }
        else
        {
            character.flipX = true;
        }

        Vector3 minScreenBounds = Camera.main.ScreenToWorldPoint(new Vector3(0, 0, 0));
        Vector3 maxScreenBounds = Camera.main.ScreenToWorldPoint(new Vector3(Screen.width, Screen.height, 0));

        transform.position = new Vector3(Mathf.Clamp(transform.position.x, minScreenBounds.x + 0.2f, maxScreenBounds.x - 0.2f), Mathf.Clamp(transform.position.y, minScreenBounds.y + 0.2f, maxScreenBounds.y - 0.2f), transform.position.z);

        if (0.01f < DistanceBetween(transform.position, mousePosition))
        {
            anim.SetBool("Run", true);
            anim.SetBool("Idle", false);
        }
        else
        {
            anim.SetBool("Run", false);
            anim.SetBool("Idle", true);
        }
    }

    private void Dash()
    {
        if (Input.GetKeyDown(KeyCode.Mouse0) && timer > dashCooldown && !isDashing)
        {
            isDashing = true;
            StartCoroutine("EndDash");
            anim.SetTrigger("Punch");
            timer = 0;
        }

        if (isDashing)
        {
            float step = (moveSpeed * dashSpeed) * Time.fixedDeltaTime;
            transform.position = Vector3.MoveTowards(transform.position, mousePosition, step / dashRange);
            transform.position.Normalize();
        }
        else
        {
            timer += Time.fixedDeltaTime;
        }
    }

    private float DistanceBetween(Vector3 player, Vector3 mouse)
    {
        return Vector3.Distance(player,mouse);
    }

    IEnumerator EndDash()
    {
        yield return new WaitForSeconds(0.1f);
        isDashing = false;
    }

}