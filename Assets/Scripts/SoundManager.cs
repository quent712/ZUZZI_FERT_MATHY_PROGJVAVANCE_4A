using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{

    [SerializeField] private AudioSource MusicSource, EffectSource;
    
    public static SoundManager Instance;
    void Awake ()   
    {
        if (Instance == null)
        {
            DontDestroyOnLoad(gameObject);
            Instance = this;
        }
        else if (Instance != this)
        {
            Destroy (gameObject);
        }

    }
    
    public void PlaySound(AudioClip clip)
    {
        EffectSource.PlayOneShot(clip);
    }
}
