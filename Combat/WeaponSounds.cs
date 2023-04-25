using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class WeaponSounds : MonoBehaviour
{
    [SerializeField] private AudioSource audioSource;
    [SerializeField] private AudioClip[] clips;


    private void Awake()
    {
        audioSource = GetComponent<AudioSource>();
    }
    public void PlaySound()
    {
        audioSource.clip = clips[Random.Range(0, clips.Length)];
        audioSource.Play();
    }
}
