using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Game.UI;
namespace Game.Interactable 
{
    public class AddItem : MonoBehaviour
    {
        public enum Item
        {
            Flower,
            Mask,
        };

        public Item itemToPick;

        private bool isPlayer;
        private bool pickedItem;
        [SerializeField] private AppearButton appearButton;
        private const string PlayerTag = "Player";

        private void Update()
        {
            if (pickedItem == false)
            {
                PickItem();
            }
        }

        private void PickItem()
        {
            if (isPlayer == true)
            {
                if (Input.GetKeyDown(KeyCode.E))
                {
                    pickedItem = true;
                    CollectItem(itemToPick);
                    appearButton.HideButton();
                }
            }
        }

        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (pickedItem == false)
            {
                if (collision.tag == PlayerTag)
                {
                    appearButton.ShowButton();
                    isPlayer = true;
                }
            }
        }

        private void OnTriggerExit2D(Collider2D collision)
        {
            if (pickedItem == false)
            {
                if (collision.tag == PlayerTag)
                {
                    appearButton.HideButton();
                    isPlayer = false;
                }
            }
        }

        private void CollectItem(Item item)
        {
            if (item == Item.Flower)
            {
                ItemPickEvent.ItemPicked(1);
            }
            if (item == Item.Mask)
            {
                ItemPickEvent.ItemPicked(2);
            }
        }
    }
}
