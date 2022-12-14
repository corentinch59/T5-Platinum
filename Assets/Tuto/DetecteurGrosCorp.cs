using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DetecteurGrosCorp : MonoBehaviour
{
    public GameObject DialogueController;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    void OnTriggerExit(Collider truc)
    {
        {
            if (truc.tag == "Hole")
            {
                DialogueController.SendMessage("GrosTombe");
                Debug.Log("Hello");
            }
        }
    }
}
