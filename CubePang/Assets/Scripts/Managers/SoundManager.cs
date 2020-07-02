using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    public AudioSource musicSource;
    public AudioSource cubeSource;
    public AudioSource explosionSource;

    public static SoundManager Instance = null;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
    }

    public void PlayCubeSound( )
    {
        if (false == cubeSource.isPlaying)
        {
            cubeSource.pitch = Random.Range(0.95f, 1.05f);
            cubeSource.Play();
        }
    }

    public void PlayExplosionSound()
    {
        explosionSource.pitch = Random.Range(0.95f, 1.05f);
        explosionSource.Play();
    }
}
