using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Corpse : Carryable
{
    [HideInInspector] public CorpseData corpseData;
    [HideInInspector] public Quest thisQuest;

    [SerializeField] private float radius = 10f;
    [SerializeField] private LayerMask localisationsLayer;
    [SerializeField] private LayerMask interactableHole;
    [SerializeField] private Sprite[] tombSprite = new Sprite[5];
    public Sprite[] TombSprite => tombSprite;

    private SpriteRenderer spriteRenderer;
    public SpriteRenderer SpriteRenderer => spriteRenderer;

    private Player[] players = new Player[2];

    private bool isInteractable;
    public bool IsInteractable { get; set; }

    private void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        IsInteractable = true;
    }
    private void Update()
    {
        if (players[0] != null && players[1] != null)
            transform.LookAt(Camera.main.transform);
    }

    public override void Interact(Player player)
    {
        GameManager.Instance.NewPNJComingWithQuest(thisQuest._request._pnjInteractable);

        // To avoid dotween problem with player increasing scale of this (as a child)
        if(thisQuest.requestInfos.siz > 0)
        {
            transform.localScale = new Vector3(2, 2, 2);
        }
        else
        {
            transform.localScale = new Vector3(1, 1, 1);
        }

        if (player.CarriedObj == null)
        {
            player.CarriedObj = this;
        }
        player.getPlayerMovement.SpriteRenderer.sprite = player.spriteCarry;
        player.CarriedObj.transform.parent = player.transform;
        player.CarriedObj.transform.localPosition = Vector3.up * 2f;
    }

    public override void PutDown(Player player, bool isTimeOut = false)
    {
        // To avoid dotween problem with player increasing scale of this (as a child)
        if (thisQuest.requestInfos.siz > 0)
        {
            transform.localScale = new Vector3(2, 2, 2);
        }
        else
        {
            transform.localScale = new Vector3(1, 1, 1);
        }

        if (player.getPlayerMovement.canMove && players[0] == null && players[1] == null) // if one player -> put the body anywhere he wants to
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

        // Visual Debug 
        player.getPlayerMovement.SpriteRenderer.sprite = player.playerNotCarrying;

        player.CarriedObj = null;
    }

    private CorpseData UpdateLocalisation()
    {
        CorpseData newLoc = new CorpseData();

        newLoc.name = corpseData.name;
        newLoc.size = corpseData.size;
        newLoc.corpseType = corpseData.corpseType;
        newLoc.coffinType = corpseData.coffinType;
        newLoc.specificity = corpseData.specificity;

        Collider[] corpsInAreas = Physics.OverlapSphere(transform.position, radius, localisationsLayer);

        float min = float.MaxValue;

        foreach (Collider col in corpsInAreas)
        {
            float dist = Vector3.Distance(col.gameObject.transform.position, transform.position);
            if (dist < min)
            {
                min = dist;
                newLoc.localisation = AddLocalisation(col.gameObject.tag);
            }
        }
        return newLoc;
    }

    [ContextMenu("Update Localisations")]
    public CorpseData UpdateRequestLocalisation()
    {
        CorpseData newLoc = new CorpseData();

        newLoc.name = thisQuest.requestInfos.corpseName;
        newLoc.size = thisQuest.requestInfos.siz;
        newLoc.corpseType = thisQuest.requestInfos.corps;
        newLoc.coffinType = thisQuest.requestInfos.cof;
        newLoc.specificity = thisQuest.requestInfos.spec;

        Collider[] corpsInAreas = Physics.OverlapSphere(transform.position, radius, localisationsLayer);

        float min = float.MaxValue;

        foreach (Collider col in corpsInAreas)
        {
            float dist = Vector3.Distance(col.gameObject.transform.position, transform.position);
            if (dist < min)
            {
                min = dist;
                newLoc.localisation = AddLocalisation(col.gameObject.tag);
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
            case "Shrine": return RequestDataBase.localisation.SHRINE;
            case "Flower": return RequestDataBase.localisation.FLOWER;
            default: return RequestDataBase.localisation.NONE;
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(transform.position, radius);
    }
}
