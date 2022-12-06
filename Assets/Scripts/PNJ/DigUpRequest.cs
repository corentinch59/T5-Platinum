using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class DigUpRequest : MonoBehaviour
{
    [SerializeField] private RequestDataBase requestInfo; // <- this is the quest the pnj will have
    [SerializeField] private SpriteRenderer requestCorpseImg;      // <- UI Corpse to dig up
    [SerializeField] private List<Sprite> spritesCorpse;      // <- Sprite to choose

    [Header("UI")]
    [SerializeField] private GameObject questParent;      // <- GameObject to show
    [SerializeField] private GameObject questToInstantiate;      // <- GameObject to show
    [SerializeField] private ScriptableTextureData texData;      // <- Texture to show in UI
    private GameObject requestInUI;      // <- GameObject to show

    #region Get/Set
    public RequestDataBase RequestInfo { get { return requestInfo; } set { requestInfo = value; } }
    public SpriteRenderer RequestCorpseImg { get { return requestCorpseImg; } set { requestCorpseImg = value; } }
    public GameObject RequestInUI => requestInUI;
    #endregion

    public void SetDigUpRequest()
    {
        requestInfo = QuestManager.instance.GetRequest(this);
        requestCorpseImg.sprite = spritesCorpse[(int)requestInfo.corps - 1];
        requestCorpseImg.transform.DOScale(1, 0.5f);

        //UI
        requestInUI = Instantiate(questToInstantiate, questParent.transform);
        requestInUI.transform.GetChild(2).GetComponent<RawImage>().texture = texData._TextureData.corpsesTex[(int)requestInfo.corps - 1];
        //requestInUI.SetActive(true);
    }
}
