using System;
using System.Collections;
using System.Collections.Generic;
using System.Timers;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.UI;

public class TimerGriefManager : MonoBehaviour
{
    
    private Image borderImg;

    private void Start()
    {
        borderImg = GetComponent<Image>();
        Material mat = Instantiate(borderImg.material);
        borderImg.material = mat;
    }

    public void SetBorderBar(float totalTime)
    {
        //borderImg.material.SetFloat("__Cutoff_Lenght", timer);
    }

 
    
    
    
    
}
