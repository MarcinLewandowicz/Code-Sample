using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ML.UI
{
    public class DamageTextPool : MonoBehaviour
    {
        [SerializeField] private GameObject damageTextPrefab;
        [SerializeField] private int poolStartSize = 5;

        private Queue<GameObject> damageTextQueue = new Queue<GameObject>();

        void Start()
        {
            for (int i = 0; i < poolStartSize; i++)
            {
                GameObject damageText = Instantiate(damageTextPrefab,transform);
                damageText.SetActive(false);
                damageTextQueue.Enqueue(damageText);
            }
        }

        public GameObject GetDamageTextPrefab()
        {
            GameObject damageText = damageTextQueue.Dequeue();
            damageTextQueue.Enqueue(damageText);
            return damageText;
        }
    }

   

}