﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game.Arena.Interactable {
    public class SpawnTaco : MonoBehaviour
    {
        [SerializeField] private Transform spawnPoint;

        private const string TacoKey = "Taco";

        private void OnEnable()
        {
            ArenaEvents.OnSpawnTaco += TacoSpawn;
        }

        private void Awake()
        {
            spawnPoint = gameObject.transform.GetChild(0);
        }

        private void TacoSpawn()
        {
            GameObject taco = Pooler.ObjectPooler.instance.GetPooledObject(TacoKey);
            if (taco != null)
            {
                taco.transform.position = spawnPoint.position;
                taco.SetActive(true);
            }
        }

        private void OnDestroy()
        {
            ArenaEvents.OnSpawnTaco -= TacoSpawn;
        }
    }
}
