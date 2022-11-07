using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.AI;

public class PNJInteractable : MonoBehaviour
{
    [SerializeField] private DigRequest request;
    [SerializeField] private GameObject requestImg;
    [SerializeField] private GameObject corpseToCreate;
    [SerializeField] private Transform startLoc;
    [SerializeField] private Transform endLoc;
    [SerializeField] private NavMeshAgent agent;

    private Coroutine feedback;

    public void Awake()
    {
        endLoc = GameObject.FindGameObjectWithTag("EndLoc").GetComponent<Transform>();
        startLoc = GameObject.FindGameObjectWithTag("StartLoc").GetComponent<Transform>();
    }

    private void Start()
    {
        transform.position = startLoc.position;
        StartCoroutine(Walk(true));
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            // display quest when arriving in pos
            DisplayQuest();
        }

        if (agent.hasPath && feedback == null)
        {
            feedback = StartCoroutine(FeedBackPlayerMoves());
        }
    }

    private void DisplayQuest()
    {
        requestImg.SetActive(true);
    }

    public void AddNewQuest()
    {
        requestImg.SetActive(false);
        request.AcceptDigRequest();

        // spawn Corpse To Bury
        Vector3 spawn = new Vector3(transform.position.x, transform.position.y, transform.position.z + 2);
        GameObject corpseCreated = Instantiate(corpseToCreate, spawn, Quaternion.identity);

        if (corpseCreated.TryGetComponent(out Corpse c))
        {
            // corpseCreated is taking data from the request
            c.thisQuest = request.quest.GetComponent<Quest>();
            // GameFeel
            if ((int)c.thisQuest.requestInfos.siz > 0)
            {
                // Big corpse
                corpseCreated.transform.DOScale(new Vector3(2, 2, 2), 0.5f);
            }
            else
            {
                // small corpse
                corpseCreated.transform.DOScale(new Vector3(1, 1, 1), 0.5f);
            }
        }

        StartCoroutine(Walk(false));
    }

    public IEnumerator Walk(bool isWalkingForward)
    {
        //Arrive Avec sa quete
        if (isWalkingForward)
        {
            requestImg.SetActive(true);
            agent.destination = endLoc.position;
            Debug.Log("Come with quest : " + agent.destination);
            yield return new WaitForSeconds(agent.remainingDistance / agent.speed);
            AddNewQuest();
        }
        //a plus de quete et rentre chez lui
        else
        {
            requestImg.SetActive(false);
            agent.destination = startLoc.position;
            Debug.Log("Go back : " + agent.destination);
            yield return null;
        }
    }

    private IEnumerator FeedBackPlayerMoves()
    {
        transform.DOScaleX(1.8f, 0.3f);
        transform.DOScaleY(2.3f, 0.3f);
        yield return new WaitForSeconds(0.3f);
        transform.DOScaleX(2f, 0.3f);
        transform.DOScaleY(2f, 0.3f);
        yield return new WaitForSeconds(0.3f);
        feedback = null;
    }
}
