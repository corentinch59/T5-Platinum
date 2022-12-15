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

    public GameObject FondMenu;

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
        SoundManager.instance.Play("MenuSubmit");
        SoundManager.instance.Stop("MenuMusic");
        SceneManager.LoadScene(p_name);
    }

    public void DisplaySettings()
    {
        SoundManager.instance.Play("MenuSubmit");
        eventSystem.SetSelectedGameObject(backBtn);
        mainUI.SetActive(false);
        FondMenu.SetActive(true);
        settingUI.SetActive(true);
    }
    public void DisplayMain()
    {
        SoundManager.instance.Play("MenuBack");
        settingUI.SetActive(false);
        mainUI.SetActive(true);
        FondMenu.SetActive(false);
        GameObject btnToSelect = new GameObject();
        btnToSelect = ButtonP;
        eventSystem.SetSelectedGameObject(btnToSelect); 
    }

    public void QuitGame()
    {
        SoundManager.instance.Play("MenuSubmit");
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
