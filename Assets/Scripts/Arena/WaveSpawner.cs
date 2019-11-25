using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaveSpawner : MonoBehaviour
{
    [Header("Wave options:")]
    public List<string> enemyTag;
    public List<string> additionalEnemiesTag;
    private int enemyNumber;
    private int additionalEnemyIndex = 0;
    public float timeToSpawn;

    private int totalWaves = 15;
    [SerializeField]
    private int currentWave = 0;

    public int totalEnemyNumber;
    private int currentEnemy;

    private bool spawnEnemies;

    [Header("Boss options")]
    public string bossTag;
    public Transform bossSpawnSpot;

    private void Start()
    {
        CheckWave();
        DeathEvent.OnEnemyDeath += EnemyDied;
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

    void SpawnEnemy()
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
        }
    }

    private void CheckWave()
    {
        if (currentEnemy == 0)
        {
            totalEnemyNumber++;
            currentWave++;
            if(currentWave == 3)
            {
                enemyTag.Add(additionalEnemiesTag[additionalEnemyIndex]);
            }
            if (currentWave == 6)
            {
                additionalEnemyIndex++;
                enemyTag.Add(additionalEnemiesTag[additionalEnemyIndex]);
            }
            if (currentWave == 9)
            {
                additionalEnemyIndex++;
                enemyTag.Add(additionalEnemiesTag[additionalEnemyIndex]);
            }
            if (currentWave == 12)
            {
                additionalEnemyIndex++;
                enemyTag.Add(additionalEnemiesTag[additionalEnemyIndex]);
            }

            if (currentWave == totalWaves)
            {
                SpawnBoss();
                Debug.Log("Boss fight !");
            }
            else
            {
                StartCoroutine("CooldownToSpawn", timeToSpawn);
                Debug.Log("New wave !");
            }
        }
    }

    public void EnemyDied()
    {
        currentEnemy--;
        CheckWave();
    }

    private void SpawnBoss()
    {
        GameObject boss = ObjectPooler.instance.GetPooledObject(bossTag);
        if (boss != null)
        {
            boss.transform.position = bossSpawnSpot.transform.position;
            boss.SetActive(true);
        }
    }

    private IEnumerator CooldownToSpawn(float time)
    {
        yield return new WaitForSeconds(time);
        spawnEnemies = true;
    }
}
