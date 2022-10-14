using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class DeuilQuest : MonoBehaviour
{
    public RequestDataBase requestInfos;
    private DeuilRequest _request;
    [SerializeField] private TextMeshProUGUI nameText;
    [SerializeField] private RawImage corpseImage;
    [SerializeField] private Slider questSlider;
    [SerializeField] private float questTime = 50;
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
                //StartCoroutine(FinishGriefQuest());
            }
        }

        
    }

    public void InitialiseDeuilQuestUI(RequestDataBase requestInformation, Texture corpseT, DeuilRequest request)
    {
        _request = request;
        requestInfos = requestInformation;
        nameText.text = requestInfos.name;
        corpseImage.texture = corpseT;
    }

    private int CheckScoreQuest(string name, RequestDataBase.localisation loc)
    {
        if (name == requestInfos.name && loc == requestInfos.loc)
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

    public IEnumerator FinishGriefQuest(string name, RequestDataBase.localisation loc)
    {
        _request.GoodByeGriefPNJ();
        _request.griefCoroutine = null;
        QuestManager.instance.UpdateScore(CheckScoreQuest(name, loc));
        QuestManager.instance.activeDeuilQuests.Remove(requestInfos);
        yield return new WaitForSeconds(1);
        Destroy(gameObject);
    }

    private IEnumerator TimeOutQuest()
    {
        _request.GoodByeGriefPNJ();
        _request.griefCoroutine = null;
        image.color = Color.red;
        QuestManager.instance.activeDeuilQuests.Remove(requestInfos);
        yield return new WaitForSeconds(2);
        Destroy(gameObject);
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