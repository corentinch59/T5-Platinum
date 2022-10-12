using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PutDownPeople : IPutDown
{
    public void PutDown(PlayerTest player)
    {
        player.objToPutDown = null;
    }
}
