using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuestManager : MonoBehaviour
{
    [SerializeField] private ScriptableRequest _scriptableRequestBase;
    public List<RequestDataBase> activeQuests;
    public List<RequestDataBase> allQuests = new List<RequestDataBase>();

    private void Awake()
    {
        foreach (var quest in _scriptableRequestBase._dataBase)
        {
            allQuests.Add(quest);
        }
    }

}
