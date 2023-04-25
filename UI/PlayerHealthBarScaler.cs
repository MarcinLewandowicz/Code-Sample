using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;
using ML.Combat;

namespace ML.UI
{
    public class PlayerHealthBarScaler : MonoBehaviour
    {
        [SerializeField] private RectTransform healthBarRectTransform;
        private Health health;


        private void Awake()
        {
            health = GameObject.FindWithTag("Player").GetComponent<Health>();
        }

        private void OnEnable()
        {
            health.OnHit += UpdateHealthBar;
            health.OnHeal += UpdateHealthBar;
        }

        private void OnDisable()
        {
            health.OnHit -= UpdateHealthBar;
            health.OnHeal -= UpdateHealthBar;
        }

        private void UpdateHealthBar()
        {
            float healthFraction = health.HealthPoints / health.StartHealthPoints;
            healthBarRectTransform.localScale = new Vector3(healthFraction, 1);
        }

        private void SetHealthBarToMaximum()
        {
            healthBarRectTransform.localScale = new Vector3(1, 1);
        }
    }

}