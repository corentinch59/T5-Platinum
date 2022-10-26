using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Carryable : MonoBehaviour, IInteractable, IPutDown
{
    public virtual void Interact(Player player)
    {
    }

    public virtual void PutDown(Player player, bool isTimeOut = false)
    {
    }
}
