using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class DigUpRequest : MonoBehaviour
{
    [SerializeField] private RequestDataBase requestInfo; // <- this is the quest the pnj will have
    [SerializeField] private SpriteRenderer requestCorpseImg;      // <- UI Corpse to dig up
    [SerializeField] private List<Sprite> spritesCorpse;      // <- Sprite to choose

    #region Get/Set
    public RequestDataBase RequestInfo { get { return requestInfo; } set { requestInfo = value; } }
    public SpriteRenderer RequestCorpseImg { get { return requestCorpseImg; } set { requestCorpseImg = value; } }
    #endregion

    public void SetDigUpRequest()
    {
        requestInfo = QuestManager.instance.GetRequest(this);
        Debug.Log((int)requestInfo.corps);
        requestCorpseImg.sprite = spritesCorpse[(int)requestInfo.corps - 1];
        requestCorpseImg.transform.DOScale(1, 0.5f);

        /*switch (requestInfo.corps)
        {
            case RequestDataBase.corpseType.CORGNON1:
                requestCorpseImg.sprite = spritesCorpse[0];
                requestCorpseImg.transform.DOScale(1, 0.5f);
                return;
            case RequestDataBase.corpseType.CORGNON2:
                requestCorpseImg.sprite = spritesCorpse[1];
                requestCorpseImg.transform.DOScale(1, 0.5f);

                return;
            case RequestDataBase.corpseType.CORGNON3:
                requestCorpseImg.sprite = spritesCorpse[2];
                requestCorpseImg.transform.DOScale(1, 0.5f);

                return;
            case RequestDataBase.corpseType.CORGNON4:
                requestCorpseImg.sprite = spritesCorpse[3];
                requestCorpseImg.transform.DOScale(1, 0.5f);

                return;
            case RequestDataBase.corpseType.LEZARD1:
                requestCorpseImg.sprite = spritesCorpse[4];
                requestCorpseImg.transform.DOScale(1, 0.5f);

                return;
            case RequestDataBase.corpseType.LEZARD2:
                requestCorpseImg.sprite = spritesCorpse[5];
                requestCorpseImg.transform.DOScale(1, 0.5f);

                return;
            case RequestDataBase.corpseType.LEZARD3:
                requestCorpseImg.sprite = spritesCorpse[6];
                requestCorpseImg.transform.DOScale(1, 0.5f);

                return;
            case RequestDataBase.corpseType.LEZARD4:
                requestCorpseImg.sprite = spritesCorpse[7];
                requestCorpseImg.transform.DOScale(1, 0.5f);

                return;
            default: return;
        }*/
    }
}
