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
            Debug.Log("Interacted with a lever");
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
