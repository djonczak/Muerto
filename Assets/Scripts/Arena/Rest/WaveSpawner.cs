using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaveSpawner : MonoBehaviour
{
    [Header("Start enemey")]
    public List<string> enemyTag;
    [Header("Additional enemies to add later")]
    public List<string> additionalEnemiesTag;
    private int enemyNumber;
    private int additionalEnemyIndex = 0;
    [Header("Wave options")]
    public float timeToSpawn;

    private int totalWaves = 15;
    [SerializeField] private int currentWave = 0;

    [SerializeField] private int totalEnemyNumber = 3;
    [SerializeField] private int currentEnemy = 0;

    [SerializeField] private bool spawnEnemies;

    private void OnEnable()
    {
        DeathEvent.OnEnemyDeath += EnemyDied;
    }

    private void Start()
    {
        CheckWave();
    }

    private void Update()
    {
        if (spawnEnemies)
        {
             enemyNumber = Random.Range(0, enemyTag.Capacity);
             SpawnEnemy();
        }
        if (currentEnemy == totalEnemyNumber)
        {
             spawnEnemies = false;
        }
    }

    private void SpawnEnemy()
    {
        GameObject enemy = ObjectPooler.instance.GetPooledObject(enemyTag[enemyNumber]);
        if (enemy != null)
        {
            Vector3 minScreenBounds = Camera.main.ScreenToWorldPoint(new Vector3(3, 3, 0));
            Vector3 maxScreenBounds = Camera.main.ScreenToWorldPoint(new Vector3(Screen.width - 3f, Screen.height - 3f, 0));
            Vector3 randomSpawnPoint = new Vector3(Random.Range(minScreenBounds.x, maxScreenBounds.x), Random.Range(minScreenBounds.y, maxScreenBounds.y), 0);
            enemy.transform.position = randomSpawnPoint;
            currentEnemy++;
            enemy.SetActive(true);
            GameObject summonEffect = ObjectPooler.instance.GetPooledObject("SummonParticle");
            if (summonEffect != null)
            {
                summonEffect.transform.position = enemy.transform.position;
                summonEffect.SetActive(true);
            }
        }
    }

    private void CheckWave()
    {
        if (currentEnemy == 0)
        {
            AddNewEnemy();

            if (currentWave == totalWaves)
            {
                SpawnBoss();
                Debug.Log("Boss fight !");
            }
            else
            {
                StartCoroutine("CooldownToSpawn", timeToSpawn);
                totalEnemyNumber++;
                currentWave++;
                Debug.Log("New wave !");
            }
        }
    }

    private void AddNewEnemy()
    {
        if (currentWave == 3)
        {
            enemyTag.Add(additionalEnemiesTag[additionalEnemyIndex]);
        }
        if (currentWave == 6)
        {
            additionalEnemyIndex++;
            enemyTag.Add(additionalEnemiesTag[additionalEnemyIndex]);
            SpawnHealPack();
        }
        if (currentWave == 9)
        {
            additionalEnemyIndex++;
            enemyTag.Add(additionalEnemiesTag[additionalEnemyIndex]);
            SpawnHealPack();
        }
        if (currentWave == 12)
        {
            SpawnHealPack();
        }
    }

    private void SpawnHealPack()
    {
        ArenaEvents.SpawnTaco();
    }

    public void EnemyDied()
    {
        currentEnemy--;
        CheckWave();
    }

    [ContextMenu("SpawnBoss")]
    private void SpawnBoss()
    {
        ArenaEvents.BossArrive();
    }

    private IEnumerator CooldownToSpawn(float time)
    {
        yield return new WaitForSeconds(time);
        spawnEnemies = true;
    }

    private void OnDestroy()
    {
        DeathEvent.OnEnemyDeath -= EnemyDied;
    }
}
