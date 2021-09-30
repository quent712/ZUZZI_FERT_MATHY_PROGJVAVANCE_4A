using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class OptionMenuScript : MonoBehaviour
{
    
    [SerializeField] private AudioClip buttonclip;
    public void SelectAIHard()
    {
        AIandSound.Instance.Difficulty = "Hard";
    }
    public void SelectAIEasy()
    {
        AIandSound.Instance.Difficulty = "Easy";
    }
    
    public void playsoundbutton()
    {
        SoundManager.Instance.PlaySoundEffect(buttonclip);
    }
}
