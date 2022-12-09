using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Menu : MonoBehaviour
{
    public GameObject ButtonP;
    public GameObject ButtonS;
    public GameObject ButtonQ;
    private Animator anim;
    public AudioSource AudioR;
    public AudioSource AudioL;

    void Start()
    {
        anim = ButtonP.GetComponent<Animator>();
    }

    public void StartSceneByIndex(int p_index)
    {
        SceneManager.LoadScene(p_index);
    }

    public void StartSceneByName(string p_name)
    {
        SceneManager.LoadScene(p_name);
    }

    public void QuitGame()
    {
        Application.Quit();
    }

    public void SwapR()
    {
        if (ButtonP.activeInHierarchy == true)
        {

            //anim.Play("Fade");
            ButtonP.SetActive(false);
            ButtonS.SetActive(true);
            AudioR.Play();
        }
        else if (ButtonS.activeInHierarchy == true)
        {
            //anim.Play("Fade");
            ButtonS.SetActive(false);
            ButtonQ.SetActive(true);
            AudioR.Play();
        }
        else if (ButtonQ.activeInHierarchy == true)
        {
            //anim.Play("Fade");
            ButtonQ.SetActive(false);
            ButtonP.SetActive(true);
            AudioR.Play();
        }

    }

    public void SwapL()
    {
        if (ButtonP.activeInHierarchy == true)
        {

            //anim.Play("Fade");
            ButtonP.SetActive(false);
            ButtonQ.SetActive(true);
            AudioL.Play();
        }
        else if (ButtonQ.activeInHierarchy == true)
        {
            //anim.Play("Fade");
            ButtonQ.SetActive(false);
            ButtonS.SetActive(true);
            AudioL.Play();
        }
        else if (ButtonS.activeInHierarchy == true)
        {
            //anim.Play("Fade");
            ButtonS.SetActive(false);
            ButtonP.SetActive(true);
            AudioL.Play();
        }

    }
}
