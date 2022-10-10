using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PutDownCorpse : IPutDown
{
    public void PutDown(PlayerTest player)
    {
        Debug.Log("Corpse put down");
        player.objToPutDown = null;
    }
}
