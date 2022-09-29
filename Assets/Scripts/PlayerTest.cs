using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerTest : MonoBehaviour
{
    public GameObject graveToCreate;

    public GameObject corpse;//debug in public
    public bool gotCorpse; // have to set false

    private float timerDigInit;
    public float timerDig;

    private int numberMashDigUpInit;
    public int numberMashDigUp;

    public float maxDistVision;
    public LayerMask graveLayer;
    /*public float timerDig
    {
        get { return _timerDig; }
        set { if(_timerDig < )_timerDig = value; }
    }*/

    private void Start()
    {
        gotCorpse = false;
        timerDigInit = timerDig;
        numberMashDigUpInit = numberMashDigUp;
    }

    private void Update()
    {
        //Block movement if dig or dig up
        if (Input.GetKey(KeyCode.Space))
        {
            // maintain to dig
            Debug.DrawRay(transform.position, transform.forward, Color.black);
            Dig();
        }

        if (Input.GetKeyUp(KeyCode.Space))
        {
            timerDig = timerDigInit;
        }

        // Check if player is facing a grave
        bool isHit = Physics.Raycast(transform.position, transform.right,out RaycastHit hit, maxDistVision, graveLayer);

        //Debug player vision
        Debug.DrawRay(transform.position, transform.right * maxDistVision, Color.red);

        if (isHit)
        {
            corpse = hit.collider.gameObject;
            // mash to digUp
            if (Input.GetKeyDown(KeyCode.E))
            {
                numberMashDigUp--;
                if (numberMashDigUp <= 0)
                {
                    DigUp();
                }
            }
        }
        else
        {
            numberMashDigUp = numberMashDigUpInit;
        }
        

        if (!gotCorpse)
        {
            // can carry corpse
        }
    }

    public void Dig()
    {
        if (gotCorpse && corpse != null)
        {
            timerDig -= Time.deltaTime;
            if (timerDig <= 0)
            {
                // Create grave at a certain position
                Debug.Log("Hole Dug");
                if (corpse.TryGetComponent(out Corpse c))
                {
                    c.UpdateLocalisation();
                }
                // create grave in front of the player /!\ have to check the rotation in game
                Debug.Log(transform.forward);
                Instantiate(graveToCreate, transform.position + transform.forward, Quaternion.identity);
                timerDig = timerDigInit;
            }
        }
    }

    public void DigUp()
    {
        Debug.Log("Hole dug up");
        numberMashDigUp = numberMashDigUpInit;
        if (corpse.TryGetComponent(out Corpse c))
        {
            c.RemoveLocalisations();
        }
    }
}
