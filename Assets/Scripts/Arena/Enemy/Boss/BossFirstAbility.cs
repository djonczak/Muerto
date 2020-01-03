using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossFirstAbility : MonoBehaviour
{
    [Header("Range Attack Options")]
    [SerializeField] private float activateRadius = 1.76f;
    [SerializeField] private int amountBulletsToSpawn = 3;
    [SerializeField] private GameObject target;

    public AudioClip abilitySound;

    private Animator anim;
    private bool canUseAbility = true;
    private AudioSource sound;
    private BossMovement bossMove;

    private void Awake()
    {
        anim = GetComponent<Animator>();
        sound = GetComponent<AudioSource>();
        bossMove = GetComponent<BossMovement>();
    }

    private void Start()
    {
        target = bossMove.target;
    }

    private void Update()
    {
        CheckAbility();
    }

    private void CheckAbility()
    {
        if (canUseAbility == true)
        {
            var distance = Vector3.Distance(transform.position, target.transform.position);
            if (distance > activateRadius)
            {
                bossMove.canMove = false;
                anim.SetBool("Idle", true);
                anim.SetBool("Run", false);
                anim.SetTrigger("Attack");
                canUseAbility = false;
            }
            else
            {
                anim.SetBool("Idle", false);
                anim.SetBool("Run", true);
                bossMove.canMove = true;
            }
        }
    }

    public void ShootProjectile()
    {
        for (int i = 0; i <= amountBulletsToSpawn; i++)
        {
            GameObject projectile = ObjectPooler.instance.GetPooledObject("SoulBullet");
            if (projectile != null)
            {
                var posX = Random.Range(transform.position.x - 3, transform.position.x + 3);
                var posY = Random.Range(transform.position.y - 3, transform.position.y + 3);
                projectile.transform.position = new Vector2(posX, posY);
                projectile.SetActive(true);
                projectile.GetComponent<BossBullet>().target = target;
            }
        }
        sound.PlayOneShot(abilitySound);
    }

    public void EndAttack()
    {
        anim.SetBool("Run", true);
        anim.SetBool("Idle", false);
        canUseAbility = true;
        bossMove.canMove = true;
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, activateRadius);
    }
}
