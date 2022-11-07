using UnityEngine;
using UnityEditor;

public class HierarchyEditor : Editor
{
    [MenuItem("GameObject/Add Custom Look", false, 49)]
    public static void ChangeColor()
    {
        if (Selection.activeGameObject != null)
        {
            //Debug.Log(Selection.activeGameObject.scene);
            Selection.activeGameObject.AddComponent<MyCustomObject>();
            //Selection.
        }
    }
}
