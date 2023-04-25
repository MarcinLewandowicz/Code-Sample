using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ML.Systems
{
    public class EnemySpawner : MonoBehaviour
    {

        private static EnemySpawner instance;
        private EnemyPool enemyPool;
        [SerializeField] private Transform[] spawnTransforms;


        public static EnemySpawner Instance
        {
            get { return instance; }
        }

        private void Awake()
        {
            if ((instance != null) && (instance != this))
            {
                Destroy(gameObject);
            }
            else
            {
                instance = this;
                DontDestroyOnLoad(gameObject);
                enemyPool = GetComponent<EnemyPool>();
            }
        }

        public void AddEnemy(GameObject enemy) => enemyPool.AddEnemyToPool(enemy);

        private void Start()
        {
            Invoke("SpawnAllEnemies", 1f);
        }

        public void SpawnEnemiesWithDelay(float delayTime)
        {
            StartCoroutine(SpawnEnemyAfterTime(delayTime));
        }

        private IEnumerator SpawnEnemyAfterTime(float delayTime)
        {
            yield return new WaitForSeconds(delayTime);
            SpawnSingleEnemy();
        }

        private void SpawnSingleEnemy()
        {
            GameObject enemy = enemyPool.GetEnemyFromPool();
            enemy.transform.position = GetPositionToSpawn();
            enemy.SetActive(true);
        }

        private void SpawnAllEnemies()
        {
            while (enemyPool.EnemyPoolQueue.Count > 0)
            {
                SpawnSingleEnemy();
            }
        }

        private Vector3 GetPositionToSpawn()
        {
            if (spawnTransforms.Length == 0) { return Vector3.zero; }
            Transform transform = spawnTransforms[Random.Range(0, spawnTransforms.Length)];
            return transform.position;
        }
    }

}