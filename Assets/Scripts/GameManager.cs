using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public delegate void StartHandler();
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

    private List<PNJInteractable> pnjsAlreadyGaveQuest = new List<PNJInteractable>();
    public List<PNJInteractable> PnjsAlreadyGaveQuest => pnjsAlreadyGaveQuest;

    private IEnumerator questsSpawning;
    public IEnumerator QuestsSpawning { get { return questsSpawning; } set { questsSpawning = value; } }

    [SerializeField] private float timeBetweenEachQuest = 5f;
    [SerializeField] private float timeToRemoveFromEveryQuest = 1f;

    public static event StartHandler OnStart;
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
        //questsSpawning = NewPNJComingWithQuest();
        //StartCoroutine(questsSpawning);

        SoundManager.instance.Play("MainLoop");
        SoundManager.instance.Play("Ambiance");

        SpawnPlayers.OnAllPlayerJoin += SpawnPlayers_OnAllPlayerJoin;
    }

    private void SpawnPlayers_OnAllPlayerJoin()
    {
        questsSpawning = NewPNJComingWithQuest();
        StartCoroutine(questsSpawning);
    }

    public IEnumerator NewPNJComingWithQuest()
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
            if(pnjsAlreadyGaveQuest.Count < pnjs.Count)
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
                // if the list is full break
                // => No more quest until players will take care of an ancient one
                questsSpawning = null;
                yield break;
            }
        }
        newPnjGivesQuest.StartCoroutine(newPnjGivesQuest.Walk(true));

        yield return new WaitForSeconds(timeBetweenEachQuest);

        timeBetweenEachQuest -= timeToRemoveFromEveryQuest;

        StartCoroutine(NewPNJComingWithQuest());
    }

    private void OnDestroy()
    {
        SpawnPlayers.OnAllPlayerJoin -= SpawnPlayers_OnAllPlayerJoin;
    }
}