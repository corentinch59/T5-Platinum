using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class IncineratorLever : MonoBehaviour, IInteractable
{
    private Player playerRef;

    public void Cancel(Player player, Hole holeDetected)
    {
    }

    public void Interact(Player player)
    {
        if (!playerRef)
        {
            playerRef = player;
            //player.DisableInput("Move");
            Debug.Log("Holding an incinerator lever.");
            IncineratorScript.OnPlayerHold?.Invoke();
        }

        if(player == playerRef)
        {
            //player.EnableInput("Move");
            Debug.Log("Stopped Holding an incinerator lever.");
            IncineratorScript.OnPlayerLetgo?.Invoke();
        }
    }

    public void SetVibrations(PlayerInput playerInput, float frequencyLeftHaptic, float frequencyRightHaptic)
    {
    }

    public IEnumerator SetVibrationsCoroutine(PlayerInput playerInput, float frequencyLeftHaptic, float frequencyRightHaptic)
    {
        yield break;
    }
}
