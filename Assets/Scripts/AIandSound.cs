using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIandSound : MonoBehaviour
{

    public string Difficulty="Easy";
    
    public static AIandSound Instance;
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
}
