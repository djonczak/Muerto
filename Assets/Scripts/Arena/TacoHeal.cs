using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TacoHeal : MonoBehaviour
{
    [SerializeField]
    private float healAmount = 1;
    private bool hasCollided;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            hasCollided = true;
            if (hasCollided == true)
            {
                collision.GetComponent<IHeal>().Heal(healAmount, this);
            }
        }
    }

    public void Healed()
    {
        hasCollided = false;
        gameObject.SetActive(false);
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            hasCollided = false;
        }
    }
}
