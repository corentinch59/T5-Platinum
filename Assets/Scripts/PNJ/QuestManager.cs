using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class QuestManager : MonoBehaviour
{
    [SerializeField] private ScriptableRequest _scriptableRequestBase;
    public List<RequestDataBase> activeQuests;
    public List<RequestDataBase> activeDeuilQuests;
    public List<RequestDataBase> allQuests = new List<RequestDataBase>();
    public List<RequestDataBase> questFinished; 
    public int numberOfQuests = 10;
    public int numberOfDeuilQuests = 10;
    public static QuestManager instance;
    

    public TextMeshProUGUI scoreText;
    public int score;

    private void Awake()
    {
        scoreText = GameObject.FindGameObjectWithTag("Score").GetComponent<TextMeshProUGUI>();
        instance = this;
        foreach (var quest in _scriptableRequestBase._dataBase)
        {
            allQuests.Add(quest);
        }
    }

    public void UpdateScore(int scoreToAdd)
    {
        score += scoreToAdd;
        score = Mathf.Clamp(score, 0, int.MaxValue);
        scoreText.text = score.ToString();
    }

    public IEnumerator WaitForNewRequest(float secondsToWait, Request request)
    {
        yield return new WaitForSeconds(secondsToWait);
        request.SetRequest();
        Debug.Log("LAQUETELA");
    }
    
    public IEnumerator WaitForNewRequest(float secondsToWait, DeuilRequest request)
    {
        yield return new WaitForSeconds(secondsToWait);
        request.SetGriefRequest();
        Debug.Log("LAQUETELA");
    }

}
