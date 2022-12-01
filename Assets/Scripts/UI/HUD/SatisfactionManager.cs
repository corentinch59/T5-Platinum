using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class SatisfactionManager : MonoBehaviour
{
    private Slider satisfactionSlider = default;

    private float lerpDuration = 1.5f;

    private Coroutine currentCoroutine = null;


    private void Start()
    {
        satisfactionSlider = GetComponent<Slider>();

       QuestManager.onFinishQuest += AddSatisfaction;
    }

    private void AddSatisfaction(float value)
    {
        if (currentCoroutine != null)
        {
            StopCoroutine(JuicyCoroutine(value));
            currentCoroutine = StartCoroutine(JuicyCoroutine(value));
        }
        else currentCoroutine = StartCoroutine(JuicyCoroutine(value));

    }

    private IEnumerator JuicyCoroutine(float value)
    {
        float elapsedTime = 0f;
        float svalue = satisfactionSlider.value;
        float endValue = svalue + value;
        
        while (elapsedTime < lerpDuration)
        {
            elapsedTime += Time.deltaTime;
            satisfactionSlider.value = Mathf.Lerp(svalue, endValue, elapsedTime / lerpDuration);
            yield return null;
        }

        currentCoroutine = null;
    }

    private void OnDestroy()
    {
        QuestManager.onFinishQuest -= AddSatisfaction;
    }
}
