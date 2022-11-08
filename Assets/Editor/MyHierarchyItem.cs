using UnityEngine;
using UnityEditor;

public struct MyHierarchyItem
{
    public int id { get; }
    public bool isSelected { get; }
    public GameObject GameObject { get; }
    public MyCustomObject PrettyObject { get; }
    public Rect BackgroundRect { get; }
    public Rect TextRect { get; }

    public MyHierarchyItem(int instanceId, MyCustomObject myPrettyObject, Rect selectionRect)
    {
        id = instanceId;
        isSelected = Selection.Contains(instanceId);
        PrettyObject = myPrettyObject;
        GameObject = PrettyObject.gameObject;

        float xPos = selectionRect.position.x + 60f - 28f - selectionRect.xMin;
        float yPos = selectionRect.position.y;
        float xSize = selectionRect.size.x + selectionRect.xMin + 28f - 60 + 16f;
        float ySize = selectionRect.size.y;

        //Si ya des enfants on reduit le rect pour pouvoir acceder aux enfants facilement dans la hierarchie
        if (PrettyObject.gameObject.transform.childCount > 0) xPos += 28f;
        
        BackgroundRect = new Rect(xPos, yPos, xSize, ySize);

        xPos = selectionRect.position.x + 18f;
        yPos = selectionRect.position.y;
        xSize = selectionRect.size.x - 18f;
        ySize = selectionRect.size.y;
        TextRect = new Rect(xPos, yPos, xSize, ySize);
    }
}
    
