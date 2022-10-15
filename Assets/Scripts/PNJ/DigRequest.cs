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
    private PNJInteractable _pnjInteractable;

    private void Awake()
    {
        questParent = GameObject.FindGameObjectWithTag("QuestUI");
        _pnjInteractable = GetComponentInParent<PNJInteractable>();
    }

    public void Start()
    {
        SetDigRequest();
    }

    public void SetDigRequest()
    {
        requestInfo = QuestManager.instance.GetRequest(this);
        UpdateDigUI();
    }
    
    private void AcceptDigRequest()
    {
        base.AcceptRequest();
    }

    private void UpdateDigUI()
    {
        TextureData tex = base.UpdateUI();
        localisationImage.texture = tex.localisationTex[(int)requestInfo.loc];
        coffinImage.texture = tex.coffinTex[(int)requestInfo.cof];
    }
    
    public void SetQuestInUI()
    {
        quest = Instantiate(questToInstantiate, questParent.transform);
        quest.GetComponent<Quest>().InitialiseQuestUI(requestInfo, corpseImage.texture,
            localisationImage.texture,coffinImage.texture, this);
    }

    public void GoodByePnj()
    {
        
        
    }
}
