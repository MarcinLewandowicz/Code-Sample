using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ML.Combat
{
    public class WeaponFXSpawner : MonoBehaviour
    {
        [SerializeField] private ParticleSystem particleSystem;

        public void SpawnParticles()
        {
            particleSystem.Play();
        }
    }
}