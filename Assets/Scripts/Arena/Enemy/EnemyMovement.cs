using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
[RequireComponent(typeof(SpriteRenderer))]
public class EnemyMovement : MonoBehaviour
{
    [SerializeField]
    private float moveSpeed = 0.7f;
    public bool isAlive;

    SpriteRenderer sprite;
    public Transform target;

    void Start()
    {
        sprite = GetComponent<SpriteRenderer>();
    }
	
	void FixedUpdate ()
    {
        Move();
	}

    void Move()
    {
        if (isAlive == true)
        {
            if (target != null)
            {
                transform.position = Vector3.MoveTowards(transform.position, target.transform.position, moveSpeed * Time.deltaTime);

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
    }
}
