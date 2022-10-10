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
}
