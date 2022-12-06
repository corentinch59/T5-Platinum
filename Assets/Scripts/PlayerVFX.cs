using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.VFX;

public class PlayerVFX : MonoBehaviour
{
    private PlayerMovement _playerMovement;
    private PlayerMovement _secondPlayerMovement;
    public SpriteRenderer[] listVfx;
    public SpriteRenderer vfx;
    private Coroutine currentCoroutine;
    public VisualEffect hitImpact;
    private Vector4 actualsPlayersOnOutline;
    


    public void Update()
    {
        if (Input.GetKeyDown(KeyCode.K))
        {
            StartCoroutine(CollisionVFX());
        }
    }


    private void OnControllerColliderHit(ControllerColliderHit hit)
    {
        if(hit.gameObject.CompareTag("Player") && hit.controller != null && currentCoroutine == null)
        {
            _secondPlayerMovement = gameObject.GetComponent<PlayerMovement>();
            Debug.Log("Collision");

            currentCoroutine = StartCoroutine(CollisionVFX());
        }
    }



    public IEnumerator CollisionVFX()
    {
        listVfx[0].gameObject.SetActive(true);
        listVfx[0].material.DOFloat(0.9f, "_Slider", 1);
        yield return new WaitForSeconds(1);
        listVfx[0].material.SetFloat("_Slider", 0);
        listVfx[0].gameObject.SetActive(false);
        currentCoroutine = null;
    }

    public IEnumerator Outline(bool isClose, SpriteRenderer nearestHole, int playerID)
    {

        if (nearestHole.TryGetComponent(out Hole hole))
        {
            if (isClose && !hole.playersID.Contains(playerID))
            {
                hole.AddInteractablePlayers(playerID);
            }
            else
            {
                hole.RemoveInteractablePlayer(playerID);
            } 
        }
        else if (nearestHole.TryGetComponent(out Corpse corpse))
        {
            if (isClose && !corpse.playersID.Contains(playerID))
            {
                corpse.AddInteractablePlayers(playerID);
            }
            else
            {
                corpse.RemoveInteractablePlayer(playerID);
            } 
        }
        else if (nearestHole.TryGetComponent(out GriefPNJInteractable grief))
        {
            if (isClose && !grief.playersID.Contains(playerID))
            {
                grief.AddInteractablePlayers(playerID);
            }
            else
            {
                grief.RemoveInteractablePlayer(playerID);
            } 
        }
        
        
        
        yield break;
    }

}
