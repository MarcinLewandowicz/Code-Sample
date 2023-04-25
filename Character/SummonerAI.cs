using ML.Combat;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ML.Combat
{
    public class SummonerAI : Enemy
    {
        [SerializeField] private GameObject enemyToSpawn;
        [SerializeField] private Transform spawnTransform;
        [SerializeField] private float spawnInterval;
        private float lastSpawnTime = Mathf.NegativeInfinity;
        private Health spawnedEnemyHealth = null;
        private bool playerSpotted = false;

        void Update()
        {
            if (enemyHealth.IsDead()) { return; }
            if (enemyFOV.IsTargetVisible() || playerSpotted)
            {
                AttackBehaviour();
            }
            else
            {
                SetExclamationMarkStatus(false);
                Move(transform.position);
            }
        }

        protected override void OnEnable()
        {
            base.OnEnable();
            enemyHealth.OnHit += OnDamageTaken;
        }

        protected override void OnDisable()
        {
            base.OnDisable();
            enemyHealth.OnHit -= OnDamageTaken;
        }

        protected override void RespawnBehaviour()
        {
            base.RespawnBehaviour();
            playerSpotted = false;
        }

        private void AttackBehaviour()
        {
            SetExclamationMarkStatus(true);
            playerSpotted = true;
            Attack();
            if (!IsEnemySpawned() && IsTimeToSpawnEnemy())
            {
                SpawnEnemy();
            }
        }

        private bool IsTimeToSpawnEnemy()
        {
            return (Time.time - lastSpawnTime > spawnInterval ? true : false);
        }

        private bool IsEnemySpawned()
        {
            if (spawnedEnemyHealth == null) { return false; }
            return !spawnedEnemyHealth.IsDead();
        }

        private void SpawnEnemy()
        {
            lastSpawnTime = Time.time;
            GameObject spawnedEnemy = Instantiate(enemyToSpawn, spawnTransform.position, Quaternion.identity);
            spawnedEnemyHealth = spawnedEnemy.GetComponent<Health>();
        }

        private void OnDamageTaken()
        {
            playerSpotted = true;
        }
    }

}