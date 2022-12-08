using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using Random = UnityEngine.Random;

public delegate void FinishQuestHandler(float value);
public class QuestManager : MonoBehaviour
{
    [SerializeField] private ScriptableRequest _scriptableRequestBase;
    public List<RequestDataBase> activeQuests;
    public List<RequestDataBase> activeDeuilQuests;
    public List<RequestDataBase> activeDigUpQuests;
    public List<RequestDataBase> allQuests = new List<RequestDataBase>();
    public List<RequestDataBase> questFinished; 
    public int numberOfQuests = 10;
    public int numberOfDeuilQuests = 10;
    public int numberOfDigUpQuests = 10;
    public static QuestManager instance;
    
    [Header("Score")]
    public float score;
    public const float failComboToAdd = 2.5f;
    public const float scoreToAdd = 1.5f;
    public const float scoreToRemove = 1.5f;
    private float failCombo = 0f;

    public static FinishQuestHandler onFinishQuest;

    private void Awake()
    {
        instance = this;
        foreach (var quest in _scriptableRequestBase._dataBase)
        {
            allQuests.Add(quest);
        }
    }

    public void UpdateScore(bool isSuccess)
    {
        if (isSuccess)
        {
            score += scoreToAdd;
            onFinishQuest?.Invoke(scoreToAdd);
        }
        else
        {
            failCombo += failComboToAdd;
            score -= scoreToRemove * failCombo;
            onFinishQuest?.Invoke(-(scoreToRemove * failCombo));
        }
        score = Mathf.Clamp(score, 0, int.MaxValue);
    }

    public void AcceptRequest(Request request, RequestDataBase requestInfo)
    {
        activeQuests.Add(requestInfo);
        allQuests.Remove(requestInfo);
    }
    
    public void AcceptRequest(GriefRequest request, RequestDataBase requestInfo)
    {
        activeDeuilQuests.Add(requestInfo);
        questFinished.Remove(requestInfo);
    }

    public RequestDataBase GetRequest(DigRequest request)
    {
        int index = GetRandomNumber(allQuests.Count);
        return allQuests[index];
    }
    public RequestDataBase GetRequest(DigUpRequest request)
    {
        int index = GetRandomNumber(instance.questFinished.Count);
        RequestDataBase databaseChosen = questFinished[index];
        activeDigUpQuests.Add(databaseChosen);
        questFinished.Remove(questFinished[index]);
        return databaseChosen;
    }

    public RequestDataBase GetRequest(GriefRequest request)
    {
        int index = GetRandomNumber(instance.questFinished.Count);
        return questFinished[index];
    }
    
    private int GetRandomNumber(int arrayCount)
    {
        return Random.Range(0, arrayCount);
    }
    
    
    public IEnumerator WaitForNewRequest(float secondsToWait, DigRequest request)
    {
        yield return new WaitForSeconds(secondsToWait);
        request.SetDigRequest();
    }

    public IEnumerator WaitForNewRequest(float secondsToWait, DigUpRequest request)
    {
        yield return new WaitForSeconds(secondsToWait);
        request.SetDigUpRequest();
    }

    public IEnumerator WaitForNewRequest(float secondsToWait, GriefRequest request)
    {
        yield return new WaitForSeconds(secondsToWait);
        request.SetGriefRequest();
    }

}
