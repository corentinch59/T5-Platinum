using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class Timer : MonoBehaviour
{
    public TextMeshProUGUI text;
    [SerializeField] private float timeLeft;
    private int zero = 0;
    public string endScene;


    void Update()
    {
        if (timeLeft > zero)
        {
            timeLeft -= Time.deltaTime;
            text.text = ((int)timeLeft).ToString();
        }

        if (timeLeft <= 0)
        {
            timeLeft = 0;
            SceneManager.LoadScene(endScene);
        }

        if (Input.GetKeyDown(KeyCode.N))
        {
            timeLeft = 0;
            SceneManager.LoadScene(endScene);
        }
    }
}
