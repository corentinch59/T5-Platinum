using System;
using System.Collections;
using System.Collections.Generic;
using JetBrains.Annotations;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;
public class DeuilRequest : MonoBehaviour
{
    [SerializeField]  private RequestDataBase _requestInfos;
    [SerializeField]  private ScriptableTextureData _textureData;
    [SerializeField]  private TextMeshProUGUI nameText;
    [SerializeField]  private RawImage corpseImage;
    [SerializeField]  private GameObject questToInstantiate;
    [SerializeField]  private QuestManager _questManager;
    [HideInInspector] public GameObject griefQuest;
    private GameObject questParent;
  


    private void Awake()
    {
        _questManager = FindObjectOfType<QuestManager>();
        questParent = GameObject.FindGameObjectWithTag("DeuilQuestUI");
    }
    

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.O))
        {
            SetRequest();
        }
        if (Input.GetKeyDown(KeyCode.P) && _questManager.questFinished.Count > 0 
                                        && _questManager.activeDeuilQuests.Count < _questManager.numberOfDeuilQuests)
        {
            AcceptRequest();
        }

    }

    public void SetRequest()
    {
        int index = GetRandomNumber(_questManager.questFinished.Count);
        _requestInfos = _questManager.questFinished[index];
        UpdateUI();
    }

    public void AcceptRequest()
    {
        SetQuestInUI();
        _questManager.activeDeuilQuests.Add(_requestInfos);
        _questManager.questFinished.Remove(_requestInfos);
        if (_questManager.questFinished.Count > 0)
        {
            SetRequest();
        }
        else
        {
            nameText.text = null;
            corpseImage.texture = null;
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
        corpseImage.texture = tex.corpsesTex[(int)_requestInfos.corps];
    }

    public void SetQuestInUI()
    {
        griefQuest = Instantiate(questToInstantiate, questParent.transform);
        griefQuest.GetComponent<DeuilQuest>().InitialiseDeuilQuestUI(_requestInfos, corpseImage.texture);
    }
}