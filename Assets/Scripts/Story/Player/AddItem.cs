using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AddItem : MonoBehaviour
{
    public enum Item
    {
        Flower,
        Mask,
    };

    public Item itemToPick;
    public GameObject button;

    bool isPlayer;

    void Start()
    {
        button.SetActive(false);
    }

    private void Update()
    {
        PickItem();
    }

    private void PickItem()
    {
        if (isPlayer == true)
        {
            if (Input.GetKeyDown(KeyCode.E))
            {
                CollectItem(itemToPick);
                gameObject.SetActive(false);
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            button.SetActive(true);
            isPlayer = true;
        }
    }

    public void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            button.SetActive(false);
            isPlayer = false;
        }
    }

    void CollectItem(Item item)
    {
        if(item == Item.Flower)
        {
            ItemPickEvent.ItemPicked(1);
        }
        if(item == Item.Mask)
        {
            ItemPickEvent.ItemPicked(2);
        }
    }
}
