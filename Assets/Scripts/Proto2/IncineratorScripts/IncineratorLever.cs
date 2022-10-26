using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IncineratorLever : MonoBehaviour, IInteractable
{
    private Player playerRef;

    public void Interact(Player player)
    {
        if (!playerRef)
        {
            playerRef = player;
            player.DisableInput("Move");
            Debug.Log("Holding an incinerator lever.");
            IncineratorScript.OnPlayerHold?.Invoke();
        }

        if(player == playerRef)
        {
            player.EnableInput("Move");
            Debug.Log("Stopped Holding an incinerator lever.");
            IncineratorScript.OnPlayerLetgo?.Invoke();
        }
    }
}
