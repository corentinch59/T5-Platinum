using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.InputSystem;

public class Hole : MonoBehaviour, IInteractable
{
    private bool isAlreadyDug = false;
    private int HoleSize = 1;
    public int SetHoleSize { 
        private get => HoleSize;

        set => ModifyHoleSize(value); 
    }

    [Header("Hole stuff")]
    private GameObject holePrefab;
    [Tooltip("The ratio that will determine how much the hole grows in size when digging more")] public float scaleAmountToAdd = 0.2f;
    [Tooltip("How quick the hole grows in size when digging more.")] public float scaleAnimDuration = 1;

    public Hole(GameObject prefab)
    {
        holePrefab = prefab;
    }

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

    /*private void Dig(bool isDug, int modifier)
    {
        if (isDug)
        {
            SetHoleSize = modifier;
        }
        else
        {
            Instantiate(holePrefab, transform.position*//*transform.position + new Vector3(orientationVect.x, HeightOfHole, orientationVect.y)*//*, Quaternion.identity);
            isAlreadyDug = true;
        }
    }*/

    public void Interact(PlayerTest player)
    {
        SetHoleSize = 1;
    }

    public void Burry()
    {
        StartCoroutine(BurryAnim());
    }

    public void SetVibrations(PlayerInput playerInput, float frequencyLeftHaptic, float frequencyRightHaptic)
    {
    }

    public IEnumerator SetVibrationsCoroutine(PlayerInput playerInput, float frequencyLeftHaptic, float frequencyRightHaptic)
    {
        yield break;
    }
}
