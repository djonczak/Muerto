using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game.Arena.Interactable {
    public class SpawnTaco : MonoBehaviour
    {
        [SerializeField] private Transform[] spawnPoints;

        private const string TacoKey = "Taco";

        private void OnEnable()
        {
            ArenaEvents.OnSpawnTaco += TacoSpawn;
        }

        private void TacoSpawn()
        {
            GameObject taco = Pooler.ObjectPooler.instance.GetPooledObject(TacoKey);
            if (taco != null)
            {
                var randomIndex = Random.Range(0, spawnPoints.Length);
                taco.transform.position = spawnPoints[randomIndex].position;
                taco.SetActive(true);
            }
        }

        private void OnDestroy()
        {
            ArenaEvents.OnSpawnTaco -= TacoSpawn;
        }
    }
}
