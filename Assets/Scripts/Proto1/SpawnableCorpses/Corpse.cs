using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using DG.Tweening;

public class Corpse : Carryable
{
    public CorpseData corpseData;
    private Vector2 direction;
    private List<PlayerTest> players = new List<PlayerTest>();
    public float radius = 10f;
    public LayerMask localisationsLayer;
    public Quest thisQuest;
    [SerializeField] private Sprite[] tombSprite = new Sprite[5];

    private SpriteRenderer spriteRenderer;

    private void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    private void Update()
    {
        // players movement
    }

    public override void Interact(PlayerTest player)
    {
        if (player.carriedObj == null)
        {
            player.carriedObj = this;
            player.interactableObj = null;
            player.isCarrying = true;
        }

        // DEBUG CARRING W/ OTHER PLAYER
        if ((int)thisQuest.requestInfos.siz > 0)
        {
            player.playerMovement.canMove = false;

            if (!players.Contains(player))
            {
                players.Add(player);
            }

            players[0].playerMovement.ChangeInput("Pilote");

            if(players.Count > 1)
            {
                players[1].playerMovement.ChangeInput("Co-Pilote");
            }

            // If everyone is up to carry the body then they can move
            if ((int)thisQuest.requestInfos.siz + 1 == players.Count)
            {
                foreach(PlayerTest p in players)
                {
                    p.playerMovement.canMove = true;
                }
            }
        } else if((int)thisQuest.requestInfos.siz <= 0)
        {
            player.GetComponent<SpriteRenderer>().sprite = player.spriteCarry;
            player.carriedObj.gameObject.SetActive(false);
        }

        // need many players
        /* if((int)corpseData.size > 0)
         {
             players.Add(player);
             foreach(PlayerTest p in players)
             {
                 p.carriedObj = this; // multiple people have to put down or leave the corpse
                 player.interactableObj = null;
             }
         }*/

        // One player
        /*if(player.carriedObj == null)
        {
            player.interactableObj = null;
            player.carriedObj = this;
        }*/
    }

    public override void PutDown(PlayerTest player, bool isTimeOut = false)
    {
        // DEBUG CARRING W/ OTHER PLAYER
        player.playerMovement.ChangeInput("Player");
        if (player.playerMovement.canMove)
        {
            //put down corpse in front of a player -> use rotation but now just t.right
            player.carriedObj.gameObject.transform.position = new Vector3(player.transform.position.x + player.playerMovement.orientationVect.x * 3f,
                player.transform.position.y, player.transform.position.z + player.playerMovement.orientationVect.y * 3f);
        }

        if(!player.playerMovement.canMove)
        {
            player.playerMovement.canMove = true;
        }

        // corpse became grave (sprite)
        player.isCarrying = false;

        // Visual Debug 
        player.carriedObj.gameObject.SetActive(true);
        player.GetComponent<SpriteRenderer>().sprite = player.playerNotCarrying;
        //player.carriedObj.gameObject.GetComponent<MeshRenderer>().material.color = Color.black;

        int randomsprite = UnityEngine.Random.Range(0, tombSprite.Length);
        spriteRenderer.sprite = tombSprite[randomsprite];
       
        transform.localScale = new Vector3(0f, 0f, 0f);
        transform.position = new Vector3(transform.position.x, transform.position.y - 3, transform.position.z);
        //Sequence sequence = DOTween.Sequence();

        transform.DOMove(new Vector3(transform.position.x, transform.position.y + 3, transform.position.z), 1f);
        transform.DOScale(1.25f, 0.5f).SetEase(Ease.OutBounce);
            //.Append(transform.DOScale(1f, 0.25f));

        // update CorpseData
        corpseData = UpdateLocalisation();
        

        if(thisQuest != null)
        {
            // check if the corpse correspond to the quest -> finish quest
            corpseData = UpdateRequestLocalisation();
            StartCoroutine(thisQuest.FinishQuest(corpseData));
        }

        player.carriedObj = null;
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

    [ContextMenu("Remove Localisations")]
    public void RemoveLocalisations()
    {
        CorpseData newLoc;
        newLoc.localisation = AddLocalisation("");
        newLoc.name = corpseData.name;
        newLoc.size = corpseData.size;
        newLoc.corpseType = corpseData.corpseType;
        newLoc.coffinType = corpseData.coffinType;
        newLoc.specificity = corpseData.specificity;
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

    private CorpseData UpdateData(CorpseData cData)
    {
        CorpseData newCD = new CorpseData();
        newCD.name = thisQuest.requestInfos.corpseName;
        newCD.size = thisQuest.requestInfos.siz;
        newCD.corpseType = thisQuest.requestInfos.corps;
        newCD.coffinType = thisQuest.requestInfos.cof;
        newCD.specificity = thisQuest.requestInfos.spec;
        //newCD.localisation = AddLocalisation(col.gameObject.tag);
        return newCD;
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(transform.position, radius);
    }
}
