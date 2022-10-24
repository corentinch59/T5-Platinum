using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IncineratorLever : MonoBehaviour
{
    private bool hasPLayer;
    public void LeverInteraction(PlayerProto2 player)
    {
        if (!hasPLayer)
        {
            hasPLayer = true;
            player.DisableInput("Move");
            IncineratorScript.OnPlayerHold?.Invoke();
        }
        else
        {
            hasPLayer = false;
            player.EnableInput("Move");
            IncineratorScript.OnPlayerLetgo?.Invoke();
        }
    }
}
