/*using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using static GameManager;

[CustomEditor(typeof(GameManager))]
public class GameManagerEditor : Editor
{
    private GameManager myGM = null;

    private void OnEnable()
    {
        this.myGM = (GameManager)this.target;
    }

    public override void OnInspectorGUI()
    {
        bool addPNJ = GUILayout.Button("Add Every Quest's PNJs");
        if (addPNJ)
        {
            PNJInteractable[] pnjArray = FindObjectsOfType<PNJInteractable>();
            GameObject[] pnjQuestLocations = GameObject.FindGameObjectsWithTag("EndLoc");
            GameObject[] pnjReturnLocations = GameObject.FindGameObjectsWithTag("StartLoc");
            myGM.pnjs.Clear();
            for (int i = 0; i < pnjArray.Length; ++i)
            {
                PnjGivesQuest newPnjFound = new PnjGivesQuest(pnjArray[i], pnjQuestLocations[i].transform, pnjReturnLocations[i].transform);
                myGM.pnjs.Add(newPnjFound);
                Debug.Log(myGM.pnjs[i].pnj.name + " added");
            }
            Debug.Log("Correctly added your pnjs !");
        }
    }
}
*/