using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.InputSystem;

public delegate void ReadyEventHandler();
public delegate void RemovePlayerEventHandler(GameObject player);
public class ChoseYourChara : MonoBehaviour
{
    [SerializeField] private List<Sprite> sprites = new List<Sprite>();
     private SpriteRenderer spriteRenderer;

    public static event ReadyEventHandler OnReady;
    public static event ReadyEventHandler UnReady;
    public static event RemovePlayerEventHandler OnRemovePlayer;

    [HideInInspector]
    public bool isReady = false;
    [HideInInspector]
    public Sprite currentSprite;
    [HideInInspector]
    public PlayerInput input;


    private void Start()
    {
        //for (int i = 0; i < charas.Count; i++)
        //{
        //    charas[i].SetActive(false);
        //}
        
        //currentChara.SetActive(true);

        input = GetComponent<PlayerInput>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        currentSprite = sprites[0];
        spriteRenderer.sprite = currentSprite;

        GetComponent<Player>().playerNotCarrying = currentSprite;
    }

    public void MoveRight(InputAction.CallbackContext context)
    {
        if (sprites.Count == 0) return;
        if (isReady) return;//on bloque si le joueur est ready 

        if (context.performed)
        {
            if (sprites.IndexOf(currentSprite) == 0)
            {
                //currentChara.SetActive(false);
                currentSprite = sprites[sprites.Count - 1];
                //spriteRenderer.sprite = currentSprite;
                //currentChara.SetActive(true);
            }
            else
            {
                //currentSprite.SetActive(false);
                currentSprite = sprites[sprites.IndexOf(currentSprite) - 1];
                
            }
            spriteRenderer.sprite = currentSprite;
        }
        GetComponent<Player>().playerNotCarrying = currentSprite;
    }

    public void MoveLeft(InputAction.CallbackContext context)
    {
        if (sprites.Count == 0) return;
        if (isReady) return;//on bloque si le joueur est ready 
        if (context.performed)
        {
            if (sprites.IndexOf(currentSprite) == sprites.Count - 1)
            {
                //currentSprite.SetActive(false);
                currentSprite = sprites[0];
                //currentSprite.SetActive(true);
            }
            else
            {
                //currentSprite.SetActive(false);
                currentSprite = sprites[sprites.IndexOf(currentSprite) + 1];
                //currentSprite.SetActive(true);
            }

            spriteRenderer.sprite = currentSprite;
        }

        GetComponent<Player>().playerNotCarrying = currentSprite;
    }

    public void GetReady(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            if (!isReady)//activation de l'etat ready 
            {
                transform.DOScale(2f, 0.5f);
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
                transform.DOScale(1f, 0.5f);
                UnReady?.Invoke();
            }
            else//Remove du player
            {
                OnRemovePlayer?.Invoke(gameObject);
                //Destroy(gameObject);
            }
        }
    }

    public void ChangeActionMapToPlayer(string name)
    {
        input.SwitchCurrentActionMap(name);
        GetComponent<ChoseYourChara>().enabled = false;
    }
}