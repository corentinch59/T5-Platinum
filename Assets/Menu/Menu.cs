using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Rendering;
using UnityEngine.SceneManagement;

public class Menu : MonoBehaviour
{
    public GameObject ButtonP;
    public GameObject ButtonS;
    public GameObject ButtonQ;
    public GameObject backBtn;
    public GameObject mainUI;
    public GameObject settingUI;
    public EventSystem eventSystem;
    private Animator anim;

    void Start()
    {
        SoundManager.instance.Play("MenuMusic");
        eventSystem.SetSelectedGameObject(ButtonP);
        settingUI.SetActive(false);
        anim = ButtonP.GetComponent<Animator>();
    }

    public void StartSceneByIndex(int p_index)
    {
        SoundManager.instance.Stop("MenuMusic");
        SceneManager.LoadScene(p_index);
    }

    public void StartSceneByName(string p_name)
    {
        SoundManager.instance.Stop("MenuMusic");
        SceneManager.LoadScene(p_name);
    }

    public void DisplaySettings()
    {
        eventSystem.SetSelectedGameObject(backBtn);
        mainUI.SetActive(false);
        settingUI.SetActive(true);
    }
    public void DisplayMain()
    {
        settingUI.SetActive(false);
        mainUI.SetActive(true);
        GameObject btnToSelect = new GameObject();
        if (ButtonP.activeInHierarchy == true)
        {
            //anim.Play("Fade");
            ButtonQ.SetActive(false);
            ButtonS.SetActive(false);
            btnToSelect = ButtonP;
        }
        else if (ButtonS.activeInHierarchy == true)
        {
            //anim.Play("Fade");
            ButtonP.SetActive(false);
            ButtonQ.SetActive(false);
            btnToSelect = ButtonS;

        }
        else if (ButtonQ.activeInHierarchy == true)
        {
            //anim.Play("Fade");
            ButtonP.SetActive(false);
            ButtonS.SetActive(false);
            btnToSelect = ButtonQ;

        }
        eventSystem.SetSelectedGameObject(btnToSelect);
    }

    public void QuitGame()
    {
        Application.Quit();
    }

    public void SwapR()
    {
        SoundManager.instance.Play("MenuSwapR");
        if (ButtonP.activeInHierarchy == true)
        {
            //anim.Play("Fade");
            ButtonP.SetActive(false);
            ButtonS.SetActive(true);
        }
        else if (ButtonS.activeInHierarchy == true)
        {
            //anim.Play("Fade");
            ButtonS.SetActive(false);
            ButtonQ.SetActive(true);
        }
        else if (ButtonQ.activeInHierarchy == true)
        {
            //anim.Play("Fade");
            ButtonQ.SetActive(false);
            ButtonP.SetActive(true);
        }

    }

    public void SwapL()
    {
        SoundManager.instance.Play("MenuSwapL");

        if (ButtonP.activeInHierarchy == true)
        {
            //anim.Play("Fade");
            ButtonP.SetActive(false);
            ButtonQ.SetActive(true);
        }
        else if (ButtonQ.activeInHierarchy == true)
        {
            //anim.Play("Fade");
            ButtonQ.SetActive(false);
            ButtonS.SetActive(true);
        }
        else if (ButtonS.activeInHierarchy == true)
        {
            //anim.Play("Fade");
            ButtonS.SetActive(false);
            ButtonP.SetActive(true);
        }

    }
}
