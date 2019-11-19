using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cloud : MonoBehaviour
{
    public Transform startPoint;
    public float moveSpeed;

    void Start()
    {
        GetComponent<Rigidbody2D>().velocity = transform.right * moveSpeed;  
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        transform.position = new Vector3(startPoint.position.x, transform.position.y, transform.position.z);
    }

}
