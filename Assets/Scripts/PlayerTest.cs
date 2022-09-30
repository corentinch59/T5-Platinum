using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerTest : MonoBehaviour
{
    public GameObject graveToCreate;

    [Tooltip("Add this GameObject as a child to the player")] public GameObject corpse;

    private float timerDigInit;
    public float timerDig;

    private int numberMashDigUpInit;
    public int numberMashDigUp;
    
    public LayerMask graveLayer;
    public float distGraveCreation;

    /*public float timerDig
    {
        get { return _timerDig; }
        set { if(_timerDig < )_timerDig = value; }
    }*/

    private void Start()
    {
        if (corpse != null)
        {
            corpse.SetActive(false);
        }
        else
        {
            Debug.LogError("You need to add a corpse in the component");
        }
        timerDigInit = timerDig;
        numberMashDigUpInit = numberMashDigUp;
    }

    private void Update()
    {
        //Block movement if dig or dig up
        if (Input.GetKey(KeyCode.Space))
        {
            Debug.DrawRay(transform.position, transform.forward, Color.black);
            Dig();
        }

        if (Input.GetKeyUp(KeyCode.Space))
        {
            timerDig = timerDigInit;
        }

        // Check if player is facing a grave
        bool isHit = Physics.Linecast(transform.position, transform.position + transform.forward * distGraveCreation,out RaycastHit hit, graveLayer);

        //Debug player vision
        Debug.DrawLine(transform.position, transform.position + transform.forward * distGraveCreation, Color.red);

        if (isHit)
        {
            Destroy(hit.collider.gameObject);
            corpse.SetActive(true);
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
        

        if (!corpse.activeSelf)
        {
            // can carry corpse
        }
    }


    /// <summary>
    /// Mash an input to call this method
    /// </summary>
    public void Dig()
    {
        // need to display QTE
        if (!corpse.activeSelf)
        {
            timerDig -= Time.deltaTime;
            if (timerDig <= 0)
            {
                // Create grave at a certain position
                if (corpse.TryGetComponent(out Corpse c))
                {
                    c.UpdateLocalisation();
                }

                // create grave in front of the player /!\ have to check the rotation in game
                GameObject graveCreated = Instantiate(graveToCreate, transform.position + transform.forward * distGraveCreation, Quaternion.identity);
                timerDig = timerDigInit;

                // player doesn't carry a corpse anymore
                corpse.SetActive(false);
            }
        }
    }

    /// <summary>
    /// Hold an input to call this method
    /// </summary>
    public void DigUp()
    {
        // Remove info & collider from the corpse to carry it 
        if (corpse.TryGetComponent(out Corpse c))
        {
            c.RemoveLocalisations();
        }
        // need to display QTE
        numberMashDigUp = numberMashDigUpInit;
    }
}
