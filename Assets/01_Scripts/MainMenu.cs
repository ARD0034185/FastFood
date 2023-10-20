using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    public AudioClip startSound;
    public AudioClip exitSound;

    public void SceneStandar()
    {
        AudioManager.instance.PlaySong(startSound);
        SceneManager.LoadScene(1);
    }

    public void SceneExit()
    {
        AudioManager.instance.PlaySong(exitSound);
        Application.Quit();
    }
}
