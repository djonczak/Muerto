using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game.Arena.Player
{
    public class DivingElbowAbility : MonoBehaviour
    {
        public bool disable = false;
        [SerializeField] private float damage = 1f;
        public float abilityCooldown = 5f;
        [SerializeField] private float abilityRange = 5f;
        [SerializeField] private float fallSpeed = 10f;
        [SerializeField] private bool canUse = true;
        [SerializeField] private bool preparedToJump = false;
        [SerializeField] private bool isFalling = false;
        [SerializeField] private LayerMask enemyLayer = 11;

        private Animator _animator;

        private const string RunKey = "Run";
        private const string IdleKey = "Idle";
        private const string FallAttackKey = "FallAttack";
        private const string DustWaveKey = "DustWave";


        private void Awake()
        {
            _animator = GetComponent<Animator>();
        }

        public void Update()
        {
            if (disable == false)
            {
                Input();

                FallDawn();
            }
        }

        private void Input()
        {
            if (UnityEngine.Input.GetKeyDown(KeyCode.Q) && canUse == true)
            {
                PrepareToJump();
            }

            if (preparedToJump == true)
            {
                if (UnityEngine.Input.GetKeyDown(KeyCode.Mouse0))
                {
                    Jump();
                }
            }
        }

        private void PrepareToJump()
        {
            canUse = false;
            preparedToJump = true;
            Time.timeScale = 0.5f;
            GetComponent<PlayerAttack>().enabled = false;
            GetComponent<PolygonCollider2D>().isTrigger = true;
            var secondAbility = GetComponent<TableChargeAbility>();
            if (secondAbility.disable == false)
            {
                secondAbility.enabled = false;
            }
            GetComponent<PlayerHP>().canBeHurt = false;
        }

        private void Jump()
        {
            Game.UI.PlayerUI.instance.Used1Ability();
            transform.position = new Vector3(Vector3Extension.MousePosition().x, transform.position.y + 1.7f, transform.position.z);
            Time.timeScale = 1f;
            GetComponent<ArenaMovement>().enabled = false;
            isFalling = true;
            _animator.SetBool(FallAttackKey, true);
            _animator.SetBool(RunKey, false);
            _animator.SetBool(IdleKey, false);
            preparedToJump = false;
        }

        private void FallDawn()
        {
            if (isFalling == true)
            {
                transform.position = Vector3.MoveTowards(transform.position, Vector3Extension.MousePosition(), fallSpeed * Time.deltaTime);
                if (0.01f > Vector3Extension.DistanceBetweenPlayerMouse(transform.position, Vector3Extension.MousePosition()))
                {
                    PoundAttack();
                }
            }
        }

        void PoundAttack()
        {
            Collider2D[] enemies = Physics2D.OverlapCircleAll(transform.position, abilityRange, enemyLayer);
            foreach (Collider2D enemy in enemies)
            {
                if (enemy.GetComponent<IDamage>() != null)
                {
                    enemy.GetComponent<IDamage>().TakeDamage(damage, DamageType.Normal);
                }
            }
            ParticleEffect();
            isFalling = false;
            GetComponent<ISoundEffect>().PlayAbility1Sound();
            _animator.SetBool(FallAttackKey, false);
            StartCoroutine(Cooldown(abilityCooldown));
        }

        IEnumerator Cooldown(float time)
        {
            GetComponent<TableChargeAbility>().enabled = true;
            GetComponent<PolygonCollider2D>().isTrigger = false;
            GetComponent<PlayerAttack>().enabled = true;
            GetComponent<ArenaMovement>().enabled = true;
            GetComponent<PlayerHP>().canBeHurt = true;
            yield return new WaitForSeconds(time);
            canUse = true;
        }

        private void ParticleEffect()
        {
            GameObject effect = Pooler.ObjectPooler.instance.GetPooledObject(DustWaveKey);
            if (effect != null)
            {
                effect.transform.position = transform.position;
                effect.SetActive(true);
            }
        }

        private void OnDrawGizmos()
        {
            Gizmos.color = Color.cyan;
            Gizmos.DrawWireSphere(transform.position, abilityRange);
        }
    }
}
