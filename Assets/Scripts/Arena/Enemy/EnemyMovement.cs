using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]
public class EnemyMovement : MonoBehaviour
{
    public bool canMove;
    public bool isRanged;

    [SerializeField] private float moveSpeed = 0.7f;
    [SerializeField] private GameObject target;

    private SpriteRenderer sprite;

    private void Awake()
    {
        sprite = GetComponent<SpriteRenderer>();
        target = PlayerObject.GetPlayerObject(); 
    }
	
	private void FixedUpdate ()
    {
        Move();
        Rotation();
	}

    private void Move()
    {
        if (canMove == true)
        {
            if (target != null)
            {
                transform.position = Vector3.MoveTowards(transform.position, target.transform.position, moveSpeed * Time.deltaTime);
            }
        }
    }

    private void Rotation()
    {
        Melee();
        Ranged();
    }

    private void Melee()
    {
        if (canMove == true && isRanged == false)
        {
            if (target.transform.position.x > transform.position.x)
            {
                sprite.flipX = false;
            }
            else
            {
                sprite.flipX = true;
            }
        }
    }

    private void Ranged()
    {
        if (isRanged == true)
        {
            if (target.transform.position.x > transform.position.x)
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
    }
}
