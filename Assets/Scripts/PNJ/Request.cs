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
    private QuestManager _questManager;
    private GameObject questParent;
  


    private void Awake()
    {
        _questManager = GameObject.FindObjectOfType<QuestManager>();
        questParent = GameObject.FindGameObjectWithTag("QuestUI");
    }

    private void Start()
    {
        SetRequest();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.D))
        {
            SetRequest();
            
        }
    }

    private void SetRequest()
    {
        if (_questManager.allQuests.Count > 0)
        {
            _questManager.activeQuests.Add(_requestInfos);
            _questManager.allQuests.Remove(_requestInfos);
            if (_questManager.allQuests.Count > 0)
            {
                int index = GetRandomNumber(_questManager.allQuests.Count);
                _requestInfos = _questManager.allQuests[index];
            }
            UpdateUI();
            GameObject quest = Instantiate(questToInstantiate, questParent.transform);
            quest.GetComponent<Quest>().InitialiseQuestUI(_requestInfos, corpseImage.texture,
                localisationImage.texture,coffinImage.texture);
        }
        else
        {
            Debug.Log("t'as pris toutes les requetes chacal");
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
}
