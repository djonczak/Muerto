using UnityEngine;

public class BossFirstAbility : MonoBehaviour
{
    [Header("Range Attack Options")]
    [SerializeField] private float activateRadius = 1.76f;
    [SerializeField] private int amountBulletsToSpawn = 1;
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
        target = PlayerObject.GetPlayerObject();
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
                canUseAbility = false;
                anim.SetTrigger("Ability");
            }
        }
    }

    public void ShootProjectile()
    {
        for (int i = 0; i < amountBulletsToSpawn; i++)
        {
            GameObject projectile = ObjectPooler.instance.GetPooledObject("BossBullet");
            if (projectile != null)
            {
                var posX = Random.Range(transform.position.x - 0.5f, transform.position.x + 0.5f);
                var posY = Random.Range(transform.position.y - 0.5f, transform.position.y + 0.5f);
                projectile.transform.position = new Vector2(posX, posY);
                projectile.SetActive(true);
                projectile.GetComponent<BossBullet>().target = target;
            }
        }
        sound.PlayOneShot(abilitySound);
    }

    public void EndAbility()
    {
        canUseAbility = true;
        bossMove.canMove = true;
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, activateRadius);
    }
}
