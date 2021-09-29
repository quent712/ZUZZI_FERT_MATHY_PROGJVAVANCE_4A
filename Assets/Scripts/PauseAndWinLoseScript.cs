using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseAndWinLoseScript : MonoBehaviour
{
    [SerializeField] private AudioClip buttonclip;
    public int numberOfPlayer = 2;
    public int mapSizeX = 13;
    public int mapSizeY = 13;
    public void Resume()
    {
        Time.timeScale = 1.0f;
    }
    
    public void Retry()
    {
        SceneManager.LoadScene("MainField");
        Time.timeScale = 1.0f;
        
        
    }

    public void BacktoMain()
    {
        SceneManager.LoadScene("Menu");
        Time.timeScale = 1.0f;
    }
    public void playsoundbutton()
    {
        SoundManager.Instance.PlaySound(buttonclip);
    }
}
