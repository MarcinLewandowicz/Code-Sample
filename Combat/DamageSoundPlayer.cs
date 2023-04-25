using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ML.Combat
{
    [RequireComponent(typeof(AudioSource))]
    public class DamageSoundPlayer : MonoBehaviour
    {
        [SerializeField] private AudioSource audioSource;
        [SerializeField] private AudioClip[] clips;

        private void OnEnable()
        {
            GetComponent<Health>().OnHit += PlaySound;
        }

        private void OnDisable()
        {
            GetComponent<Health>().OnHit -= PlaySound;
        }

        private void PlaySound()
        {
            if (clips.Length == 0) { return; }
            //if (audioSource.isPlaying) { return; }
            audioSource.clip = clips[Random.Range(0, clips.Length)];
            audioSource.Play();
        }
    }

}