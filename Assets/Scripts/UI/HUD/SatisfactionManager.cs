using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using TMPro;

public class SatisfactionManager : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI timerEndMeshPro;
    [SerializeField] private GameObject gameOverScreen;
    [SerializeField] private GameObject winScreen;

    private Slider satisfactionSlider = default;
    private Coroutine currentCoroutine = null;
    private TextMeshProUGUI winnerRank;
    private TextMeshProUGUI percentageRank;

    private float lerpDuration = 1.5f;
    private float endTimeDuration = 5f;
    private float elapsedTimeBeforeGO = 0f;

    private float displayTimerFloat = 5f;

    private bool IsGameOver = false;

    private void Start()
    {
        satisfactionSlider = GetComponent<Slider>();

        QuestManager.onFinishQuest += AddSatisfaction;
        Sunshine.onTimerEnd += TestWin;
        timerEndMeshPro.gameObject.SetActive(false);
        gameOverScreen.gameObject.SetActive(false);
        winScreen.gameObject.SetActive(false);
        winnerRank = winScreen.gameObject.transform.GetChild(2).GetComponent<TextMeshProUGUI>();
        percentageRank = winScreen.gameObject.transform.GetChild(3).GetComponent<TextMeshProUGUI>();
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

        #region 2eme itération float 

        if (satisfactionSlider.value == 0)
        {
            timerEndMeshPro.gameObject.SetActive(true);
            elapsedTimeBeforeGO += Time.deltaTime;
            displayTimerFloat = endTimeDuration - elapsedTimeBeforeGO;
            timerEndMeshPro.text = displayTimerFloat.ToString("0.");
            timerEndMeshPro.transform.DOShakePosition(5f, 2f);
            //Faire un effet de scale
        }
        else
        {
            elapsedTimeBeforeGO = 0f;
            timerEndMeshPro.gameObject.SetActive(false);
            displayTimerFloat = 5f;
        }
        #endregion

        if (displayTimerFloat <= 0)
        {
            IsGameOver = true;
            timerEndMeshPro.text = "0";
            Debug.Log("GameOver");
            gameOverScreen.SetActive(true);
            UIGameOver.onGameOver?.Invoke();
            timerEndMeshPro.gameObject.SetActive(false);
        }
    }

    private void TestWin()
    {
        if(satisfactionSlider.value >= 20f)
        {
            winScreen.SetActive(true);
            if(satisfactionSlider.value >= 80)
            {
                winnerRank.text = "S";
            }
            else if(satisfactionSlider.value >= 60)
            {
                winnerRank.text = "A";
            }
            else if(satisfactionSlider.value >= 40)
            {
                winnerRank.text = "B";
            }
            else
            {
                winnerRank.text = "C";
            }

            percentageRank.text = $"Satisfaction : {satisfactionSlider.value}%";
        }
        else
        {

        }
    }

    private void OnDestroy()
    {
        QuestManager.onFinishQuest -= AddSatisfaction;
    }
}
