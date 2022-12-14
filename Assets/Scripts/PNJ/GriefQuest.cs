using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using DG.Tweening;

public class GriefQuest : MonoBehaviour
{
    public RequestDataBase requestInfos;
    private GriefRequest _request;
    [SerializeField] private TextMeshProUGUI nameText;
    [SerializeField] private RawImage corpseImage;
    [SerializeField] private Sprite spriteInGame;
    [SerializeField] private Slider questSlider;
    [SerializeField] private float questTime = 50;
    [SerializeField] private Image image;
    private bool isQuestFinished;
    private float timer = 0;

    public Sprite SpriteInGame => spriteInGame;
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

    public void InitialiseDeuilQuestUI(RequestDataBase requestInformation, Texture headT, GriefRequest request)
    {
        _request = request;
        requestInfos = requestInformation;
        nameText.text = requestInfos.corpseName;
        corpseImage.texture = headT;
    }

    private bool CheckScoreQuest(RequestDataBase.corpseType corpseType)
    {
        if (corpseType == requestInfos.corps)
        {
            _request.GriefPnjInteractable.transform.DOJump(_request.GriefPnjInteractable.transform.position, 3f, 3, 3f);
            return true;
            //image.color = Color.green;
            // add score
            /*switch (stateTimer)
            {
                case StateTimer.EXCELLENT:
                    return 5;
                case StateTimer.MID:
                    return 2;
                case StateTimer.BAD:
                    return 1;
                default:
                    return 10;
            }*/
        }
        else
        {
            _request.GriefPnjInteractable.transform.DOShakePosition(3f, new Vector3(2, 0, 0), 5, 10, false, true, ShakeRandomnessMode.Harmonic);
            return false;
            // remove score
            //image.color = Color.red;
        }
    }

    public IEnumerator FinishGriefQuest(RequestDataBase.corpseType corpseType)
    {
        _request.GoodByeGriefPNJ();
        isQuestFinished = true;
        //_request.griefCoroutine = null;
        QuestManager.instance.UpdateScore(CheckScoreQuest(corpseType));
        QuestManager.instance.activeDeuilQuests.Remove(requestInfos);
        yield return new WaitForSeconds(1);
        Destroy(gameObject);
    }

    private IEnumerator TimeOutQuest()
    {
        _request.GoodByeGriefPNJ();
        //_request.griefCoroutine = null;
        //QuestManager.onFinishQuest?.Invoke(-5f);
        QuestManager.instance.UpdateScore(false);
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