using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PutDownCorpse : IPutDown
{
    public void PutDown(PlayerTest player)
    {
        Debug.Log("Corpse put down");
        player.objToPutDown = null;

        // Debug
        player.carriedObj.gameObject.SetActive(true);
        player.carriedObj.gameObject.GetComponent<MeshRenderer>().material.color = Color.black;
        player.playerMovement.canMove = false;
        //put down corpse in front of a player -> use rotation but now just t.right
        player.carriedObj.gameObject.transform.position = new Vector3(player.transform.position.x + player.playerMovement.orientation.x * 1.5f, 
            player.transform.position.y, player.transform.position.z + player.playerMovement.orientation.y * 1.5f);

        player.carriedObj = null;
        player.playerMovement.canMove = true;
    }
}
