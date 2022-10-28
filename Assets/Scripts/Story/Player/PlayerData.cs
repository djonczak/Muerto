using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace Game.Story
{
    public class PlayerData : MonoBehaviour
    {

        public bool hasFlower;
        public bool hasMask;

        public bool canPass;

        public Action PickedItem;

        private void OnEnable()
        {
            ItemPickEvent.OnItemPick += CheckWhichItem;
        }

        private void ChecksIfGathered()
        {
            if (hasFlower && hasMask)
            {
                canPass = true;
            }
            else
            {
                canPass = false;
            }
        }

        private void CheckWhichItem(int i)
        {
            if (i == 1)
            {
                AddFlowers();
            }
            if (i == 2)
            {
                AddMask();
            }
        }

        public void AddFlowers()
        {
            hasFlower = true;
            PickedItem?.Invoke();
            ChecksIfGathered();
        }

        public void AddMask()
        {
            hasMask = true;
            PickedItem?.Invoke();
            ChecksIfGathered();
        }

        private void OnDestroy()
        {
            ItemPickEvent.OnItemPick -= CheckWhichItem;
        }
    }
}
