using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using TMPro;

public class SatisfactionManager : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI timerEndMeshPro;

    private Slider satisfactionSlider = default;

    private Coroutine currentCoroutine = null;

    private float lerpDuration = 1.5f;
    private float endTimeDuration = 5f;
    private float elapsedTimeBeforeGO = 0f;

    private float displayTimerFloat = 5f;
    private int displayTimer = 5;

    private bool IsGameOver = false;

    private void Start()
    {
        satisfactionSlider = GetComponent<Slider>();

        QuestManager.onFinishQuest += AddSatisfaction;
        timerEndMeshPro.gameObject.SetActive(false);
    }

    private void Update()
    {
        TestGameOver();
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

    private void TestGameOver()
    {
        if (IsGameOver) return;

        #region 1er itération int
        //if (satisfactionSlider.value == 0)
        //{
        //    textMeshProUGUI.gameObject.SetActive(true);
        //    elapsedTimeBeforeGO += Time.deltaTime;
        //    displayTimer = (int)(endTimeDuration - elapsedTimeBeforeGO) + 1; // On met un +1 car unity arrondi au supérieur les int. ex : 0.1f -> 1 int
        //    textMeshProUGUI.text = displayTimer.ToString();
        //}
        //else
        //{
        //    elapsedTimeBeforeGO = 0f;
        //    textMeshProUGUI.gameObject.SetActive(false);
        //}
        #endregion

        #region 2eme itération float 

        if (satisfactionSlider.value == 0)
        {
            timerEndMeshPro.gameObject.SetActive(true);
            elapsedTimeBeforeGO += Time.deltaTime;
            displayTimerFloat = endTimeDuration - elapsedTimeBeforeGO;
            timerEndMeshPro.text = displayTimerFloat.ToString("0.00");
            timerEndMeshPro.transform.DOShakePosition(5f);
        }
        else
        {
            elapsedTimeBeforeGO = 0f;
            timerEndMeshPro.gameObject.SetActive(false);
        }

        #endregion


        if (displayTimerFloat <= 0)
        {
            IsGameOver = true;
            timerEndMeshPro.text = "0.00";
            Debug.Log("GameOver");
        }
        if (displayTimer <= 0)
        {
            IsGameOver = true;
            Debug.Log("GameOver");
        }
    }

    private void OnDestroy()
    {
        QuestManager.onFinishQuest -= AddSatisfaction;
    }
}
