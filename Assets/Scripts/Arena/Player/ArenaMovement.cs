using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game.Arena.Player {

    [RequireComponent(typeof(Animator))]
    public class ArenaMovement : MonoBehaviour
    {
        [SerializeField] private float moveSpeed = 1f;

        public bool canMove;

        private Animator _animator;

        private const string IdleKey = "Idle";
        private const string RunKey = "Run";

        void Awake()
        {
            _animator = GetComponent<Animator>();
        }

        void Update()
        {
            MoveTowardsMouse();
        }

        void MoveTowardsMouse()
        {
            if (canMove == true)
            {
                transform.position = Vector3.MoveTowards(transform.position, Vector3Extension.MousePosition(), moveSpeed * Time.deltaTime);

                CharacterRotation();
            }

            MovementBoundsToScreen();

            AnimationsControll();
        }

        private void CharacterRotation()
        {
            if (Vector3Extension.MousePosition().x > transform.position.x)
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

        private void MovementBoundsToScreen()
        {
            Vector3 minScreenBounds = Camera.main.ScreenToWorldPoint(new Vector3(0, 0, 0));
            Vector3 maxScreenBounds = Camera.main.ScreenToWorldPoint(new Vector3(Screen.width, Screen.height, 0));

            transform.position = new Vector3(Mathf.Clamp(transform.position.x, minScreenBounds.x + 0.2f, maxScreenBounds.x - 0.2f), Mathf.Clamp(transform.position.y, minScreenBounds.y + 0.2f, maxScreenBounds.y - 0.2f), transform.position.z);
        }

        private void AnimationsControll()
        {
            if (0.01f < Vector3Extension.DistanceBetweenPlayerMouse(transform.position, Vector3Extension.MousePosition()))
            {
                _animator.SetBool(RunKey, true);
                _animator.SetBool(IdleKey, false);
            }
            else
            {
                _animator.SetBool(RunKey, false);
                _animator.SetBool(IdleKey, true);
            }
        }
    }
}