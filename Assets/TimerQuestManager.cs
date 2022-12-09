using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public class TimerQuestManager : MonoBehaviour
{
    [SerializeField] private Image borderImg;
    [SerializeField] private GameObject vfxGameObject;
    private int sortPriority = -10; // Or whatever HDRP sortPriority you want to set it to.


    private void Start()
    {
        Renderer renderer = vfxGameObject.GetComponent<Renderer>(); //VFXRenderer inherits from Renderer
        if (renderer != null)
        {
            renderer.sharedMaterial.renderQueue = 3000 + sortPriority;
            renderer.sharedMaterial.SetFloat("_TransparentSortPriority", sortPriority);
        }
    }

    

    public void SetBorderBar(float totalTime)
    {
        Material mat = Instantiate(borderImg.material);
        borderImg.material = mat;
        borderImg.material.DOFloat(-0.1f,"_Cutoff_Lenght", totalTime);
        Debug.Log(borderImg.material);
    }

   
}
