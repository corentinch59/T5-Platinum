using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class Hole : MonoBehaviour
{
    private int HoleSize = 1;
    public int SetHoleSize { 
        private get => HoleSize;

        set => ModifyHoleSize(value); 
    }

    [Header("Hole stuff")]
    [Tooltip("The ratio that will determine how much the hole grows in size when digging more")] public float scaleAmountToAdd;
    [Tooltip("How quick the hole grows in size when digging more.")] public float scaleAnimDuration;

    private void ModifyHoleSize(int modifier)
    {
        if(modifier > 0 && HoleSize < 3)
        {
            HoleSize += modifier;
            transform.DOScale(transform.localScale + new Vector3(scaleAmountToAdd, 0, scaleAmountToAdd) * modifier, scaleAnimDuration).SetEase(Ease.InBounce);
        }
        else if (modifier < 0)
        {
            StartCoroutine(BurryAnim());
        }

        //HoleSize += modifier;
        //if(HoleSize < 1)
        //{
        //    StartCoroutine(BurryAnim());
        //}
        //else
        //    transform.DOScale(transform.localScale + new Vector3(scaleRatioModifier, 0, scaleRatioModifier) * modifier, scaleAnimDuration).SetEase(Ease.InBounce);
    }

    private IEnumerator BurryAnim()
    {
        transform.DOScale(0, scaleAnimDuration).SetEase(Ease.InBounce);
        yield return new WaitForSeconds(scaleAnimDuration);
        Destroy(gameObject);
    }
}
