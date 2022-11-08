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
    
    private Player[] players = new Player[2];

    private SpriteRenderer spriteRenderer;
    private Hole holeToBurry;

    private bool isInHole;
    private bool canBurry;

    private void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
    }
    private void Update()
    {
        if (players[0] != null && players[1] != null)
            transform.LookAt(Camera.main.transform);

        Collider[] hits = Physics.OverlapSphere(transform.position, radius, interactableHole);
        if (hits.Length > 0)
        {
            foreach (Collider hit in hits)
            {
                if (hit.gameObject.TryGetComponent(out Hole hole))
                {
                    canBurry = true;
                    holeToBurry = hole;
                }
            }
        }
        else
        {
            canBurry = false;
        }
        //Debug.DrawRay(transform.position, Vector3.down * 20, Color.black);
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

        if (player.carriedObj == null)
        {
            player.carriedObj = this;
            player.interactableObj = null;
            player.isCarrying = true;
        }
        player.getPlayerMovement.SpriteRenderer.sprite = player.spriteCarry;
        player.carriedObj.transform.parent = player.transform;
        player.carriedObj.transform.localPosition = Vector3.up * 2f;
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
            if(canBurry)
            {
                player.carriedObj.transform.parent = null;

                // update CorpseData
                corpseData = UpdateLocalisation();
                isInHole = true;
                StartCoroutine(BurryingCorpse(holeToBurry));
            }
            else
            {
                //put down corpse in front of a player -> use rotation but now just t.right
                player.carriedObj.gameObject.transform.position = new Vector3(player.transform.position.x + player.getPlayerMovement.getOrientation.x * 3f,
                    player.transform.position.y, player.transform.position.z + player.getPlayerMovement.getOrientation.y * 3f);
                player.carriedObj.transform.parent = null;

                if (isInHole)
                {
                    // update CorpseData
                    corpseData = UpdateLocalisation();
                }
            }
        }
        else
        {
            player.getPlayerMovement.canMove = true;
        }

        player.isCarrying = false;

        // Visual Debug 
        player.getPlayerMovement.SpriteRenderer.sprite = player.playerNotCarrying;

        if (thisQuest != null)
        {
            // check if the corpse correspond to the quest -> finish quest
            if (isInHole)
            {
                corpseData = UpdateRequestLocalisation();
            }
            StartCoroutine(thisQuest.FinishQuest(corpseData));
        }

        player.carriedObj = null;
    }

    private IEnumerator BurryingCorpse(Hole hole)
    {
        Vector3 holepos = hole.transform.position;
        // burry corpse
        hole.Burry();
        gameObject.layer = 0;

        transform.localScale = new Vector3(0f, 0f, 0f);
        transform.position = new Vector3(holepos.x, holepos.y - 3, holepos.z);

        yield return new WaitForSeconds(2f);

        // grave
        int randomsprite = UnityEngine.Random.Range(0, tombSprite.Length);
        spriteRenderer.sprite = tombSprite[randomsprite];

        //Sequence sequence = DOTween.Sequence();

        transform.DOMove(new Vector3(holepos.x, holepos.y, holepos.z), 1f);
        transform.DOScale(1f, 0.5f).SetEase(Ease.OutBounce);
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
