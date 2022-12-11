using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

public class OutlineManager : MonoBehaviour
{
    [SerializeField] private Image exclamationImage;
    private void Start()
    {
        Material mat = Instantiate(exclamationImage.material);
        exclamationImage.material = mat;
        OutlineIntensity();
    }

    private void OutlineIntensity()
    {
        exclamationImage.material.DOFloat(12, "_Intensity", 8);
    }
}
