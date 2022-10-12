using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Corpse : Carryable
{
    public CorpseData corpseData;
    private Vector2 direction;
    private List<PlayerTest> players = new List<PlayerTest>();

    public Sprite spriteCarry;
 
    private void Update()
    {
        // players movement
    }

    public override void Interact(PlayerTest player)
    {
        Debug.Log("Interaction");
        // need many players
        if(corpseData.size > 1)
        {
            players.Add(player);
            foreach(PlayerTest p in players)
            {
                p.objToPutDown = new PutDownCorpse(); // multiple people have to put down or leave the corpse
                p.carriedObj = this;
                player.interactableObj = null;
            }
        }

        // One player
        player.GetComponent<SpriteRenderer>().sprite = spriteCarry;
        player.interactableObj = null;
        player.carriedObj = this;
        player.carriedObj.gameObject.SetActive(false);
        player.objToPutDown = new PutDownCorpse();
    }
}
