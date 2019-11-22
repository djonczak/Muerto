using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerHP : MonoBehaviour, IDamage
{
    [SerializeField]
    private float minHP = 0;
    [SerializeField]
    private float maxHP = 2;
    public Image[] healthBars;
    private int i = -1;

    PlayerAttack player;
    [SerializeField]
    private bool isAlive = true;

	void Start ()
    {
        player = GetComponent<PlayerAttack>();
        minHP = maxHP;
	}

    public void TakeDamage(float amount, DamageType type)
    {
        if (player.isDashing == false && isAlive == true)
        {
            minHP -= amount;
            i++;
            healthBars[i].enabled = false;
            GetComponent<DamageEffect>().ShowEffect();
        }

        if(minHP <= 0)
        {
            var animator = GetComponent<Animator>();
            animator.SetTrigger("Death");
            animator.SetBool("Idle", false);
            animator.SetBool("Run", false);
            GetComponent<Collider2D>().enabled = false;
            player.enabled = false;
            GetComponent<ArenaMovement>().enabled = false;
            isAlive = false;
            this.enabled = false;
        }
    }
}
