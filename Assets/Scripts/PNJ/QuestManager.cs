using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class QuestManager : MonoBehaviour
{
    [SerializeField] private ScriptableRequest _scriptableRequestBase;
    public List<RequestDataBase> activeQuests;
    public List<RequestDataBase> allQuests = new List<RequestDataBase>();
    
    public TextMeshProUGUI scoreText;
    public int score;

    private void Awake()
    {
        scoreText = GameObject.FindGameObjectWithTag("Score").GetComponent<TextMeshProUGUI>();
        foreach (var quest in _scriptableRequestBase._dataBase)
        {
            allQuests.Add(quest);
        }
    }

    public void UpdateScore(int scoreToAdd)
    {
        score += scoreToAdd;
        scoreText.text = score.ToString();
    }

}
