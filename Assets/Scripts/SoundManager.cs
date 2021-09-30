using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{

    [SerializeField] private AudioSource MusicSource, EffectSource;
    [SerializeField] private AudioClip Boomeffect;
    
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
    
    
    public void PlaySoundMusic(AudioClip clip)
    {
        MusicSource.PlayOneShot(clip);
    }
    
    public void PlaySoundEffect(AudioClip clip)
    {
        EffectSource.PlayOneShot(clip);
    }

    public void PlaySoundEffectBoom()
    {
        EffectSource.PlayOneShot(Boomeffect);
    }

    public void ToggleEffect()
    {
        EffectSource.mute = !EffectSource.mute;
    }
    
    public void ToggleMusic()
    {
        MusicSource.mute = !MusicSource.mute;
    }

    public void StopSoundMusic()
    {
        MusicSource.Stop();
    }
}
