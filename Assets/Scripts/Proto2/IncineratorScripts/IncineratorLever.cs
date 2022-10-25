using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IncineratorLever : MonoBehaviour
{
    private bool hasPLayer;
    public void LeverInteraction(PlayerIncineration player)
    {
        if (!hasPLayer)
        {
            hasPLayer = true;
            player.DisableInput("Move");
            Debug.Log("Holding an incinerator lever .");
            IncineratorScript.OnPlayerHold?.Invoke();
        }
        else
        {
            hasPLayer = false;
            player.EnableInput("Move");
            Debug.Log("Stopped Holding an incinerator lever.");
            IncineratorScript.OnPlayerLetgo?.Invoke();
        }
    }
}
