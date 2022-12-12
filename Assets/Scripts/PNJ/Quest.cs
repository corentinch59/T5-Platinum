using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public delegate void QuestDelegate();

public class Quest : MonoBehaviour
{
    public RequestDataBase requestInfos;
    [HideInInspector] public DigRequest _request;
    [SerializeField] public RawImage corpseImage;
    [SerializeField] private TextMeshProUGUI nameText;
    [SerializeField] private RawImage localisationImage;
    [SerializeField] private RawImage coffinImage;
    [SerializeField] private Image image;
    [SerializeField] private RawImage Outline;
    [SerializeField] private Slider questSlider;
    [SerializeField] private float questTime = 5;
    [SerializeField] private TimerQuestManager timerQuestManager;
    public float QuestTime => questTime;
    public QuestDelegate onQuestDestroy;
    private bool isQuestFinished;
    [HideInInspector] public float timer;
    private bool isBarShaderStarted;

    private Sprite corpseSprite;
    public Sprite CorpseSprite => corpseSprite;

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
        }

        if (!isBarShaderStarted)
        {
            timerQuestManager.SetBorderBar(questTime);
            isBarShaderStarted = true;
        }
        
    }

    public void InitialiseQuestUI(RequestDataBase requestInformation, Texture headT, Texture2D corpseT, Texture localisationT,
         DigRequest request)
    {
        _request = request; 
        requestInfos = requestInformation;
        nameText.text = requestInfos.corpseName;
        corpseImage.texture = headT;
        corpseSprite = Sprite.Create(corpseT, new Rect(0, 0, corpseT.width, corpseT.height), new Vector2(0.5f, 0.5f));
        localisationImage.texture = localisationT;
        
    }

    private bool CheckScoreQuest(CorpseData data)
    {
        if(data.localisation == requestInfos.loc)
        {
            ParticlePlayManager.instance.PlayAtPosition("Satisfaction");
            image.color = Color.green;
            return true;
            // add score
           /* switch (stateTimer)
            {
                case StateTimer.EXCELLENT:
                    return 8f;
                case StateTimer.MID:
                    return 5f;
                case StateTimer.BAD:
                    return 3f;
                default:
                    return 0f;
            }*/
            
        }
        else
        {
            // remove score
            image.color = Color.red;
            return false;
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
        onQuestDestroy?.Invoke();
        Destroy(gameObject);
    }

    private IEnumerator TimeOutQuest()
    {
        _request.GoodByePnj();
        isQuestFinished = true;
        image.color = Color.red;
        QuestManager.instance.activeQuests.Remove(requestInfos);
        QuestManager.instance.questFinished.Add(requestInfos);
        //QuestManager.onFinishQuest?.Invoke(-5f);
        QuestManager.instance.UpdateScore(false);
        yield return new WaitForSeconds(1);
        gameObject.SetActive(false);
        //GameManager.Instance.NewPNJComingWithQuest();
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
