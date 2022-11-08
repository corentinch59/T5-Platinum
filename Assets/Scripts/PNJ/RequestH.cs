using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using Random = UnityEngine.Random;
public class RequestH : MonoBehaviour
{
    [Header("Heritage")] 
    [SerializeField] public RequestDataBase requestInfo;
    [SerializeField] protected ScriptableTextureData textureData;
    [SerializeField]  protected TextMeshPro nameText;
    [SerializeField]  protected RawImage corpseImage;
    [SerializeField]  protected GameObject questToInstantiate;
    [SerializeField]  protected GameObject questParent;
    public GameObject quest;
    

    public void AcceptRequest()
    {
        QuestManager.instance.activeQuests.Add(requestInfo);
        QuestManager.instance.allQuests.Remove(requestInfo);
    }
    /*
    public void SetRequest()
    {
        //requestInfo = QuestManager.instance.GetRequest(this);
        UpdateUI();
    }
    */
    
    protected TextureData UpdateUI()
    {
        nameText.text = requestInfo.corpseName;
        TextureData tex = textureData._TextureData;
        corpseImage.texture = tex.corpsesTex[(int)requestInfo.corps];
        return tex;
    }

    protected void GoodByePnj(DigRequest request, RawImage localisationImage, RawImage coffinImage)
    {
        Debug.Log("Ah La moi je pars");
        //StartCoroutine(_pnjInteractable.Walk(false));
        if (QuestManager.instance.allQuests.Count > 0)
        {
            StartCoroutine(QuestManager.instance.WaitForNewRequest(3,request));
        }
        else
        {
            nameText.text = null;
            corpseImage.texture = null;
            localisationImage.texture = null;
            coffinImage.texture = null;
        }
    }
    
    protected void GoodByePnj(GriefRequest request)
    {
        if (QuestManager.instance.questFinished.Count > 0)
        {
            //StartCoroutine(_griefPnjInteractable.Walk(false));
            StartCoroutine(QuestManager.instance.WaitForNewRequest(3, request));
            //griefCoroutine = null;
        }
        else
        {
            nameText.text = null;
            corpseImage.texture = null;
        }
    }
    
}
