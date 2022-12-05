using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public delegate void GameOverHandler();
public class UIGameOver : MonoBehaviour
{
    public static event GameOverHandler onGameOver;
    void Start()
    {
        Debug.Log("STOP");
        onGameOver?.Invoke();
    }
}
