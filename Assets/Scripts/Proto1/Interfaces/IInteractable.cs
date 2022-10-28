using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public interface IInteractable
{
    public void Interact(PlayerTest player);   
    public void SetVibrations(PlayerInput playerInput, float frequencyLeftHaptic, float frequencyRightHaptic);
    public IEnumerator SetVibrationsCoroutine(PlayerInput playerInput, float frequencyLeftHaptic, float frequencyRightHaptic);
}
