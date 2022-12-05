using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class GameManager : MonoBehaviour
{
    private static GameManager instance;
    public static GameManager Instance { get; private set; }

    [Serializable]
    public struct PnjGivesQuest
    {
        public PnjGivesQuest(PNJInteractable newPnj, Transform newQuestLocation, Transform newReturnLocation)
        {
            pnj = newPnj;
            questLocation = newQuestLocation;
            returnLocation = newReturnLocation;
        }

        public PNJInteractable pnj;
        public Transform questLocation;
        public Transform returnLocation;
    }

    [SerializeField] public List<PnjGivesQuest> pnjs;

    [SerializeField] private List<PNJInteractable> pnjsAlreadyGaveQuest;
    public List<PNJInteractable> PnjsAlreadyGaveQuest => pnjsAlreadyGaveQuest;

    protected virtual void Awake()
    {
        if (Instance == null)
            Instance = this;
    }

    private void Start()
    {
        for(int i = 0; i < pnjs.Count; i++)
        {
            pnjs[i].pnj.returnLoc = pnjs[i].returnLocation;
            pnjs[i].pnj.questLoc = pnjs[i].questLocation;
            pnjs[i].pnj.transform.position = pnjs[i].pnj.returnLoc.position;
        }
        NewPNJComingWithQuest();

        SoundManager.instance.Play("MainLoop");
        SoundManager.instance.Play("Ambiance");
    }

    public void NewPNJComingWithQuest()
    {
        PNJInteractable newPnjGivesQuest;

        // Choose random pnj who will give a new quest
        // We check if newPnjGivesQuest is not the same as the previous one 
        // And also if he is not the same as the pnjsAlreadyGaveQuest
        int randomPnj = Random.Range(0, pnjs.Count);
        newPnjGivesQuest = pnjs[randomPnj].pnj;

        if(!pnjsAlreadyGaveQuest.Contains(newPnjGivesQuest))
        {
            pnjsAlreadyGaveQuest.Add(newPnjGivesQuest);
        }
        else
        {
            // < of the max pnjs in scene
            if(pnjsAlreadyGaveQuest.Count < 3)
            {
                // change pnj
                while (pnjsAlreadyGaveQuest.Contains(newPnjGivesQuest))
                {
                    randomPnj = Random.Range(0, pnjs.Count);
                    newPnjGivesQuest = pnjs[randomPnj].pnj;
                }
                pnjsAlreadyGaveQuest.Add(newPnjGivesQuest);
            }
            else
            {
                // if the list is full return
                return;
            }
        }
        newPnjGivesQuest.StartCoroutine(newPnjGivesQuest.Walk(true));
    }
}