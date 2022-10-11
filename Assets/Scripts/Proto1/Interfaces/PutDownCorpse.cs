using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PutDownCorpse : IPutDown
{
    public void PutDown(PlayerTest player)
    {
        Debug.Log("Corpse put down");
        player.objToPutDown = null;
        player.carriedObj.gameObject.SetActive(true);
        //put down corpse in front of a player -> use rotation but now just t.right
        player.carriedObj.gameObject.transform.position = player.transform.right * 2f;
        player.carriedObj = null;
    }
}
