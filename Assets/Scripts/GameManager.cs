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
        NewPNJComingWithQuest(null);


        SoundManager.instance.Play("MainLoop");
    }

    public void NewPNJComingWithQuest(PNJInteractable previousPnj)
    {
        if(pnjsAlreadyGaveQuest.Count >= 4)
        {
            return;
        }

        // take random pnj (not the same as before) in the list and tell him to give player a quest
        if(pnjsAlreadyGaveQuest.Count <= 0)
        {
            PNJInteractable newPnjGivesQuest;
            int randomPnj = Random.Range(0, pnjs.Count);
            newPnjGivesQuest = pnjs[randomPnj].pnj;

            while(pnjs[randomPnj].pnj == previousPnj)
            {
                randomPnj = Random.Range(0, pnjs.Count);
                newPnjGivesQuest = pnjs[randomPnj].pnj;
            }
            pnjsAlreadyGaveQuest.Add(newPnjGivesQuest);
            newPnjGivesQuest.StartCoroutine(newPnjGivesQuest.Walk(true));
        }
        else
        {
            PNJInteractable newPnjGivesQuest;
            int randomPnj = Random.Range(0, pnjs.Count);
            newPnjGivesQuest = pnjs[randomPnj].pnj;

            if(pnjsAlreadyGaveQuest.Count >= 1 && pnjsAlreadyGaveQuest.Count < 4)
            {
                while (pnjsAlreadyGaveQuest.Contains(newPnjGivesQuest))
                {
                    randomPnj = Random.Range(0, pnjs.Count);
                    newPnjGivesQuest = pnjs[randomPnj].pnj;
                }
            }
            pnjsAlreadyGaveQuest.Add(newPnjGivesQuest);
            newPnjGivesQuest.StartCoroutine(newPnjGivesQuest.Walk(true));
        }
    }
}