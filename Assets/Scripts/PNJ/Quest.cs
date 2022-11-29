using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Quest : MonoBehaviour
{
    public RequestDataBase requestInfos;
    [HideInInspector] public DigRequest _request;
    [SerializeField] private TextMeshProUGUI nameText;
    [SerializeField] public RawImage corpseImage;
    [SerializeField] private RawImage localisationImage;
    [SerializeField] private RawImage coffinImage;
    [SerializeField] private Slider questSlider;
    [SerializeField] private float questTime = 5;
    [SerializeField] private Image image;
    private bool isQuestFinished;
    private float timer;

    public enum StateTimer
    {
        EXCELLENT,
        MID,
        BAD,
    }

    public StateTimer stateTimer;
    
    
    private void Update()
    {
        if (!isQuestFinished)
        {
            if (timer <= questTime)
            {
                timer += Time.deltaTime;
                questSlider.value = Mathf.Lerp(1, 0, timer / questTime);
                CheckTimer(timer);
            }
            else
            {
                questSlider.value = 0;
                StartCoroutine(TimeOutQuest());
            }

            if (Input.GetKeyDown(KeyCode.A))
            {
                //StartCoroutine(FinishQuest());
            }
        }
    }

    public void InitialiseQuestUI(RequestDataBase requestInformation, Texture corpseT, Texture localisationT,
        Texture coffinT, DigRequest request)
    {
        _request = request; 
        requestInfos = requestInformation;
        nameText.text = requestInfos.corpseName;
        corpseImage.texture = corpseT;
        localisationImage.texture = localisationT;
        coffinImage.texture = coffinT;
    }

    private int CheckScoreQuest(CorpseData data)
    {
        if(data.localisation == requestInfos.loc)
        {
            image.color = Color.green;
            // add score
            switch (stateTimer)
            {
                case StateTimer.EXCELLENT:
                    return 5;
                case StateTimer.MID:
                    return 2;
                case StateTimer.BAD:
                    return 1;
                default:
                    return 10;
            }
            
        }
        else
        {
            // remove score
            image.color = Color.red;
            return -5;
        }
    }
    
    public IEnumerator FinishQuest(CorpseData data)
    {
        _request.GoodByePnj();
        QuestManager.instance.UpdateScore(CheckScoreQuest(data));
        isQuestFinished = true;
        yield return new WaitForSeconds(1);
        QuestManager.instance.questFinished.Add(requestInfos);
        QuestManager.instance.activeQuests.Remove(requestInfos);
        Destroy(gameObject);
    }

    private IEnumerator TimeOutQuest()
    {
        _request.GoodByePnj();
        isQuestFinished = true;
        image.color = Color.red;
        yield return new WaitForSeconds(2);
        QuestManager.instance.questFinished.Add(requestInfos);
        QuestManager.instance.activeQuests.Remove(requestInfos);
        gameObject.SetActive(false);
    }

    private void CheckTimer(float timer)
    {
        if (timer >= 4f)
        {
            stateTimer = StateTimer.BAD;
        }
        else if (timer >= 2.5f && timer < 4f)
        {
            stateTimer = StateTimer.MID;
        }
        else
        {
            stateTimer = StateTimer.EXCELLENT;
        }
    }
}
