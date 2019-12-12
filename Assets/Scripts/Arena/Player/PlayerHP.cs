using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerHP : MonoBehaviour, IDamage, IHeal
{
    [SerializeField] private float currentHP = 0;
    [SerializeField] private float maxHP = 2;
    public Image[] healthBars;
    private int i = -1;

    public bool isAlive = true;

	void Start ()
    {
        currentHP = maxHP;
	}

    public void TakeDamage(float amount, DamageType type)
    {
        if (isAlive == true)
        {
            if (GetComponent<PlayerAttack>().isDashing == false || GetComponent<TableChargeAbility>().isCharging == false)
            {
                currentHP -= amount;
                i++;
                healthBars[i].enabled = false;
                GetComponent<SpriteEffect>().DamageEffect();
            }

            if (currentHP <= 0)
            {
                Death();
            }
        }
    }

    private void Death()
    {
        isAlive = false;
        var animator = GetComponent<Animator>();
        animator.SetTrigger("Death");
        animator.SetBool("Idle", false);
        animator.SetBool("Run", false);
        GetComponent<Rigidbody2D>().constraints = RigidbodyConstraints2D.FreezePositionX | RigidbodyConstraints2D.FreezePositionY | RigidbodyConstraints2D.FreezeRotation;
        GetComponent<PlayerAttack>().enabled = false;
        GetComponent<ArenaMovement>().enabled = false;
        GetComponent<TableChargeAbility>().enabled = false;
        GetComponent<DivingElbowAbility>().enabled = false;
        GetComponent<ISoundEffect>().PlayDeathSound();
        this.enabled = false;
        ArenaEvents.PlayerDeath();
    }

    public void Heal(float amount, TacoHeal taco)
    {
        if(currentHP < maxHP)
        {
            currentHP += amount;
            healthBars[i].enabled = true;
            i--;
            GetComponent<SpriteEffect>().HealEffect();
            taco.Healed();
        }
    }
}
