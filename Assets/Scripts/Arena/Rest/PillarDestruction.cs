using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PillarDestruction : MonoBehaviour
{
    public Sprite destroyedPillar;
    private bool isDestroyed = false;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player" && isDestroyed == false)
        {
            if (collision.gameObject.GetComponent<PlayerAttack>().isDashing == true || collision.gameObject.GetComponent<TableChargeAbility>().isCharging == true)
            {
                DestroyPillar();
                isDestroyed = true;
            }
        }

        if (collision.gameObject.tag == "Chupacabra" && isDestroyed == false)
        {
            if (collision.gameObject.GetComponent<EnemyDashAttack>().isDashing == true)
            {
                DestroyPillar();
                isDestroyed = true;
            }
        }
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player" && isDestroyed == false)
        {
            if (collision.gameObject.GetComponent<PlayerAttack>().isDashing == true || collision.gameObject.GetComponent<TableChargeAbility>().isCharging == true)
            {
                DestroyPillar();
                isDestroyed = true;
            }
        }

        if (collision.gameObject.tag == "Chupacabra" && isDestroyed == false)
        {
            if (collision.gameObject.GetComponent<EnemyDashAttack>().isDashing == true)
            {
                DestroyPillar();
                isDestroyed = true;
            }
        }
    }

    private void DestroyPillar()
    {
        GameObject destructionEffect = ObjectPooler.instance.GetPooledObject("DestructionEffect");
        if (destructionEffect != null)
        {
            destructionEffect.transform.position = new Vector2(transform.position.x + 0.13f, transform.position.y + 0.1f);
            destructionEffect.SetActive(true);
        }
        GetComponent<SpriteRenderer>().sprite = destroyedPillar;
        GetComponent<BoxCollider2D>().enabled = false;
    }
}
