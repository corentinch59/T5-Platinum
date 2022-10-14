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
    [SerializeField] public RequestDataBase _requestInfos;
    [SerializeField] private TextMeshPro nameText;
    [SerializeField] private RawImage corpseImage;
    [SerializeField] private RawImage localisationImage;
    [SerializeField] private RawImage coffinImage;
    [SerializeField] private GameObject questToInstantiate;
    [SerializeField] public GameObject quest;
    private GameObject questParent;
    private PNJInteractable _pnjInteractable;
  


    private void Awake()
    {
        questParent = GameObject.FindGameObjectWithTag("QuestUI");
        _pnjInteractable = GetComponentInParent<PNJInteractable>();
    }

    private void Start()
    {
        SetRequest();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.D) && QuestManager.instance.allQuests.Count > 0 
                                        && QuestManager.instance.activeQuests.Count < QuestManager.instance.numberOfQuests)
        {
            AcceptRequest();
        }
    }

    public void SetRequest()
    {
        int index = GetRandomNumber(QuestManager.instance.allQuests.Count);
        _requestInfos = QuestManager.instance.allQuests[index];
        StartCoroutine(_pnjInteractable.Walk(true));
        UpdateUI();
    }

    public void AcceptRequest()
    {
        SetQuestInUI();
        QuestManager.instance.activeQuests.Add(_requestInfos);
        QuestManager.instance.allQuests.Remove(_requestInfos);
        
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
        localisationImage.texture = tex.localisationTex[(int)_requestInfos.loc];
        coffinImage.texture = tex.coffinTex[(int)_requestInfos.cof];
    }

    public void SetQuestInUI()
    {
        quest = Instantiate(questToInstantiate, questParent.transform);
        quest.GetComponent<Quest>().InitialiseQuestUI(_requestInfos, corpseImage.texture,
            localisationImage.texture,coffinImage.texture, this);
    }

    public void GoodByePnj()
    {
        Debug.Log("Ah La moi je pars");
        StartCoroutine(_pnjInteractable.Walk(false));
        if (QuestManager.instance.allQuests.Count > 0)
        {
            StartCoroutine(QuestManager.instance.WaitForNewRequest(3,this));
        }
        else
        {
            nameText.text = null;
            corpseImage.texture = null;
            localisationImage.texture = null;
            coffinImage.texture = null;
        }

    }
}
