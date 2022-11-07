using UnityEditor;
using UnityEngine;

[DisallowMultipleComponent]
public class MyCustomObject : MonoBehaviour
{
#if UNITY_EDITOR
    [SerializeField] private Color32 backgroundColor = new Color(255, 255, 255, 255);
    [SerializeField] private Color32 textColor = new Color(0, 0, 0, 255);
    [SerializeField] private int fontSize = 12;
    [SerializeField] private FontStyle fontStyle = FontStyle.Normal;
    private TextAnchor alignment = TextAnchor.MiddleCenter;

    public Color32 BackgroundColor { get { return backgroundColor; } }
    public Color32 TextColor { get { return textColor; } }
    public int FontSize { get { return fontSize; } }
    public FontStyle FontStyle { get { return fontStyle; } }
    public TextAnchor Alignment { get { return alignment; } }

    private void OnValidate()
    {
        EditorApplication.RepaintHierarchyWindow();
    }
#endif
}
