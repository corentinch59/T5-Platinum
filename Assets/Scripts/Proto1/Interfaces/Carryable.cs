using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Carryable : MonoBehaviour, IInteractable, IPutDown
{
    public virtual void Interact(PlayerTest player)
    {
    }

    public virtual void PutDown(PlayerTest player, bool isTimeOut = false)
    {
    }

    public virtual void SetVibrations(PlayerInput playerInput, float frequencyLeftHaptic, float frequencyRightHaptic)
    {
        StartCoroutine(SetVibrationsCoroutine(playerInput, frequencyLeftHaptic, frequencyRightHaptic));
    }

    public IEnumerator SetVibrationsCoroutine(PlayerInput playerInput, float frequencyLeftHaptic, float frequencyRightHaptic)
    {
        playerInput.GetDevice<Gamepad>().SetMotorSpeeds(frequencyLeftHaptic, frequencyRightHaptic);
        yield return new WaitForSeconds(0.5f);
        playerInput.GetDevice<Gamepad>().PauseHaptics();
    }
    
    
}
