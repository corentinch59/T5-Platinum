using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Corpse : Carryable
{
    [SerializeField] private CorpseData corpseData;
    [SerializeField] private Quest thisQuest;
    private PNJInteractable pnjFrom;

    [SerializeField] private float radius = 10f;
    [SerializeField] private LayerMask localisationsLayer;
    [SerializeField] private LayerMask interactableHole;
    [SerializeField] private Sprite[] tombSprite = new Sprite[5];

    private BigCorpse bigCorpse;
    private SpriteRenderer spriteRenderer;
    private bool isInteractable;

    #region get/set
    public CorpseData CorpseData { get { return corpseData; } set { corpseData = value; } }
    public Quest ThisQuest { get { return thisQuest; } set { thisQuest = value; } }
    public BigCorpse BigCorpse { get { return bigCorpse; } set { bigCorpse = value; } }
    public Sprite[] TombSprite => tombSprite;

    public SpriteRenderer SpriteRenderer => spriteRenderer;
    public PNJInteractable PnjFrom { get { return pnjFrom; } set { pnjFrom = value; } }
    public bool IsInteractable { get { return isInteractable; } set { isInteractable = value; } }
    #endregion

    private void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        if(ThisQuest != null)
        {
            Texture2D texture2D = (Texture2D) thisQuest.corpseImage.texture;
            Sprite sprite = Sprite.Create(texture2D, new Rect(0,0, texture2D.width, texture2D.height), new Vector2(0.5f,0.5f));
            spriteRenderer.sprite = sprite;
        }
        IsInteractable = true;
    }

    public override void Interact(Player player)
    {

        if (thisQuest != null)
        {
            if(thisQuest.requestInfos.siz <= 0)
            {
                if (player.CarriedObj == null)
                {
                    player.CarriedObj = this;
                }
                transform.parent = player.transform;
                transform.localPosition = Vector3.up * 2f;
                player.getPlayerMovement.SpriteRenderer.sprite = player.spriteCarry;
                transform.localScale = new Vector3(1, 1, 1);
            }
        }
        else
        {
            if(corpseData.size <= 0)
            {
                if (player.CarriedObj == null)
                {
                    player.CarriedObj = this;
                }
                transform.parent = player.transform;
                transform.localPosition = Vector3.up * 2f;
                player.getPlayerMovement.SpriteRenderer.sprite = player.spriteCarry;
                transform.localScale = new Vector3(1, 1, 1);
            }
        }
        // Remove tag "Corpse" to avoid the pnj to check if he has to leave cause this is already at its spot
        // And tell another pnj to come give player a quest
        if((gameObject.tag == "Corpse" && QuestManager.instance.allQuests.Count >= 0 && thisQuest != null) || 
            (thisQuest == null && GameManager.Instance.PnjsAlreadyGaveQuest.Count >= 4))
        {
            gameObject.tag = "Untagged";
            //Debug.Log("Previous Quest Giver : " + thisQuest._request._pnjInteractable);
            if(GameManager.Instance.PnjsAlreadyGaveQuest.Count > 1 && pnjFrom != null && thisQuest == null)
            {
                GameManager.Instance.PnjsAlreadyGaveQuest.Remove(pnjFrom);
                GameManager.Instance.NewPNJComingWithQuest(pnjFrom);
                pnjFrom = null;
                return;
            }
            if(pnjFrom != null)
            {
                GameManager.Instance.PnjsAlreadyGaveQuest.Remove(pnjFrom);
                GameManager.Instance.NewPNJComingWithQuest(pnjFrom);
            }
        }
    }

    public override void PutDown(Player player, bool isTimeOut = false)
    {
        gameObject.layer = 7; // <-is Interactable
        isInteractable = true;
        // To avoid dotween problem with player increasing scale of this (as a child)
        if ((thisQuest != null && thisQuest.requestInfos.siz > 0) || (thisQuest == null && corpseData.size > 0))
        {
            transform.localScale = new Vector3(2, 2, 2);
        }
        else
        {
            transform.localScale = new Vector3(1, 1, 1);
        }

        if (player.getPlayerMovement.canMove) // if one player -> put the body anywhere he wants to
        {
            //put down corpse in front of a player -> use rotation but now just t.right
            player.CarriedObj.gameObject.transform.position = new Vector3(player.transform.position.x + player.getPlayerMovement.getOrientation.x * 3f,
                player.transform.position.y, player.transform.position.z + player.getPlayerMovement.getOrientation.y * 3f);
            player.CarriedObj.transform.parent = null;
        }
        else
        {
            player.getPlayerMovement.canMove = true;
        }

        player.getPlayerMovement.SpriteRenderer.sprite = player.playerNotCarrying;

        player.CarriedObj = null;
    }

    public CorpseData UpdateRequestLocalisation(bool locationToo = false)
    {
        CorpseData newLoc = new CorpseData();

        newLoc.name = thisQuest.requestInfos.corpseName;
        newLoc.size = thisQuest.requestInfos.siz;
        newLoc.corpseType = thisQuest.requestInfos.corps;
        newLoc.coffinType = thisQuest.requestInfos.cof;

        if (!locationToo)
        {
            Collider[] corpsInAreas = Physics.OverlapSphere(transform.position, radius, localisationsLayer);

            float min = float.MaxValue;

            for(int i = 0; i < corpsInAreas.Length; ++i)
            {
                Debug.Log("HIT");
                float dist = Vector3.Distance(corpsInAreas[i].gameObject.transform.position, transform.position);
                if (dist < min)
                {
                    min = dist;
                    newLoc.localisation = AddLocalisation(corpsInAreas[i].gameObject.tag);
                }
            }
        }
        return newLoc;
    }

    public RequestDataBase.localisation AddLocalisation(string tag)
    {
        switch (tag)
        {
            case "Water": return RequestDataBase.localisation.WATER;
            case "Tree": return RequestDataBase.localisation.TREE;
            //case "Shrine": return RequestDataBase.localisation.SHRINE;
            //case "Flower": return RequestDataBase.localisation.FLOWER;
            default: return RequestDataBase.localisation.NONE;
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(transform.position, radius);
    }
}