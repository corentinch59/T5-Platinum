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
        public PNJInteractable pnj;
        public Transform questLocation;
        public Transform returnLocation;
    }

    [SerializeField] public List<PnjGivesQuest> pnjs;

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
    }

    public void NewPNJComingWithQuest(PNJInteractable previousPnj)
    {
        PNJInteractable newPnjGivesQuest;
        int randomPnj = Random.Range(0, pnjs.Count);
        newPnjGivesQuest = pnjs[randomPnj].pnj;
        while(pnjs[randomPnj].pnj == previousPnj)
        {
            randomPnj = Random.Range(0, pnjs.Count);
            newPnjGivesQuest = pnjs[randomPnj].pnj;
        }
        newPnjGivesQuest.StartCoroutine(newPnjGivesQuest.Walk(true));
    }
}
