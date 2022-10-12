using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class Hole : MonoBehaviour
{
    private int HoleSize;
    public int SetHoleSize { 
        private get => HoleSize;

        set => ModifyHoleSize(value); 
    }

    [Header("Hole stuff")]
    public float scaleRatioModifier;
    public float scaleAnimDuration;

    private void ModifyHoleSize(int modifier)
    {
        HoleSize += modifier;
        transform.DOScale(transform.localScale * scaleRatioModifier * modifier, scaleAnimDuration).SetEase(Ease.OutBounce);
    }
}
