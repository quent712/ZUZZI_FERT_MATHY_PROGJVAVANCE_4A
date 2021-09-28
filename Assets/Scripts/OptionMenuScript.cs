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
        //Debug.Log(AIandSound.Instance.Difficulty);
    }
    public void SelectAIEasy()
    {
        AIandSound.Instance.Difficulty = "Easy";
        //Debug.Log(AIandSound.Instance.Difficulty);
    }
    
    public void playsoundbutton()
    {
        SoundManager.Instance.PlaySound(buttonclip);
    }
}
