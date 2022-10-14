using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class PNJInteractable : MonoBehaviour, IInteractable
{
    [SerializeField] private Request request;
    [SerializeField] private GameObject requestImg;
    [SerializeField] private GameObject corpseToCreate;
    [SerializeField] private Transform startLoc;
    [SerializeField] private Transform endLoc;
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
            request.AcceptRequest();

            // spawn Corpse To Bury
            Vector3 spawn = new Vector3(transform.position.x, transform.position.y, transform.position.z + 2);
            GameObject corpseCreated = Instantiate(corpseToCreate, spawn, Quaternion.identity);

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
            transform.DOMove(endLoc.position, 2);
        }
        //a plus de quete et rentre chez lui
        else
        {
            requestImg.SetActive(false);
            transform.DOMove(startLoc.position, 2);
        }
        
        yield return new WaitForSeconds(2);
        isInteractable = true;
    }
}