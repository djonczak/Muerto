﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossMovement : MonoBehaviour
{
    [SerializeField] private float moveSpeed = 0.7f;
    public bool canMove;

    public GameObject target;

    private void Awake()
    {
        target = GameObject.FindGameObjectWithTag("Player");
    }

    private void FixedUpdate()
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