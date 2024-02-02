using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game.VFX
{
    public class Cloud : MonoBehaviour
    {
        public Transform startPoint;
        public float moveSpeed;
        private const string Catcher = "Catcher";
        private Rigidbody2D rigidbody2D;

        private void Awake()
        {
            rigidbody2D = GetComponent<Rigidbody2D>();
        }

        void Update()
        {
            transform.position += Vector3.right * moveSpeed * Time.deltaTime;
        }

        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (collision.gameObject.tag == Catcher)
            {
                transform.position = new Vector3(startPoint.position.x, transform.position.y, transform.position.z);
            }
        }
    }
}
