using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Have to add a collider
public class Grave : MonoBehaviour
{
    public float radius;
    /*[HideInInspector]*/ public List<string> localisations = new List<string>(); 
    public LayerMask localisationsLayer;

    [ContextMenu("Update Localisations")]
    public void UpdateLocalisation()
    {
        Collider[] corpsInAreas = Physics.OverlapSphere(transform.position, radius, localisationsLayer);

        foreach (Collider col in corpsInAreas)
        {
            localisations.Add(col.gameObject.tag);
        }
    }

    [ContextMenu("Remove Localisations")]
    public void RemoveLocalisations()
    {
        localisations.Clear();
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.DrawWireSphere(transform.position, radius);
        Gizmos.color = Color.yellow;
    }
}
