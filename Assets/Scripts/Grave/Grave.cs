using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Grave : MonoBehaviour
{
    public float radius;
    public RequestDataBase.localisation localisation; 
    public LayerMask localisationsLayer;

    [ContextMenu("Update Localisations")]
    public void UpdateLocalisation()
    {
        Collider[] corpsInAreas = Physics.OverlapSphere(transform.position, radius, localisationsLayer);

        foreach (Collider col in corpsInAreas)
        {
            localisation = AddLocalisation(col.gameObject.tag);
        }
    }

    [ContextMenu("Remove Localisations")]
    public void RemoveLocalisations()
    {
        localisation = AddLocalisation("");
    }

    public RequestDataBase.localisation AddLocalisation(string tag)
    {
        switch (tag)
        {
            case "Water": return RequestDataBase.localisation.WATER;
            case "Tree": return RequestDataBase.localisation.TREE;
            default: return RequestDataBase.localisation.NONE;
        }
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.DrawWireSphere(transform.position, radius);
        Gizmos.color = Color.yellow;
    }
}
