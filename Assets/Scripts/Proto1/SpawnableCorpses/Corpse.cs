using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Corpse : Carryable
{
    public CorpseData corpseData;
    private Vector2 direction;
    private PlayerTest[] players = new PlayerTest[2];
    public float radius = 10f;
    public LayerMask localisationsLayer;
    public Quest thisQuest;
    [SerializeField] private Sprite[] tombSprite = new Sprite[5];

    private SpriteRenderer spriteRenderer;

    private void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    [SerializeField] private Transform piloteLocation;
    [SerializeField] private Transform coPiloteLocation;
    private Vector3 snapPlayer;

    private void Update()
    {
        if (players[0] != null && players[1] != null)
            transform.LookAt(Camera.main.transform);
    }

    public override void Interact(PlayerTest player)
    {
        // To avoid dotween problem with player increasing scale of this (as a child)
        if(thisQuest.requestInfos.siz > 0)
        {
            transform.localScale = new Vector3(2, 2, 2);
        }
        else
        {
            transform.localScale = new Vector3(1, 1, 1);
        }

        //player.transform.DOComplete();
        

        // DEBUG CARRING W/ OTHER PLAYER
        if ((int)thisQuest.requestInfos.siz > 0)
        {

            // snap player to the corpse
            if(Vector3.Distance(player.transform.position, piloteLocation.position) <= 5)
            {
                if (players[0] == null)
                {
                    //Debug.Log("Player is pilote");
                    player.playerMovement.canMove = false;
                    player.gameObject.transform.DOPause(); //-> worked a bit
                    SetVibrations(player.playerMovement.PlayerInput, 0.1f, 0.1f);

                    if (player.carriedObj == null)
                    {
                        player.carriedObj = this;
                        player.interactableObj = null;
                        player.isCarrying = true;
                    }
                    players[0] = player;
                    //player.transform.DOMove(piloteLocation.position, 0.5f, true).SetEase(Ease.Linear);
                    player.transform.position = piloteLocation.position;
                    player.playerMovement.ChangeInput("Pilote");
                    player.playerMovement.IsPilote.SetActive(true);
                    if (players[1] != null)
                        player.playerMovement.positionCopilote = players[1].transform;
                }
            }
            else if(Vector3.Distance(player.transform.position, coPiloteLocation.position) <= 5)
            {
                if (players[1] == null)
                {
                    //Debug.Log("Player is co-pilote");
                    player.playerMovement.canMove = false;
                    player.gameObject.transform.DOPause(); //-> worked a bit
                    SetVibrations(player.playerMovement.PlayerInput, 0.1f, 0.1f);

                    if (player.carriedObj == null)
                    {
                        player.carriedObj = this;
                        player.interactableObj = null;
                        player.isCarrying = true;
                    }
                    players[1] = player;
                    //player.transform.DOMove(coPiloteLocation.position, 0.5f, true).SetEase(Ease.Linear);
                    player.transform.position = coPiloteLocation.position;
                    player.playerMovement.ChangeInput("Co-Pilote");
                    player.playerMovement.IsCoPilote.SetActive(true);
                    if (players[0] != null)
                        players[0].playerMovement.positionCopilote = player.transform;
                }
            }

            // If everyone is up to carry the body then they can move
            if ((int)thisQuest.requestInfos.siz + 1 == players.Length)
            {
                int playersSet = 0;
                for(int i = 0; i < players.Length; ++i)
                {
                    if (players[i] != null)
                        playersSet++;
                }
                if(playersSet > 1)
                {
                    players[0].playerMovement.canMove = true;
                    players[1].playerMovement.canMove = true;
                    transform.parent = players[1].transform;
                    players[1].transform.parent = players[0].transform;
                }
            }
        } else if((int)thisQuest.requestInfos.siz <= 0)
        {

            if (player.carriedObj == null)
            {
                player.carriedObj = this;
                player.interactableObj = null;
                player.isCarrying = true;
            }
            player.GetComponent<SpriteRenderer>().sprite = player.spriteCarry;
            player.carriedObj.gameObject.SetActive(false);
        }
    }

    public override void PutDown(PlayerTest player, bool isTimeOut = false)
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

        // DEBUG CARRYING W/ OTHER PLAYER
        player.playerMovement.ChangeInput("Player");



        for(int i = 0; i < players.Length; i++)
        {
            if (players[i] == player)
            {
                if(i == 0)
                {
                    // Pilote is leaving
                    //Debug.Log("Pilote leaves");
                    player.playerMovement.IsPilote.SetActive(false);
                    if (players[1] != null)
                    {
                        transform.parent = players[1].transform;
                        players[1].transform.parent = null;
                        players[1].playerMovement.canMove = false;
                    }
                    else
                    {
                        transform.parent = null;
                    }
                    player.playerMovement.positionCopilote = null;
                }
                else
                {
                    // Copilote is leaving
                    //Debug.Log("Co-Pilote leaves");
                    player.playerMovement.IsCoPilote.SetActive(false);
                    if(players[0] != null)
                    {
                        players[0].playerMovement.canMove = false;
                        players[0].playerMovement.positionCopilote = null;
                        player.transform.parent = null;
                        transform.parent = players[0].transform;
                    }
                    else
                    {
                        transform.parent = null;
                    }
                }
                players[i] = null;
            }
        }

        if (player.playerMovement.canMove && players[0] == null && players[1] == null) // if one player -> put the body anywhere he wants to
        {
            //put down corpse in front of a player -> use rotation but now just t.right
            player.carriedObj.gameObject.transform.position = new Vector3(player.transform.position.x + player.playerMovement.orientationVect.x * 3f,
                player.transform.position.y, player.transform.position.z + player.playerMovement.orientationVect.y * 3f);
        }
        else
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
