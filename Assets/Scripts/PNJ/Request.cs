using System;
using System.Collections;
using System.Collections.Generic;
using JetBrains.Annotations;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;

public class Request : MonoBehaviour
{
    [SerializeField] private ScriptableTextureData _textureData;
    [SerializeField] private RequestDataBase _requestInfos;
    [SerializeField] private TextMeshProUGUI nameText;
    [SerializeField] private RawImage corpseImage;
    [SerializeField] private RawImage localisationImage;
    [SerializeField] private RawImage coffinImage;
    [SerializeField] private GameObject questToInstantiate;
    [SerializeField] private int _numberOfQuests = 10;
    [SerializeField] private QuestManager _questManager;
    private GameObject questParent;
  


    private void Awake()
    {
        _questManager = FindObjectOfType<QuestManager>();
        questParent = GameObject.FindGameObjectWithTag("QuestUI");
    }

    private void Start()
    {
        SetRequest();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.D) && _questManager.allQuests.Count > 0 
                                        && _questManager.activeQuests.Count < _numberOfQuests)
        {
            AcceptRequest();
        }
    }

    private void SetRequest()
    {
        int index = GetRandomNumber(_questManager.allQuests.Count);
        _requestInfos = _questManager.allQuests[index];
        UpdateUI();
    }

    public void AcceptRequest()
    {
        SetQuestInUI();
        _questManager.activeQuests.Add(_requestInfos);
        _questManager.allQuests.Remove(_requestInfos);
        if (_questManager.allQuests.Count > 0)
        {
            SetRequest();
        }
        else
        {
            nameText.text = null;
            corpseImage.texture = null;
            localisationImage.texture = null;
            coffinImage.texture = null;
        }
        
    }

    private int GetRandomNumber(int arrayCount)
    {
        return Random.Range(0, arrayCount);
    }

    private void UpdateUI()
    {
        nameText.text = _requestInfos.name;
        TextureData tex = _textureData._TextureData;
        corpseImage.texture = tex.corpsesTex[(int)_requestInfos.corp];
        localisationImage.texture = tex.localisationTex[(int)_requestInfos.loc];
        coffinImage.texture = tex.coffinTex[(int)_requestInfos.cof];
    }

    public void SetQuestInUI()
    {
        GameObject quest = Instantiate(questToInstantiate, questParent.transform);
        quest.GetComponent<Quest>().InitialiseQuestUI(_requestInfos, corpseImage.texture,
            localisationImage.texture,coffinImage.texture);
    }
}
