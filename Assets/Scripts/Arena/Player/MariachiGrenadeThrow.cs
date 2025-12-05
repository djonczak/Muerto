using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game.Arena.Player 
{
    public class MariachiGrenadeThrow : MonoBehaviour
    {
        public bool disable = false;
        [SerializeField] private bool canUseAbility = true;

        public bool CanUseAbility
        {
            set => canUseAbility = value;
        }


        public float abilityCooldown = 15f;
        [SerializeField] private bool canUse = true;
        [SerializeField] private MariachiGrenade mariachiGrenade;
        private bool canThrow;
        private Animator animator;
        private PlayerAttack playerAttack;
        private ArenaMovement arenaMovement;
        private MariachiTrumpetAbility mariachiTrumpetAbility;
        private PlayerHP playerHP;
        private Vector3 throwPoint;

        private ISoundEffect iSoundEffect;

        private const string IdleKey = "Idle";
        private const string RunKey = "Run";
        private const string ThrowKey = "Throw";
        private const string FirstAbilityKey = "FirstAbility";

        private void Awake()
        {
            animator = GetComponent<Animator>();
            playerAttack = GetComponent<PlayerAttack>();
            iSoundEffect = GetComponent<ISoundEffect>();
            arenaMovement = GetComponent<ArenaMovement>();
            playerHP = GetComponent<PlayerHP>();
            mariachiTrumpetAbility = GetComponent<MariachiTrumpetAbility>();
        }

        private void Update()
        {
            if (disable == false)
            {
                if (canUseAbility)
                {
                    Input();
                }
            }
        }

        private void Input()
        {
            if (UnityEngine.Input.GetKeyDown(KeyCode.Q) && canUse == true)
            {
                PrepareForThrow();
            }

            if (canThrow == true)
            {
                if (UnityEngine.Input.GetKeyDown(KeyCode.Mouse0))
                {
                    var mouse = Camera.main.ScreenToWorldPoint(UnityEngine.Input.mousePosition);
                    mouse.z = 0f;
                    throwPoint = mouse;
                    Time.timeScale = 1f;
                    animator.SetTrigger(ThrowKey);
                    canThrow = false;
                }
                else
                {
                    if(Vector3Extension.MousePosition().x > transform.position.x)
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

        public void Throw()
        {
            iSoundEffect.PlayAbility1Sound();
            Game.UI.PlayerUI.instance.Used1Ability();
            playerHP.canBeHurt = true;
            arenaMovement.enabled = true;
            playerAttack.enabled = true;
            mariachiGrenade.ThrowGrenade(throwPoint);
            mariachiTrumpetAbility.CanUseAbility = true;
            StartCoroutine(AbilityCooldown(abilityCooldown));
        }

        private void PrepareForThrow()
        {
            mariachiTrumpetAbility.CanUseAbility = false;
            canUse = false;
            arenaMovement.enabled = false;
            playerAttack.enabled = false;
            animator.SetTrigger(FirstAbilityKey);
            animator.SetBool(RunKey, false);
            animator.SetBool(IdleKey, false);
            canThrow = true;
            playerHP.canBeHurt = false;
            Time.timeScale = 0.5f;
        }


        public void CancelAbility()
        {
            if (canThrow)
            {
                StopAllCoroutines();
                playerHP.canBeHurt = true;
                ArenaEvents.PlayerCharge();
                GetComponent<Rigidbody2D>().velocity = Vector2.zero;
                arenaMovement.enabled = true;
                playerAttack.enabled = true;
                canThrow = false;
            }
        }

        IEnumerator AbilityCooldown(float time)
        {
            yield return new WaitForSeconds(time);
            canUse = true;
        }
    }
}
