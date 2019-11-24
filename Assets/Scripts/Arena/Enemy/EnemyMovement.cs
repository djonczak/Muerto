﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]
public class EnemyMovement : MonoBehaviour
{
    [SerializeField]
    private float moveSpeed = 0.7f;
    public bool isAlive;

    SpriteRenderer sprite;
    public GameObject target;

    void Start()
    {
        sprite = GetComponent<SpriteRenderer>();
        target = GameObject.FindGameObjectWithTag("Player");
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
