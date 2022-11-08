using UnityEditor;
using UnityEngine;

[InitializeOnLoad]
public static class MyCustomHierarchy
{
    static MyCustomHierarchy()
    {
        EditorApplication.hierarchyWindowItemOnGUI += OnChangeHierarchyWindowItemOnGUI;
    }

    private static void OnChangeHierarchyWindowItemOnGUI(int instanceID, Rect selectionRect)
    {
        Object instance = EditorUtility.InstanceIDToObject(instanceID);

        if (instance != null)
        {
            MyCustomObject myPrettyObject = (instance as GameObject).GetComponent<MyCustomObject>();

            if (myPrettyObject != null)
            {
                MyHierarchyItem myHierarchyItem = new MyHierarchyItem(instanceID, myPrettyObject, selectionRect);
                PaintBackground(myHierarchyItem);
                PaintText(myHierarchyItem);
            }
        }
    }

    private static void PaintBackground(MyHierarchyItem item)
    {
        Color32 color;
        color = item.PrettyObject.BackgroundColor;

        EditorGUI.DrawRect(item.BackgroundRect, color);
    }  
    private static void PaintText(MyHierarchyItem item)
    {
        Color32 color;
        color = item.PrettyObject.TextColor;

        GUIStyle labelGUIStyle = new GUIStyle
        {
            normal = new GUIStyleState { textColor = color },
            fontStyle = item.PrettyObject.FontStyle,
            alignment = item.PrettyObject.Alignment,
            fontSize = item.PrettyObject.FontSize,
        };
        EditorGUI.LabelField(item.TextRect, item.PrettyObject.name, labelGUIStyle);
    }
}
