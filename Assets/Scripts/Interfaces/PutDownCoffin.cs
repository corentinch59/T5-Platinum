using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PutDownCoffin : IPutDown
{
    public void PutDown(PlayerTest player)
    {
        player.objToPutDown = null;
    }
}
