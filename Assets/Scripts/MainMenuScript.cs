using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuScript : MonoBehaviour
{
    [SerializeField] private AudioClip buttonclip;
    public void NewGame()
    {
        SceneManager.LoadScene("MainField");
    }
    
    public void QuitGame()
    {
        Application.Quit();
    }

    public void playsoundbutton()
    {
        SoundManager.Instance.PlaySound(buttonclip);
        Time.timeScale = 0.0f;
    }
}
