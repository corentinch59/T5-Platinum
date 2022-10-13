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
    [Tooltip("The ratio that will determine how much the hole grows in size when digging more")]public float scaleRatioModifier;
    [Tooltip("How quick the hole grows in size when digging more.")] public float scaleAnimDuration;

    private void ModifyHoleSize(int modifier)
    {
        HoleSize += modifier;
        if(HoleSize < 1)
        {
            StartCoroutine(DeathAnim());
        }
        else
            transform.DOScale(transform.localScale + new Vector3(scaleRatioModifier, 0, scaleRatioModifier) * modifier, scaleAnimDuration).SetEase(Ease.InBounce);

    }

    private IEnumerator DeathAnim()
    {
        transform.DOScale(0, scaleAnimDuration).SetEase(Ease.InBounce);
        yield return new WaitForSeconds(scaleAnimDuration);
        Destroy(gameObject);
    }
}
