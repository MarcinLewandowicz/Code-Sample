using ML.Combat;
using ML.Player;
using ML.Systems;
using UnityEngine.AI;
using UnityEngine;
using System;
using ML.UI;

namespace ML.Combat
{
    public class Health : MonoBehaviour, IClickable
    {
        public event Action OnHit;
        public event Action OnDeath;
        public event Action OnHeal;
        [SerializeField] private float healthPoints;
        [SerializeField] private float startHealthPoints;
        private bool isDead = false;
        [SerializeField] private DamageTextPool damageTextPool;
        [SerializeField] private ParticleSystem damageParticles;



        public bool InteractWithRaycast(PlayerController playerController)
        {
            if (Input.GetMouseButtonDown(0))
            {
                if (isDead) { return false; }
                if (!Input.GetMouseButtonDown(0)) { return false; }
                playerController.GetComponent<Fighter>().StartAttack(this);
            }
            return true;
        }

        public float HealthPoints
        {
            get { return healthPoints; }
        }

        public float StartHealthPoints
        {
            get { return startHealthPoints; }
        }

        public void TakeDamage(float damage, Fighter fighter)
        {
            DisplayDamageText(damage);
            if (damageParticles != null)
            {
                damageParticles.Play();
            }            
            if (healthPoints - damage <= 0)
            {
                healthPoints = 0;
                OnDeath();
                Die(fighter);
            }
            else
            {
                healthPoints -= damage;
            }
            if (OnHit != null)
            {
                OnHit();
            }            
        }

        public void TakeDamage(float damage)
        {
            DisplayDamageText(damage);
            if (damageParticles != null)
            {
                damageParticles.Play();
            }
            if (healthPoints - damage <= 0)
            {
                healthPoints = 0;
                OnDeath();
                Die();
            }
            else
            {
                healthPoints -= damage;
            }
            if (OnHit != null)
            {
                OnHit();
            }
        }

        public void MissedAttack()
        {
            if(damageTextPool == null) { return; }
            GameObject damageText = damageTextPool.GetDamageTextPrefab();
            damageText.SetActive(true);
            DamageTextDisplay damageTextDisplay = damageText.GetComponent<DamageTextDisplay>();
            damageTextDisplay.InitializeText("MISS",Color.grey);
        }

        private void DisplayDamageText(float damage)
        {
            if (damageTextPool == null) { return; }
            GameObject damageText = damageTextPool.GetDamageTextPrefab();
            damageText.SetActive(true);
            DamageTextDisplay damageTextDisplay = damageText.GetComponent<DamageTextDisplay>();
            damageTextDisplay.InitializeText(damage.ToString(),Color.red);
        }

        public bool IsDead()
        {
            return isDead;
        }

        private void Die(Fighter fighter)
        {
            fighter.StopAction();
            isDead = true;
            if (GetComponent<Animator>() != null)
            {
                GetComponent<Animator>().SetTrigger("die");
            }            
            if (GetComponent<CapsuleCollider>())
            {
                GetComponent<CapsuleCollider>().enabled = false;
            }
            if (GetComponent<NavMeshAgent>())
            {
                GetComponent<NavMeshAgent>().enabled = false;
            }
        }

        private void Die()
        {
            isDead = true;
            if (GetComponent<Animator>() != null)
            {
                GetComponent<Animator>().SetTrigger("die");
            }
            if (GetComponent<CapsuleCollider>())
            {
                GetComponent<CapsuleCollider>().enabled = false;
            }
            if (GetComponent<NavMeshAgent>())
            {
                GetComponent<NavMeshAgent>().enabled = false;
            }
        }



        public void Resurrect()
        {
            HealToFullHealthPoints();
            isDead = false;
            GetComponent<Animator>().ResetTrigger("die");
            if (GetComponent<CapsuleCollider>())
            {
                GetComponent<CapsuleCollider>().enabled = true;
            }
            if (GetComponent<NavMeshAgent>())
            {
                GetComponent<NavMeshAgent>().enabled = true;
            }
        }

        public void HealToFullHealthPoints()
        {
            healthPoints = startHealthPoints;
        }

        public void Heal(float healAmount)
        {
            if (healthPoints + healAmount >= startHealthPoints)
            {
                healthPoints = startHealthPoints;
            }
            else
            {
                healthPoints += healAmount;
            }
            OnHeal();
        }

        public CursorType GetCursorType()
        {
            return CursorType.Combat;
        }
    }

}