using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnTaco : MonoBehaviour
{
    [SerializeField] private Transform spawnPoint;

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
        GameObject taco = ObjectPooler.instance.GetPooledObject("Taco");
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
