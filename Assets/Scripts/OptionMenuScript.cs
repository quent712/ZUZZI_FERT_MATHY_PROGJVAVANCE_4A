using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class OptionMenuScript : MonoBehaviour
{
    // Start is called before the first frame update
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
}
