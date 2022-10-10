using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Quest : MonoBehaviour
{
    public RequestDataBase requestInfos;
    public float questTimer;

    [SerializeField] private TextMeshProUGUI nameText;
    [SerializeField] private RawImage corpseImage;
    [SerializeField] private RawImage localisationImage;
    [SerializeField] private RawImage coffinImage;
    [SerializeField] private Slider questSlider;
    [SerializeField] private float questTime = 5;
    [SerializeField] private Image image;
    private float timer;


    private void Update()
    {
        if (timer <= questTime)
        {
            timer += Time.deltaTime;
            questSlider.value = Mathf.Lerp(1, 0, timer / questTime);
            Debug.Log("oui");
        }
        else
        {
            questSlider.value = 0;
            StartCoroutine(TimeOutQuest());
        }
       
       
        
    }

    public void InitialiseQuestUI(RequestDataBase request, Texture corpseT, Texture localisationT,
        Texture coffinT)
    {
        requestInfos = request;
        nameText.text = requestInfos.name;
        corpseImage.texture = corpseT;
        localisationImage.texture = localisationT;
        coffinImage.texture = coffinT;
    }
    
    public void FinishQuest()
    {
        
    }

    private IEnumerator TimeOutQuest()
    {
        image.color = Color.red;
        yield return new WaitForSeconds(2);
        Destroy(gameObject);
    }
}
