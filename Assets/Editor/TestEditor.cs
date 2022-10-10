using System.Collections;
using System.Collections.Generic;
using Codice.CM.Common.Serialization;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(PlayerTest))]
public class TestEditor : Editor
{
    private PlayerTest myPlayer = null;

    public void OnEnable()
    {
        this.myPlayer = (PlayerTest)this.target;
    }

    public override void OnInspectorGUI()
    {
        GUIStyle myStyle = new GUIStyle();
        myStyle.fontSize = 15;
        myStyle.fontStyle = FontStyle.Bold;
        myStyle.normal.textColor = Color.white;
            
        EditorGUILayout.BeginHorizontal();
        EditorGUILayout.LabelField("Grave Settings", myStyle);
        EditorGUILayout.EndHorizontal();

        EditorGUILayout.Space(5f);

        this.myPlayer.distGraveCreation =
            EditorGUILayout.FloatField("Grave's Creation Distance", this.myPlayer.distGraveCreation);

        this.myPlayer.graveToCreate = EditorGUILayout.ObjectField("Grave To Create", this.myPlayer.graveToCreate, typeof(GameObject), true) as GameObject;

        EditorGUILayout.HelpBox("Set the grave's creation distance.", MessageType.Info);

        EditorGUILayout.BeginHorizontal();
        EditorGUILayout.LabelField("Dig/Dig Up Settings", myStyle);
        EditorGUILayout.EndHorizontal();

        this.myPlayer.timerDig =
            EditorGUILayout.FloatField("Time to dig", this.myPlayer.timerDig);

        this.myPlayer.numberMashDigUp =
            EditorGUILayout.IntField("How many times the player mash the button", this.myPlayer.numberMashDigUp);

        this.myPlayer.corpse = EditorGUILayout.ObjectField("Corpse the player will carry", this.myPlayer.corpse, typeof(GameObject), true) as GameObject;

    }
}
