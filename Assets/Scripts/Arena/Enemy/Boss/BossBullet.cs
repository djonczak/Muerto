using System.Collections;
using UnityEngine;

public class BossBullet : MonoBehaviour
{
    [SerializeField] private float moveSpeed = 4f;
    [SerializeField] private float bulletDuration = 10f;
    [SerializeField] private float damage = 1f;
    public GameObject target;
    private bool canFollow = false;
    private Animator anim;
    private SpriteRenderer sprite;

    private void OnEnable()
    {
        canFollow = true;
        StartCoroutine("DisperseCooldown", bulletDuration);
    }

    private void Awake()
    {
        anim = GetComponent<Animator>();
        sprite = GetComponent<SpriteRenderer>();
    }

    private void Update()
    {
        if(canFollow == true && target != null)
        {
            transform.position = Vector3.MoveTowards(transform.position, target.transform.position, moveSpeed * Time.deltaTime);

            RotateTowardsPlayer();
        }
    }

    private void RotateTowardsPlayer()
    {
        if (target.transform.position.x > transform.position.x)
        {
            sprite.flipX = false;
        }
        else
        {
            sprite.flipX = true;
        }
    }

    private IEnumerator DisperseCooldown(float time)
    {
        yield return new WaitForSeconds(time);
        canFollow = false;
        anim.SetTrigger("Disperse");
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.tag == "Player")
        {
            collision.GetComponent<IDamage>().TakeDamage(damage,DamageType.Normal);
            anim.SetTrigger("Hit");
            canFollow = false;
        }
    }

    private void OnDisable()
    {
        canFollow = false;
        StopCoroutine("DisperseCooldown");
    }
}
