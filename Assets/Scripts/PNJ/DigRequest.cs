using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DigRequest : RequestH
{
    [Header("DigRequest")]
    [SerializeField] private RawImage localisationImage;
    [SerializeField] private RawImage coffinImage;
    [HideInInspector] public PNJInteractable _pnjInteractable;

    private void Awake()
    {
        questParent = GameObject.FindGameObjectWithTag("QuestUI");
        _pnjInteractable = GetComponentInParent<PNJInteractable>();
    }

    public void Start()
    {
        SetDigRequest();
        QuestManager.instance.allQuests.Remove(requestInfo);
        //QuestManager.instance.activeQuests.Add(requestInfo);
    }

    public void SetDigRequest()
    {
        requestInfo = QuestManager.instance.GetRequest(this);
        //StartCoroutine(_pnjInteractable.Walk(true));
        //UpdateDigUI();
    }
    
    public void AcceptDigRequest()
    {
        AcceptRequest();
        SetQuestInUI();
    }
    
    /*
    private void UpdateDigUI()
    {
        TextureData tex = UpdateUI();
        localisationImage.texture = tex.localisationTex[(int)requestInfo.loc];
        coffinImage.texture = tex.coffinTex[(int)requestInfo.cof];
    }
    */
    
    public void SetQuestInUI()
    {
        TextureData tex = UpdateUI();
        quest = Instantiate(questToInstantiate, questParent.transform);
        /*Debug.Log("loc" + (int)requestInfo.loc);
        Debug.Log("corpse" + ((int)requestInfo.corps - 1));*/
        quest.GetComponent<Quest>().InitialiseQuestUI(requestInfo, tex.corpsesTex[(int)requestInfo.corps - 1],
            tex.localisationTex[(int)requestInfo.loc],tex.coffinTex[(int)requestInfo.cof], this);
    }

    public void GoodByePnj()
    {
        //StartCoroutine(_pnjInteractable.Walk(false));
        if (QuestManager.instance.allQuests.Count > 0)
        {
            _pnjInteractable.CorpseCreated.GetComponent<Corpse>().ThisQuest = null;
            StartCoroutine(QuestManager.instance.WaitForNewRequest(2, this));
        }
    }
}
