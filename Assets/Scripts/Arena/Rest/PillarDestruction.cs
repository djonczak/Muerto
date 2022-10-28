using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game.VFX 
{
    public class PillarDestruction : MonoBehaviour
    {
        public Sprite destroyedPillar;
        private bool _isDestroyed = false;

        private const string PlayerTag = "Player";
        private const string ChupacabraTag = "Chupacabra";
        private const string DestructionEffect = "DestructionEffect";

        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (collision.gameObject.tag == PlayerTag && _isDestroyed == false)
            {
                if (collision.gameObject.GetComponent<Arena.Player.PlayerAttack>().isDashing == true || collision.gameObject.GetComponent<Arena.Player.TableChargeAbility>().isCharging == true)
                {
                    DestroyPillar();
                    _isDestroyed = true;
                }
            }

            if (collision.gameObject.tag == ChupacabraTag && _isDestroyed == false)
            {
                if (collision.gameObject.GetComponent<Arena.AI.EnemyDashAttack>().isDashing == true)
                {
                    DestroyPillar();
                    _isDestroyed = true;
                }
            }
        }

        private void OnTriggerStay2D(Collider2D collision)
        {
            if (collision.gameObject.tag == PlayerTag && _isDestroyed == false)
            {
                if (collision.gameObject.GetComponent<Arena.Player.PlayerAttack>().isDashing == true || collision.gameObject.GetComponent<Arena.Player.TableChargeAbility>().isCharging == true)
                {
                    DestroyPillar();
                    _isDestroyed = true;
                }
            }

            if (collision.gameObject.tag == ChupacabraTag && _isDestroyed == false)
            {
                if (collision.gameObject.GetComponent<Arena.AI.EnemyDashAttack>().isDashing == true)
                {
                    DestroyPillar();
                    _isDestroyed = true;
                }
            }
        }

        private void DestroyPillar()
        {
            GameObject destructionEffect = Arena.Pooler.ObjectPooler.instance.GetPooledObject(DestructionEffect);
            if (destructionEffect != null)
            {
                destructionEffect.transform.position = new Vector2(transform.position.x + 0.13f, transform.position.y + 0.1f);
                destructionEffect.SetActive(true);
            }
            GetComponent<SpriteRenderer>().sprite = destroyedPillar;
            GetComponent<BoxCollider2D>().enabled = false;
        }
    }
}
