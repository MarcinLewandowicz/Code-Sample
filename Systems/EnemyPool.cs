using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ML.Systems
{
    public class EnemyPool : MonoBehaviour
    {
        [SerializeField] private GameObject enemyPrefab;
        [SerializeField] private int poolStartSize = 5;

        private Queue<GameObject> enemyPoolQueue = new Queue<GameObject>();

        void Start()
        {
            for (int i = 0; i < poolStartSize; i++)
            {
                GameObject enemy = Instantiate(enemyPrefab);
                enemy.SetActive(false);
                enemyPoolQueue.Enqueue(enemy);
            }
        }

        public GameObject GetEnemyFromPool()
        {
            if (enemyPoolQueue.Count == 0) { return null; }
            GameObject pooledEnemy = enemyPoolQueue.Dequeue();
            return pooledEnemy;
        }

        public void AddEnemyToPool(GameObject enemy)
        {
            enemyPoolQueue.Enqueue(enemy);
        }

        public Queue<GameObject> EnemyPoolQueue
        {
            get { return enemyPoolQueue; }
        }

    }
}
