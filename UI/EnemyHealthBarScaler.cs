using ML.Combat;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace ML.UI
{
    public class EnemyHealthBarScaler : MonoBehaviour
    {
        [SerializeField] private RectTransform healthBarRectTransform;
        [SerializeField] private GameObject healthBar;
        [SerializeField] private Health health;

        private void OnEnable()
        {
            health.OnHit += UpdateHealthBar;
            //ResetHealthBarState();
        }

        private void OnDisable()
        {
            health.OnHit -= UpdateHealthBar;
        }


        private void ResetHealthBarState()
        {
            float healthFraction = health.HealthPoints / health.StartHealthPoints;
            healthBarRectTransform.localScale = new Vector3(healthFraction, 1);
            healthBar.SetActive(false);
        }

        private void UpdateHealthBar()
        {
            if (!healthBar.activeSelf)
            {
                healthBar.SetActive(true);
            }
            float healthFraction = health.HealthPoints / health.StartHealthPoints;
            if (healthFraction <= 0)
            {
                healthBar.SetActive(false);
            }
            healthBarRectTransform.localScale = new Vector3(healthFraction, 1);
        }
    }

}