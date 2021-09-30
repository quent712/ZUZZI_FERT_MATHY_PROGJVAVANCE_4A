using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ToggleAudio : MonoBehaviour
{
    [SerializeField] private bool Togglesound, Togglemusic;

    public void Togglezique()
    {
        if (Togglemusic) SoundManager.Instance.ToggleMusic();
        
    }

    public void Toggleeffet()
    {
        if (Togglesound) SoundManager.Instance.ToggleEffect();
    }
}
