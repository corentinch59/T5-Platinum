using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class ShockWave : MonoBehaviour
{
    private PlayerMovement _playerMovement;
    private PlayerMovement _secondPlayerMovement;
    public SpriteRenderer vfx;
    private Coroutine currentCoroutine;

    
    private void Awake()
    {
        vfx.gameObject.SetActive(false);
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
        vfx.gameObject.SetActive(true);
        vfx.material.DOFloat(0.9f, "_Slider", 1);
        yield return new WaitForSeconds(1);
        vfx.material.SetFloat("_Slider", 0);
        vfx.gameObject.SetActive(false);
        currentCoroutine = null;
    }
}
