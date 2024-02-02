using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace Game.Story
{
    public class PlayerData : MonoBehaviour
    {
        public static PlayerData Instance;

        public bool HasFlower;
        public bool HasMask;

        public bool CanPass;

        public Action PickedItem;

        private void OnEnable()
        {
            ItemPickEvent.OnItemPick += CheckWhichItem;
        }

        private void Awake()
        {
            if(Instance != null)
            {
                Destroy(this);
            }

            Instance = this;
        }

        private void ChecksIfGathered()
        {
            if (HasFlower && HasMask)
            {
                CanPass = true;
            }
            else
            {
                CanPass = false;
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
            HasFlower = true;
            PickedItem?.Invoke();
            ChecksIfGathered();
        }

        public void AddMask()
        {
            HasMask = true;
            PickedItem?.Invoke();
            ChecksIfGathered();
        }

        private void OnDestroy()
        {
            ItemPickEvent.OnItemPick -= CheckWhichItem;
        }
    }
}
