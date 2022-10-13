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
    [HideInInspector] public GameObject griefQuest;
    private GameObject questParent;
  


    private void Awake()
    {
        questParent = GameObject.FindGameObjectWithTag("DeuilQuestUI");
    }
    

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.O))
        {
            SetRequest();
        }
        if (Input.GetKeyDown(KeyCode.P) && QuestManager.instance.questFinished.Count > 0 
                                        && QuestManager.instance.activeDeuilQuests.Count < QuestManager.instance.numberOfDeuilQuests)
        {
            AcceptRequest();
        }

    }

    public void SetRequest()
    {
        int index = GetRandomNumber(QuestManager.instance.questFinished.Count);
        _requestInfos = QuestManager.instance.questFinished[index];
        UpdateUI();
    }

    public void AcceptRequest()
    {
        SetQuestInUI();
        QuestManager.instance.activeDeuilQuests.Add(_requestInfos);
        QuestManager.instance.questFinished.Remove(_requestInfos);
        if (QuestManager.instance.questFinished.Count > 0)
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