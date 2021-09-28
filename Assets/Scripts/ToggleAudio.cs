using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ToggleAudio : MonoBehaviour
{
    [SerializeField] private bool Togglesound, Togglemusic;

    public void Toggle()
    {
        if (Togglemusic) SoundManager.Instance.ToggleMusic();
        if (Togglesound) SoundManager.Instance.ToggleEffect();
    }
}
