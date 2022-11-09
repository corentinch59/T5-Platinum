using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RaycastEmptyHand : IRaycastBehavior
{
    public GameObject PerformRaycast(Player player,Vector3 position, float radius, LayerMask layer)
    {
        Collider[] colliders = Physics.OverlapSphere(position, radius, layer);
        if (colliders.Length > 0)
        {
            GameObject interactableObject = colliders[0].gameObject;
            for (int i = 0; i < colliders.Length; i++)
            {
                float distanceCurrent = (colliders[i].transform.position - player.transform.position).magnitude;
                float distancePrevious = (interactableObject.transform.position - player.transform.position).magnitude;

                if (distanceCurrent > distancePrevious)
                {
                    interactableObject = colliders[i].gameObject;
                }
            }
            return interactableObject;
        }
        return null;
    }
}