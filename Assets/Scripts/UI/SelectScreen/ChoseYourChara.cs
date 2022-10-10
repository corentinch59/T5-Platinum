using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.InputSystem;

public delegate void ReadyEventHandler();
public class ChoseYourChara : MonoBehaviour
{
    [SerializeField] private List<GameObject> charas = new List<GameObject>();
    //[SerializeField] private Canvas canvas;

    private GameObject currentChara; 

    public static event ReadyEventHandler OnReady;

    public bool isReady = false;

    private Vector3 leftPos;
    private Vector3 rightPos;

    private void Start()
    {
        //transform.SetParent(canvas.transform);

        currentChara = charas[0];
        for (int i = 0; i < charas.Count; i++)
        {
            charas[i].SetActive(false);
        }
        
        currentChara.SetActive(true);

        leftPos = new Vector3(-25f, 0f, 0f);
        leftPos = new Vector3(25f, 0f, 0f);
    }

    private void GetInput()
    {
        
    }

    public void MoveCharas(int move)
    {
        if (isReady) return;

        // 0 == on bouge vers la Droite
        if (move == 0)
        {
            if (charas.IndexOf(currentChara) == 0)
            {
                currentChara.SetActive(false);
                currentChara = charas[charas.Count - 1];
                currentChara.SetActive(true);
            }
            else
            {

                currentChara.SetActive(false);
                currentChara = charas[charas.IndexOf(currentChara) - 1];
                currentChara.SetActive(true);
            }
        }
        // 1 == on bouge vers la gauche
        else if (move == 1)
        {
            if (charas.IndexOf(currentChara) == charas.Count - 1)
            {
                currentChara.SetActive(false);
                currentChara = charas[0];
                currentChara.SetActive(true);
            }
            else
            {
                currentChara.SetActive(false);
                currentChara = charas[charas.IndexOf(currentChara) + 1];
                 currentChara.SetActive(true);
            }
        }
    }

    public void OnReadyButton()
    {
        if (isReady)
        {
            isReady = false;
            currentChara.transform.DOScale(1f, 0.5f);
        }
        else
        {
            currentChara.transform.DOScale(2f, 0.5f);
            isReady = true;
            OnReady?.Invoke();
        }
    }
}