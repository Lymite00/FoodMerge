using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    [Header("Elements")]
    [SerializeField] private AudioSource aSource;
    [SerializeField] private AudioClip impactSound;

    public void PlayImpactSound()
    {
        aSource.PlayOneShot(impactSound);
    }
}
