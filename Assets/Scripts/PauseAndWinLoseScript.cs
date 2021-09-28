using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseAndWinLoseScript : MonoBehaviour
{
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
}
