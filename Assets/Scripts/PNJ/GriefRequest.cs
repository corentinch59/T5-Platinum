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
    public GriefPNJInteractable GriefPnjInteractable => _griefPnjInteractable;
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
        if (QuestManager.instance.questFinished.Count > 0 && griefCoroutine == null)
        {
            if (timer <= 5) // 15
            {
                timer += Time.deltaTime;
            }
            else
            {
                timer = 5; // 15
                _griefPnjInteractable.gameObject.layer = 0;
                griefCoroutine = StartCoroutine(QuestManager.instance.WaitForNewRequest(2, this));
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
        /*
        nameText.text = _requestInfos.corpseName;
        TextureData tex = _textureData._TextureData;
        corpseImage.texture = tex.corpsesTex[(int)_requestInfos.corps];
        */
    }

    public void SetQuestInUI()
    {
        TextureData tex = _textureData._TextureData;
        griefQuest = Instantiate(questToInstantiate, questParent.transform);
        griefQuest.GetComponent<GriefQuest>().InitialiseDeuilQuestUI(_requestInfos, tex.corpsesTex[(int)_requestInfos.corps], this);
    }

    public void GoodByeGriefPNJ()
    {
        if (QuestManager.instance.questFinished.Count > 0)
        {
            
            //griefCoroutine = null;
        }
        else
        {
            //nameText.text = null;
            corpseImage.texture = null;
            griefCoroutine = null;
        }

        if(_griefPnjInteractable.transform.parent != null && _griefPnjInteractable.transform.parent.TryGetComponent(out Player player))
        {
            //_griefPnjInteractable.PutDown(player, true);

        }
        _griefPnjInteractable.gameObject.layer = 0;
        StartCoroutine(_griefPnjInteractable.Grieffing());
    }
}