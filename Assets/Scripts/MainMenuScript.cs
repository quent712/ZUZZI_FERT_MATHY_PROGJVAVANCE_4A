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
        SoundManager.Instance.StopSoundMusic();
    }
    
    public void QuitGame()
    {
        Application.Quit();
    }

    public void playsoundbutton()
    {
        SoundManager.Instance.PlaySoundEffect(buttonclip);
    }
}
