using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class DeuilQuest : MonoBehaviour
{
    public RequestDataBase requestInfos;
    private QuestManager _questManager;
    [SerializeField] private TextMeshProUGUI nameText;
    [SerializeField] private RawImage corpseImage;
    [SerializeField] private Slider questSlider;
    [SerializeField] private float questTime = 5;
    [SerializeField] private Image image;
    private float timer;


    private void Awake()
    {
        _questManager = FindObjectOfType<QuestManager>();
    }

    private void Update()
    {
        if (timer <= questTime)
        {
            timer += Time.deltaTime;
            questSlider.value = Mathf.Lerp(1, 0, timer / questTime);
        }
        else
        {
            questSlider.value = 0;
            StartCoroutine(TimeOutQuest());
        }

        if (Input.GetKeyDown(KeyCode.A))
        {
            StartCoroutine(FinishQuest());
        }
       
        
    }

    public void InitialiseDeuilQuestUI(RequestDataBase request, Texture corpseT)
    {
        requestInfos = request;
        nameText.text = requestInfos.name;
        corpseImage.texture = corpseT;
    }
    
    private IEnumerator FinishQuest()
    {
        image.color = Color.green;
        _questManager.activeDeuilQuests.Remove(requestInfos);
        yield return new WaitForSeconds(1);
        _questManager.UpdateScore(5);

        Destroy(gameObject);
    }

    private IEnumerator TimeOutQuest()
    {
        image.color = Color.red;
        _questManager.activeDeuilQuests.Remove(requestInfos);
        yield return new WaitForSeconds(2);
        Destroy(gameObject);
    }
}