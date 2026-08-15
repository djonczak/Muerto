using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Game.Arena.Events;

namespace Game.Arena.AI
{
    public class EnemyHP : MonoBehaviour, IDamage
    {
        [SerializeField] private float maxHP = 1f;
        [SerializeField] private float currentHP;
        public float xp;
        public bool isAlive;
        private bool specialDeath;

        private const string DeathEffectKey = "DeathEffect";

        private void OnEnable()
        {
            isAlive = true;
            currentHP = maxHP; 
            specialDeath = true;
        }

        public void TakeDamage(float amount, DamageType type)
        {
            if (type == DamageType.Normal)
            {
                currentHP -= amount;
            }
            else if (type == DamageType.KnockBack)
            {
                KnockBack();
            }

            if (currentHP <= 0)
            {
                Dead();
            }
        }

        public void DeathOverTime(float duration)
        {
            if (specialDeath)
            {
                specialDeath = false;
                var movement = GetComponent<EnemyMovement>();
                if (movement != null)
                {
                    movement.canMove = false;
                }

                var attackRange = GetComponent<EnemyRangeAttack>();
                if (attackRange != null)
                {
                    attackRange.canAttack = false;
                }

                var attackDash = GetComponent<EnemyDashAttack>();
                if (attackDash != null)
                {
                    attackDash.canDash = false;
                }

                var attackMelee = GetComponent<EnemyMeleeAttack>();
                if (attackMelee != null)
                {
                    attackMelee.canAttack = false;
                }
                StartCoroutine(DeathOverTimer(duration));
            }
        }

        private IEnumerator DeathOverTimer(float duration)
        {
            yield return new WaitForSeconds(duration);
            Dead();
        }

        private void KnockBack()
        {

        }

        private void Dead()
        {
            DeathEffect();
            var rigidbody2d = GetComponent<Rigidbody2D>();
            if(rigidbody2d != null)
            {
                rigidbody2d.velocity = Vector2.zero;
            }
            DeathEvent.EnemyDiedShake();
            DeathEvent.EnemyDied(xp);
            GetComponent<IReset>().OnDeathReset();
            gameObject.SetActive(false);
        }

        private void DeathEffect()
        {
            GameObject effect = Pooler.ObjectPooler.instance.GetPooledObject(DeathEffectKey);
            if (effect != null)
            {
                effect.transform.position = transform.position;
                effect.SetActive(true);
            }
        }
    }
}
