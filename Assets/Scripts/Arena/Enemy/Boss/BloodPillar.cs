using UnityEngine;

namespace Game.Arena.AI 
{

    public class BloodPillar : MonoBehaviour
    {
        [SerializeField] private float damage = 1f;

        private const string Player = "Player";

        public void UsePillar(Transform target)
        {
            transform.position = target.transform.position;
        }

        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (collision.tag == "Player")
            {
                collision.GetComponent<IDamage>().TakeDamage(damage, DamageType.Normal);

            }
        }
    }
}
