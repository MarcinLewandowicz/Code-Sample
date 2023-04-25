using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ML.Systems
{
    public class TurnOnRandomEnemy : MonoBehaviour
    {

        [SerializeField] private GameObject[] enemies;

        private void OnEnable()
        {
            enemies[Random.Range(0, enemies.Length)].SetActive(true);
        }

        private void OnDisable()
        {
            foreach (GameObject enemy in enemies)
            {
                enemy.SetActive(false);
            }
        }
    }

}