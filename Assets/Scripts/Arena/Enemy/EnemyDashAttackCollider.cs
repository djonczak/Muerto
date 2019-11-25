using UnityEngine;

public class EnemyDashAttackCollider : MonoBehaviour
{
    [SerializeField]
    private float damage;

    private void Start()
    {
        damage = GetComponentInParent<EnemyDashAttack>().attackDamage;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.tag == "Player" && GetComponentInParent<EnemyDashAttack>().isDashing == true)
        {
            collision.GetComponent<IDamage>().TakeDamage(damage, DamageType.Normal);
        }
    }
}
