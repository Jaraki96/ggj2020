using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelManager : MonoBehaviour
{
    private AudioManager audioManger;

    void Start()
    {
        audioManger = AudioManager.instance;
        if (audioManger == null)
            Debug.LogError("No Audio Manager in this scene.");
        audioManger.PlaySound("Title Music");
    }

    public void LoadLevel(string name)
    {
        Time.timeScale = 1;
        SceneManager.LoadScene(name);
    }

    public void Quit() {
        Application.Quit();
    }
}
