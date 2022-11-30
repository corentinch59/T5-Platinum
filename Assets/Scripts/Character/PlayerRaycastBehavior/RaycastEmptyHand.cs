using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RaycastEmptyHand : IRaycastBehavior
{
    public GameObject PerformRaycast(Vector3 position, float radius, LayerMask layer, string[] objTag = null)
    {
        Collider[] colliders = Physics.OverlapSphere(position, radius, layer);
        if (colliders.Length > 0)
        {
            GameObject interactableObject = colliders[0].gameObject;
            for (int i = 0; i < colliders.Length; i++)
            {
                if(objTag != null)
                {
                    for(int j = 0; j < objTag.Length; j++)
                    {
                        if(colliders[i].gameObject.tag == objTag[j])
                        {
                            return colliders[i].gameObject;
                        }
                    }
                }

                float distanceCurrent = (colliders[i].transform.position - position).magnitude;
                float distancePrevious = (interactableObject.transform.position - position).magnitude;

                if (distanceCurrent > distancePrevious)
                {
                    interactableObject = colliders[i].gameObject;
                }
            }

            if(objTag != null)
            {
                return null;
            }
            else
            {
                return interactableObject;
            }
        }
        return null;
    }
}
