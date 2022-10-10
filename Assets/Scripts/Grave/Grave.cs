using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Have to add a collider
public class Grave : MonoBehaviour
{
    public float radius;
    /*[HideInInspector]*/ public List<LocalisationsEnum> localisations = new List<LocalisationsEnum>(); 
    public LayerMask localisationsLayer;

    [ContextMenu("Update Localisations")]
    public void UpdateLocalisation()
    {
        Collider[] corpsInAreas = Physics.OverlapSphere(transform.position, radius, localisationsLayer);

        foreach (Collider col in corpsInAreas)
        {
            localisations.Add(AddLocalisation(col.gameObject.tag));
        }
    }

    [ContextMenu("Remove Localisations")]
    public void RemoveLocalisations()
    {
        localisations.Clear();
    }

    public LocalisationsEnum AddLocalisation(string tag)
    {
        switch (tag)
        {
            case "Water": return LocalisationsEnum.WATER;
            case "Tree": return LocalisationsEnum.TREE;
            default: return LocalisationsEnum.NONE;
        }
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.DrawWireSphere(transform.position, radius);
        Gizmos.color = Color.yellow;
    }
}
