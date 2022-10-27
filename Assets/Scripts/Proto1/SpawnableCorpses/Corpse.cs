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

    [SerializeField] private Transform piloteLocation;
    [SerializeField] private Transform coPiloteLocation;
    private Vector3 snapPlayer;

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
        player.gameObject.transform.DOPause(); //-> worked a bit
        SetVibrations(player.playerMovement.PlayerInput, 0.1f, 0.1f);

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

            // snap player to the corpse
            if (player.transform.position.x >= transform.position.x)
            {
                players[0] = player;
                player.transform.DOMove(piloteLocation.position, 0.5f, true).SetEase(Ease.Linear);
                player.playerMovement.ChangeInput("Pilote");
                player.playerMovement.IsPilote.SetActive(true);
                transform.parent = player.transform;
            }
            else
            {
                players[1] = player;
                player.transform.DOMove(coPiloteLocation.position, 0.5f, true).SetEase(Ease.Linear);
                player.playerMovement.ChangeInput("Co-Pilote");
                player.playerMovement.IsCoPilote.SetActive(true);
                transform.parent = player.transform;
                player.transform.parent = players[0].transform;
                players[0].playerMovement.positionCopilote = player.transform;
            }

            // If everyone is up to carry the body then they can move
            if ((int)thisQuest.requestInfos.siz + 1 == players.Length)
            {
                for(int i = 0; i < players.Length; ++i)
                {
                    players[i].playerMovement.canMove = true;
                }
            }
        } else if((int)thisQuest.requestInfos.siz <= 0)
        {
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

        if (player.playerMovement.canMove && players.Length < 2) // if one player -> put the body anywhere he wants to
        {
            //put down corpse in front of a player -> use rotation but now just t.right
            player.carriedObj.gameObject.transform.position = new Vector3(player.transform.position.x + player.playerMovement.orientationVect.x * 3f,
                player.transform.position.y, player.transform.position.z + player.playerMovement.orientationVect.y * 3f);
        }
        else
        {
            player.playerMovement.canMove = true;
        }

        // if multiple players
        if (players.(player) && players.Count > 1)
        {
            // if player is the pilote
            if (players.IndexOf(player) == 0)
            {
                players[0].playerMovement.IsPilote.SetActive(false);
                players[1].transform.parent = null;
                players[1].playerMovement.canMove = false;
                players[1].playerMovement.ChangeInput("Pilote");
                players[1].playerMovement.IsPilote.SetActive(true);
                players[1].playerMovement.IsCoPilote.SetActive(false);
            }
            else
            {
                player.playerMovement.IsCoPilote.SetActive(false);
                transform.parent = players[0].transform;
                players[0].playerMovement.canMove = false;
                player.transform.parent = null;
            }
            players[0].playerMovement.positionCopilote = null;
            players.Remove(player);
        } else if(players.Count < 2) // one player
        {
            players[0].playerMovement.IsPilote.SetActive(false);
            transform.parent = null;
            player.transform.parent = null;
            players.Remove(player);
        }

        // corpse became grave (sprite)
        player.isCarrying = false;

        // Visual Debug 
        player.carriedObj.gameObject.SetActive(true);
        player.GetComponent<SpriteRenderer>().sprite = player.playerNotCarrying;
        player.carriedObj.gameObject.GetComponent<MeshRenderer>().material.color = Color.black;


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
