using System;
using System.Collections;
using System.Collections.Generic;
using System.Timers;
using JetBrains.Annotations;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;
public class GriefRequest : MonoBehaviour
{
    [SerializeField]  private RequestDataBase _requestInfos;
    [SerializeField]  private ScriptableTextureData _textureData;
    [SerializeField]  private TextMeshProUGUI nameText;
    [SerializeField]  private RawImage corpseImage;
    [SerializeField]  private GameObject questToInstantiate; 
    public GameObject griefQuest;
    [SerializeField] private Image image;
    private GriefPNJInteractable _griefPnjInteractable;
    private GameObject questParent;
    public Coroutine griefCoroutine;
    private float timer;



    private void Awake()
    {
        questParent = GameObject.FindGameObjectWithTag("DeuilQuestUI");
        _griefPnjInteractable = GetComponentInParent<GriefPNJInteractable>();
    }
    

    private void Update()
    {
        /* 
         if (Input.GetKeyDown(KeyCode.O))
         {
             SetGriefRequest();
         }
         if (Input.GetKeyDown(KeyCode.P) && QuestManager.instance.questFinished.Count > 0 
                                         && QuestManager.instance.activeDeuilQuests.Count < QuestManager.instance.numberOfDeuilQuests)
         {
             AcceptRequest();
         }
         */
        Debug.Log(griefCoroutine);
        if (QuestManager.instance.questFinished.Count > 0 && griefCoroutine == null)
        {
            if (timer <= 5) // 15
            {
                timer += Time.deltaTime;
            }
            else
            {
                timer = 5; // 15
                Debug.Log("Cor");
                griefCoroutine = StartCoroutine(QuestManager.instance.WaitForNewRequest(2, this));
                //StartCoroutine(_griefPnjInteractable.Walk(true));
            }
        }
        else
        {
            return;
        }
    }

    public void SetGriefRequest()
    {
        if(QuestManager.instance.questFinished.Count > 0)
        {
            int index = GetRandomNumber(QuestManager.instance.questFinished.Count);
            _requestInfos = QuestManager.instance.questFinished[index];
            UpdateUI();
            StartCoroutine(_griefPnjInteractable.Walk(true));
        }
    }

    public void AcceptRequest()
    {
        if (QuestManager.instance.questFinished.Count > 0 && QuestManager.instance.activeDeuilQuests.Count < QuestManager.instance.numberOfDeuilQuests )
        {
            SetQuestInUI();
            QuestManager.instance.activeDeuilQuests.Add(_requestInfos);
            QuestManager.instance.questFinished.Remove(_requestInfos);
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
        griefQuest.GetComponent<GriefQuest>().InitialiseDeuilQuestUI(_requestInfos, corpseImage.texture, this);
    }

    public void GoodByeGriefPNJ()
    {
        if (QuestManager.instance.questFinished.Count > 0)
        {
            
            //griefCoroutine = null;
        }
        else
        {
            nameText.text = null;
            corpseImage.texture = null;
            griefCoroutine = null;
        }
        StartCoroutine(_griefPnjInteractable.Grieffing());
    }
}