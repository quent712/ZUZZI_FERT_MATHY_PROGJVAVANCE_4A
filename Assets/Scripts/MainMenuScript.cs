using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuScript : MonoBehaviour
{
    public void NewGame()
    {
        SceneManager.LoadScene("MainField");
    }
    
    public void QuitGame()
    {
        Application.Quit();
    }

}
