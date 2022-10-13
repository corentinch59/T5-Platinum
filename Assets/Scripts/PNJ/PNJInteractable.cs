using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PNJInteractable : MonoBehaviour, IInteractable
{
    [SerializeField] private Request request;
    [SerializeField] private GameObject requestImg;
    [SerializeField] private GameObject corpseToCreate;

    private void Start()
    {
        request.SetRequest();
        requestImg.SetActive(false);
        // move
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
        Destroy(this); // enable = false not working
        // move back
        Debug.Log("Quest accepted");
    }
}
