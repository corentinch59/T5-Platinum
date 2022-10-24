using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PutDownCorpse : IPutDown
{
    public void PutDown(PlayerTest player, bool isTimeOut = false)
    {

        // Visual Debug 
        player.carriedObj.gameObject.SetActive(true);
        player.GetComponent<SpriteRenderer>().sprite = player.playerNotCarrying;
        player.carriedObj.gameObject.GetComponent<MeshRenderer>().material.color = Color.black;

        //put down corpse in front of a player -> use rotation but now just t.right
        player.carriedObj.gameObject.transform.position = new Vector3(player.transform.position.x + player.playerMovement.orientationVect.x * 3f, 
            player.transform.position.y, player.transform.position.z + player.playerMovement.orientationVect.y * 3f);

        player.carriedObj = null;
    }
}
