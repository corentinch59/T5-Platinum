using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public delegate void GameOverHandler();
public class UIGameOver : MonoBehaviour
{
    public static GameOverHandler onGameOver;
    //public static event GameOverHandler onRestart;

    [SerializeField] private string sceneNameToLoad;

    public void Restart()
    {
        SceneManager.LoadScene(sceneNameToLoad);
    }
}
