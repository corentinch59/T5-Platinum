using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Corpse : Carryable
{
    public CorpseData corpseData;
    private Vector2 direction;
    private List<PlayerTest> players = new List<PlayerTest>();
    public float radius = 10f;
    public LayerMask localisationsLayer;
    public Quest thisQuest;

    private void Update()
    {
        // players movement
    }

    public override void Interact(PlayerTest player)
    {
        player.isCarrying = true;
        player.GetComponent<SpriteRenderer>().sprite = player.spriteCarry;
        // need many players
        if((int)corpseData.size > 0)
        {
            players.Add(player);
            foreach(PlayerTest p in players)
            {
                p.carriedObj = this; // multiple people have to put down or leave the corpse
                player.interactableObj = null;
            }
        }
        // One player
        if(player.carriedObj == null)
        {
            player.interactableObj = null;
            player.carriedObj = this;
        }
        player.carriedObj.gameObject.SetActive(false);
    }

    public override void PutDown(PlayerTest player)
    {
        // corpse became grave (sprite)
        player.isCarrying = false;

        // Visual Debug 
        player.carriedObj.gameObject.SetActive(true);
        player.GetComponent<SpriteRenderer>().sprite = player.playerNotCarrying;
        player.carriedObj.gameObject.GetComponent<MeshRenderer>().material.color = Color.black;

        //put down corpse in front of a player -> use rotation but now just t.right
        player.carriedObj.gameObject.transform.position = new Vector3(player.transform.position.x + player.playerMovement.orientationVect.x * 3f,
            player.transform.position.y, player.transform.position.z + player.playerMovement.orientationVect.y * 3f);

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
