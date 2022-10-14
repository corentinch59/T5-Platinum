using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Carryable : MonoBehaviour, IInteractable, IPutDown
{
    public virtual void Interact(PlayerTest player)
    {
    }

    public virtual void PutDown(PlayerTest player)
    {
    }
}
