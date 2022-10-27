using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
        public GameObject button;

        private bool _isPlayer;

        private const string PlayerTag = "Player";

        private void Start()
        {
            button.SetActive(false);
        }

        private void Update()
        {
            PickItem();
        }

        private void PickItem()
        {
            if (_isPlayer == true)
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
            if (collision.tag == PlayerTag)
            {
                button.SetActive(true);
                _isPlayer = true;
            }
        }

        private void OnTriggerExit2D(Collider2D collision)
        {
            if (collision.tag == PlayerTag)
            {
                button.SetActive(false);
                _isPlayer = false;
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
