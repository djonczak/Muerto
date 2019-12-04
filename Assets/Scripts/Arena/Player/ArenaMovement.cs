using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Animator))]
public class ArenaMovement : MonoBehaviour
{
    [SerializeField] private float moveSpeed = 1f;

    private Animator anim;
    public Vector3 mousePosition;
    public bool canMove;
    
    void Start()
    {
        anim = GetComponent<Animator>();
    }

    void Update()
    {
        Move();
    }

    void Move()
    {
        if (canMove == true)
        {
            mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            mousePosition.z = 0;
            transform.position = Vector3.MoveTowards(transform.position, mousePosition, moveSpeed * Time.deltaTime);

            if (mousePosition.x > transform.position.x)
            {
                transform.rotation = Quaternion.Euler(0, 0, 0);
                transform.localScale = new Vector3(transform.localScale.x, transform.localScale.y, 1);
            }
            else
            {
                transform.rotation = Quaternion.Euler(0, 180, 0);
                transform.localScale = new Vector3(transform.localScale.x, transform.localScale.y, -1);
            }
        }

        Vector3 minScreenBounds = Camera.main.ScreenToWorldPoint(new Vector3(0, 0, 0));
        Vector3 maxScreenBounds = Camera.main.ScreenToWorldPoint(new Vector3(Screen.width, Screen.height, 0));

        transform.position = new Vector3(Mathf.Clamp(transform.position.x, minScreenBounds.x + 0.2f, maxScreenBounds.x - 0.2f), Mathf.Clamp(transform.position.y, minScreenBounds.y + 0.2f, maxScreenBounds.y - 0.2f), transform.position.z);

        Animations();
    }

    private void Animations()
    {
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

    private float DistanceBetween(Vector3 player, Vector3 mouse)
    {
        return Vector3.Distance(player,mouse);
    }
}