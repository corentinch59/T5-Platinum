using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.InputSystem;

public delegate void ReadyEventHandler();
public delegate void RemovePlayerEventHandler(GameObject player);
public class ChoseYourChara : MonoBehaviour
{
    [SerializeField] private List<GameObject> charas = new List<GameObject>();

    private GameObject currentChara; 

    public static event ReadyEventHandler OnReady;
    public static event ReadyEventHandler UnReady;
    public static event RemovePlayerEventHandler OnRemovePlayer;

    public bool isReady = false;

    private void Start()
    {
        currentChara = charas[0];
        for (int i = 0; i < charas.Count; i++)
        {
            charas[i].SetActive(false);
        }
        
        currentChara.SetActive(true);
    }

    public void MoveRight(InputAction.CallbackContext context)
    {
        if (isReady) return;//on bloque si le joueur est ready 

        if (context.performed)
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
    }

    public void MoveLeft(InputAction.CallbackContext context)
    {
        if (isReady) return;//on bloque si le joueur est ready 
        if (context.performed)
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

    public void GetReady(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            if (!isReady)//activation de l'etat ready 
            {
                currentChara.transform.DOScale(2f, 0.5f);
                isReady = true;
                OnReady?.Invoke();
            }
        }
    }

    public void RemovePlayer(InputAction.CallbackContext ctx){

        if (ctx.performed)
        {
            if (isReady)//Cancel l'etat ready 
            {
                isReady = false;
                currentChara.transform.DOScale(1f, 0.5f);
                UnReady?.Invoke();
            }
            else//Remove du player
            {
                OnRemovePlayer?.Invoke(gameObject);
            }
        }
    }
}