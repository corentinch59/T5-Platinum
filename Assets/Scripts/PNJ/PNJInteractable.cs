using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.InputSystem;

public class PNJInteractable : MonoBehaviour, IInteractable
{
    [SerializeField] private DigRequest request;
    [SerializeField] private GameObject requestImg;
    [SerializeField] private GameObject corpseToCreate;
    [SerializeField] private Transform startLoc;
    [SerializeField] private Transform endLoc;
    [SerializeField] private NavMeshAgent agent;

    private bool isInteractable = true;

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
    }

    private void DisplayQuest()
    {
        requestImg.SetActive(true);
    }

    public void Interact(PlayerTest player)
    {
        if (isInteractable)
        {
            requestImg.SetActive(false);
            request.AcceptDigRequest();

            // spawn Corpse To Bury
            Vector3 spawn = new Vector3(transform.position.x, transform.position.y, transform.position.z + 2);
            GameObject corpseCreated = Instantiate(corpseToCreate, spawn, Quaternion.identity);

            // GameFeel
            corpseCreated.transform.DOScale(new Vector3(4, 1, 2f), 0.5f);

            if(corpseCreated.TryGetComponent(out Corpse c))
            {
                // corpseCreated is taking data from the request
                c.thisQuest = request.quest.GetComponent<Quest>();

            }

            //StartCoroutine(Walk(false));
            //Destroy(this); // enable = false not working
            isInteractable = false;
        }
    }

    public IEnumerator Walk(bool isWalkingForward)
    {
        //Arrive Avec sa quete
        if (isWalkingForward)
        {
            requestImg.SetActive(true);
            agent.destination = endLoc.position;
            //transform.DOMove(endLoc.position, 2);
        }
        //a plus de quete et rentre chez lui
        else
        {
            requestImg.SetActive(false);
            agent.destination = startLoc.position;
            //transform.DOMove(startLoc.position, 2);
        }
        yield return new WaitForSeconds(agent.remainingDistance / agent.speed);
        isInteractable = true;
    }

    public void SetVibrations(PlayerInput playerInput, float frequencyLeftHaptic, float frequencyRightHaptic)
    {
    }

    public IEnumerator SetVibrationsCoroutine(PlayerInput playerInput, float frequencyLeftHaptic, float frequencyRightHaptic)
    {
        yield break;
    }
}
