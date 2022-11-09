using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class PlayerVFX : MonoBehaviour
{
    private PlayerMovement _playerMovement;
    private PlayerMovement _secondPlayerMovement;
    public SpriteRenderer[] listVfx;
    public SpriteRenderer vfx;
    private Coroutine currentCoroutine;

    
    private void Awake()
    {
        //vfx.gameObject.SetActive(false);
    }


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

    public IEnumerator Outline(bool isClose, SpriteRenderer nearestHole)
    {
        if(isClose)
            nearestHole.material.SetFloat("_IsOuline", 1);
        else
            nearestHole.material.SetFloat("_IsOuline", 0);
        yield break;
    }
}