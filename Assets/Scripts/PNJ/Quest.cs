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
    public float QuestTime => questTime;
    [SerializeField] private Image image;
    [SerializeField] private RawImage Outline;
    private bool isQuestFinished;
    private float timer;

    private void Start()
    {
        Material mat = Instantiate(Outline.material);
        Outline.material = mat;
    }

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

    private float CheckScoreQuest(CorpseData data)
    {
        if(data.localisation == requestInfos.loc)
        {
            image.color = Color.green;
            // add score
            switch (stateTimer)
            {
                case StateTimer.EXCELLENT:
                    return 8f;
                case StateTimer.MID:
                    return 5f;
                case StateTimer.BAD:
                    return 3f;
                default:
                    return 0f;
            }
        }
        else
        {
            // remove score
            image.color = Color.red;
            return -5f;
        }
    }
    
    public IEnumerator FinishQuest(CorpseData data)
    {
        _request.GoodByePnj();
        QuestManager.instance.UpdateScore(CheckScoreQuest(data));
        isQuestFinished = true;
        QuestManager.instance.questFinished.Add(requestInfos);
        QuestManager.instance.activeQuests.Remove(requestInfos);
        yield return new WaitForSeconds(1);
        Destroy(gameObject);
    }

    private IEnumerator TimeOutQuest()
    {
        _request.GoodByePnj();
        isQuestFinished = true;
        image.color = Color.red;
        QuestManager.instance.activeQuests.Remove(requestInfos);
        QuestManager.instance.questFinished.Add(requestInfos);
        QuestManager.onFinishQuest?.Invoke(-5f);
        yield return new WaitForSeconds(2);
        gameObject.SetActive(false);
        GameManager.Instance.NewPNJComingWithQuest();
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

    public void ActivateOulineUI(int playerID)
    {
        Outline.material.SetInt("_PlayerIDColor", playerID);
        Outline.material.SetFloat("_IsOutline", 1);
    }
    
    public void DesactivateOulineUI()
    {
        Outline.material.SetFloat("_IsOutline", 0);
    }
}
