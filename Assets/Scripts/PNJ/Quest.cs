using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Quest : MonoBehaviour
{
    public RequestDataBase requestInfos;
    private QuestManager _questManager;
    [SerializeField] private TextMeshProUGUI nameText;
    [SerializeField] private RawImage corpseImage;
    [SerializeField] private RawImage localisationImage;
    [SerializeField] private RawImage coffinImage;
    [SerializeField] private Slider questSlider;
    [SerializeField] private float questTime = 5;
    [SerializeField] private Image image;
    private bool isQuestFinished;
    private float timer;


    private void Awake()
    {
        _questManager = FindObjectOfType<QuestManager>();
    }

    private void Update()
    {
        if (!isQuestFinished)
        {
            if (timer <= questTime)
            {
                timer += Time.deltaTime;
                questSlider.value = Mathf.Lerp(1, 0, timer / questTime);
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

    public void InitialiseQuestUI(RequestDataBase request, Texture corpseT, Texture localisationT,
        Texture coffinT)
    {
        requestInfos = request;
        nameText.text = requestInfos.name;
        corpseImage.texture = corpseT;
        localisationImage.texture = localisationT;
        coffinImage.texture = coffinT;
    }

    public int CheckScoreQuest(CorpseData data)
    {
        return 1;
    }
    
    private IEnumerator FinishQuest(CorpseData data)
    {
        _questManager.UpdateScore(CheckScoreQuest(data));
        isQuestFinished = true;
        image.color = Color.green;
        yield return new WaitForSeconds(1);
        _questManager.questFinished.Add(requestInfos);
        _questManager.activeQuests.Remove(requestInfos);
        Destroy(gameObject);
    }

    private IEnumerator TimeOutQuest()
    {
        isQuestFinished = true;
        image.color = Color.red;
        yield return new WaitForSeconds(2);
        _questManager.questFinished.Add(requestInfos);
        _questManager.activeQuests.Remove(requestInfos);
        Destroy(gameObject);
    }
}
