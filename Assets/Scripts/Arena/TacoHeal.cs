using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TacoHeal : MonoBehaviour
{
    [SerializeField]
    private float healAmount = 1;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
           collision.GetComponent<IHeal>().Heal(healAmount, this);
        }
    }

    public void Healed()
    {
        gameObject.SetActive(false);
    }
}
