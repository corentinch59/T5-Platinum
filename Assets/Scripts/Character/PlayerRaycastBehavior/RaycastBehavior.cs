using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IRaycastBehavior
{
    public abstract GameObject PerformRaycast(Vector3 position, float radius, LayerMask layer, string objTag = "");
}
