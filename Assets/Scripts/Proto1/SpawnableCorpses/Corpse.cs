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
    private List<PlayerTest> players = new List<PlayerTest>();
    public float radius = 10f;
    public LayerMask localisationsLayer;
    public Quest thisQuest;

    [SerializeField] private List<Transform> snapLocations = new List<Transform>();
    private List<Transform> transformToReinject = new List<Transform>();
    private Vector3 snapPlayer;

    public override void Interact(PlayerTest player)
    {
        transform.localScale = new Vector3(4, 1, 2);

        // snap player to the corpse
        float distSnap = float.MaxValue;
        int transformToRemove = 0;
        for(int i = 0; i < snapLocations.Count; i++)
        {
            if(Vector3.Distance(player.transform.position, snapLocations[i].position) <= distSnap)
            {
                distSnap = Vector3.Distance(player.transform.position, snapLocations[i].position);
                snapPlayer = snapLocations[i].position;
                transformToRemove = i;
            }
        }
        transformToReinject.Add(snapLocations[transformToRemove]);
        snapLocations.RemoveAt(transformToRemove);
        player.transform.position = snapPlayer;

        player.transform.DOComplete();
        //player.gameObject.transform.DOPause(); -> worked a bit
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

            if (!players.Contains(player))
            {
                players.Add(player);
            }

            players[0].playerMovement.ChangeInput("Pilote");
            players[0].playerMovement.IsPilote.SetActive(true);
            transform.parent = players[0].transform;

            if (players.Count > 1)
            {
                transform.parent = players[1].transform;
                players[1].playerMovement.ChangeInput("Co-Pilote");
                players[1].playerMovement.IsCoPilote.SetActive(true);
                players[1].transform.parent = players[0].transform;
                players[0].playerMovement.positionCopilote = players[1].transform;
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
    }

    public override void PutDown(PlayerTest player, bool isTimeOut = false)
    {
        transform.localScale = new Vector3(4f, 1, 2);

        for(int i = 0; i < transformToReinject.Count; i++)
        {
            // WIP somtimes it's working sometimes not
            if (transformToReinject[i].position == player.transform.position)
            {
                Debug.Log("same pos");
                if(snapLocations.Count > 0)
                {
                    snapLocations.Insert(snapLocations.Count - 1, transformToReinject[i]);
                }
                else
                {
                    snapLocations.Insert(snapLocations.Count, transformToReinject[i]);
                }
                transformToReinject.RemoveAt(i);
                break;
            }
        }

        /*if(player.playerMovement.CurrentInput == "Pilote")
        {
            player.playerMovement.IsPilote.SetActive(false);
        } else if(player.playerMovement.CurrentInput == "Co-Pilote")
        {
            player.playerMovement.IsCoPilote.SetActive(false);
        }*/

        // DEBUG CARRYING W/ OTHER PLAYER
        player.playerMovement.ChangeInput("Player");

        if (player.playerMovement.canMove && players.Count < 2) // if one player -> put the body anywhere he wants to
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
        if (players.Contains(player) && players.Count > 1)
        {
            // if player is the pilote
            if (players.IndexOf(player) == 0)
            {
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
