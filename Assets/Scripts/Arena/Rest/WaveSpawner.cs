using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace Game.Arena.Spawner 
{
    public class WaveSpawner : MonoBehaviour
    {
        [Header("Start enemey")]
        public List<string> enemyTag;
        [Header("Additional enemies to add later")]
        public List<string> additionalEnemiesTag;

        [Header("Wave options")]
        [SerializeField] private float timeToSpawn = 4;
        [SerializeField] private int currentWave = 0;
        [SerializeField] private int totalEnemyNumber = 3;
        [SerializeField] private int currentEnemy = 0;
        [SerializeField] private bool spawnEnemies;
        [SerializeField] private int additionalEnemyIndex = 0;

        private int totalWaves = 15;

        private const string SummonParticleKey = "SummonParticle";

        private void OnEnable()
        {
            DeathEvent.OnEnemyDeath += EnemyDied;
        }

        private void Start()
        {
            CameraManager.CameraFade.Instance.FadedScreen += CheckWave;
        }

        private void Update()
        {
            if (spawnEnemies)
            {
                SpawnEnemy();
            }
            if (currentEnemy == totalEnemyNumber)
            {
                spawnEnemies = false;
            }
        }

        private void SpawnEnemy()
        {
            var enemyNumber = Random.Range(0, enemyTag.Capacity);
            Debug.Log(enemyNumber);
            GameObject enemy = Pooler.ObjectPooler.instance.GetPooledObject(enemyTag[enemyNumber]);
            if (enemy != null)
            {
                Vector3 spawnPoint = GetRandomSpawnPoint();
                enemy.transform.position = spawnPoint;
                currentEnemy++;
                enemy.SetActive(true);
                SummonEffect(enemy);
            }
        }

        private static Vector3 GetRandomSpawnPoint()
        {
            Vector3 minScreenBounds = Camera.main.ScreenToWorldPoint(new Vector3(3, 3, 0));
            Vector3 maxScreenBounds = Camera.main.ScreenToWorldPoint(new Vector3(Screen.width - 3f, Screen.height - 3f, 0));
            Vector3 randomSpawnPoint = new Vector3(Random.Range(minScreenBounds.x, maxScreenBounds.x), Random.Range(minScreenBounds.y, maxScreenBounds.y), 0);
            return randomSpawnPoint;
        }

        private static void SummonEffect(GameObject enemy)
        {
            GameObject summonEffect = Pooler.ObjectPooler.instance.GetPooledObject(SummonParticleKey);
            if (summonEffect != null)
            {
                summonEffect.transform.position = enemy.transform.position;
                summonEffect.SetActive(true);
            }
        }

        private void CheckWave()
        {
            CameraManager.CameraFade.Instance.FadedScreen -= CheckWave;

            if (currentEnemy == 0)
            {
                AddNewEnemy();

                if (currentWave == totalWaves)
                {
                    SpawnBoss();
                }
                else
                {
                    StartCoroutine(CooldownToSpawn(timeToSpawn));
                    totalEnemyNumber++;
                    currentWave++;
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

        private void SpawnBoss()
        {
            ArenaEvents.BossArrive();
        }

        public void EnemyDied()
        {
            currentEnemy--;
            CheckWave();
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
}
