using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

public class MenuManager : MonoBehaviour
{
    [SerializeField] private string playScene;
    [SerializeField] private GameObject mainUI;
    [SerializeField] private GameObject settingsUI;

    [Header("First Buttons Selected")]
    [SerializeField] private EventSystem eventSystem;
    [SerializeField] private GameObject playBtn;
    [SerializeField] private GameObject backBtn;


    private void Start()
    {
        settingsUI.SetActive(false);
        mainUI.SetActive(true);
        SoundManager.instance.Play("MenuMusic");
    }

    public void Play()
    {
        SoundManager.instance.Stop("MenuMusic");
        SceneManager.LoadScene(playScene);
    }

    public void Settings()
    {
        eventSystem.SetSelectedGameObject(backBtn);
        mainUI.SetActive(false);
        settingsUI.SetActive(true);
    }
    public void Back()
    {
        eventSystem.SetSelectedGameObject(playBtn);
        settingsUI.SetActive(false);
        mainUI.SetActive(true);
    }

    public void Quit()
    {
        Application.Quit();
    }
}
