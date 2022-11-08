using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IRaycastBehavior
{
    public abstract GameObject PerformRaycast(Player player,Vector3 position, float radius, LayerMask layer);
}
