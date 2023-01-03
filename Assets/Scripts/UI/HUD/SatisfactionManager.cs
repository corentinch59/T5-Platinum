using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using TMPro;
using UnityEngine.EventSystems;

public class SatisfactionManager : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI timerEndMeshPro;
    [SerializeField] private EventSystem eventSystem;
    [SerializeField] private GameObject finish;
    [SerializeField] private float finishDuration;
    [SerializeField] private GameObject questUI;
    [SerializeField] private GameObject gameOverScreen;
    [SerializeField] private GameObject winScreen;
    [SerializeField] private GameObject retryButton;
    [SerializeField] private ScreenShot _screenShot;
    [SerializeField] private List<Sprite> ranks = new List<Sprite>();
    [SerializeField] private Image sliderFill;

    private Slider satisfactionSlider = default;
    private Coroutine currentCoroutine = null;
    private Image winnerRank;

    private float lerpDuration = 1.5f;
    private float endTimeDuration = 5f;
    private float elapsedTimeBeforeGO = 0f;

    private float displayTimerFloat = 5f;

    private bool IsGameOver = false;

    private void Start()
    {
        finish.SetActive(false);
        satisfactionSlider = GetComponent<Slider>();
        QuestManager.onFinishQuest += AddSatisfaction;
        Sunshine.onTimerEnd += TestWin;
        //Sunshine.onTimerEnd += () => { eventSystem.firstSelectedGameObject = retryWinButton; };
        //UIGameOver.onGameOver += () => { eventSystem.firstSelectedGameObject = retryGameOverButton; };
        timerEndMeshPro.gameObject.SetActive(false);
        gameOverScreen.gameObject.SetActive(false);
        winScreen.gameObject.SetActive(false);
        retryButton.gameObject.SetActive(false);
        winnerRank = winScreen.gameObject.transform.GetChild(2).GetComponent<Image>();

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

        if (value < 0f)
        {
            sliderFill.color = Color.red;
        }

        while (elapsedTime < lerpDuration)
        {
            elapsedTime += Time.deltaTime;
            satisfactionSlider.value = Mathf.Lerp(svalue, endValue, elapsedTime / lerpDuration);
            yield return null;
        }

        currentCoroutine = null;
        sliderFill.color = Color.white;
    }

    private IEnumerator TimeOutFinish(bool gameOver)
    {
        yield return new WaitForSeconds(finishDuration);

        finish.SetActive(true);

        //finish.GetComponent<RectTransform>().DORotate(new Vector3(0f, 0f, 13f), 0.5f);

        yield return new WaitForSeconds(finishDuration);

        finish.SetActive(false);

        if (gameOver)
        {
            timerEndMeshPro.text = "0";
            questUI.SetActive(false);
            gameOverScreen.SetActive(true);
            UIGameOver.onGameOver?.Invoke();
            StartCoroutine(_screenShot.TakeScreenShot(false));
            timerEndMeshPro.gameObject.SetActive(false);
            retryButton.gameObject.SetActive(true);
            eventSystem.SetSelectedGameObject(retryButton);
        }
        else
        {
            if (satisfactionSlider.value >= 20f)
            {
                questUI.SetActive(false);
                winScreen.SetActive(true);
                StartCoroutine(_screenShot.TakeScreenShot(true));
                if (satisfactionSlider.value >= 80)
                {
                    winnerRank.sprite = ranks[0]; //S
                }
                else if (satisfactionSlider.value >= 60)
                {
                    winnerRank.sprite = ranks[1];//A
                }
                else if (satisfactionSlider.value >= 40)
                {
                    winnerRank.sprite = ranks[2];//B
                }
                else
                {
                    winnerRank.sprite = ranks[3];//C
                }

                retryButton.gameObject.SetActive(true);
                eventSystem.SetSelectedGameObject(retryButton);
            }
            else
            {

            }
        }
    }

    private void TestGameOver()
    {
        if (IsGameOver)
        {
            return;
        }

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
            StartCoroutine(TimeOutFinish(true));
        }
    }

    private void TestWin()
    {
        StartCoroutine(TimeOutFinish(false));
        
    }

    private void OnDestroy()
    {
        QuestManager.onFinishQuest -= AddSatisfaction;
        Sunshine.onTimerEnd -= TestWin;
    }
}
