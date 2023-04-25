using ML.Combat;
using ML.Player;
using ML.Systems;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ML.Combat
{

    public class Barrel : MonoBehaviour
    {
        [SerializeField] private float explosionRange;
        [SerializeField] private float explosionDamage;
        [SerializeField] private GameObject explosionParticles;

        private void OnEnable()
        {
            GetComponent<Health>().OnDeath += Explode;
            GetComponent<Health>().OnHit += null;
        }

        private void OnDisable()
        {
            GetComponent<Health>().OnDeath -= Explode;
            GetComponent<Health>().OnHit -= null;
        }

        private void Explode()
        {
            Collider[] hits = Physics.OverlapSphere(transform.position, explosionRange);
            foreach (Collider hit in hits)
            {
                if ((hit.GetComponent<Health>() != null) && (hit.GetComponent<Health>() != GetComponent<Health>()))
                {
                    hit.GetComponent<Health>().TakeDamage(explosionDamage);
                }
            }
            Instantiate(explosionParticles, transform.position, Quaternion.identity);
            Destroy(gameObject);
        }

        private void OnDrawGizmos()
        {
            Gizmos.DrawWireSphere(transform.position, explosionRange);
        }
    }

}