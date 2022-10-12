using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PNJInteractable : MonoBehaviour, IInteractable
{
    [SerializeField] private Request request;
    [SerializeField] private GameObject requestImg;
    [SerializeField] private Corpse corpseToCreate;

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
        Corpse corpseCreated = Instantiate(corpseToCreate, transform.position, Quaternion.identity);
        Debug.Log("Quest accepted");
    }
}
