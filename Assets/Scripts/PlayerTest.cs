using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerTest : MonoBehaviour
{
    public GameObject graveToCreate;
    private GameObject graveCreated;

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
        // Check if player is facing a grave
        bool isHit = Physics.Linecast(transform.position, transform.position + transform.forward * distGraveCreation,out RaycastHit hit,
            graveLayer);

        //Debug player vision
        Debug.DrawLine(transform.position, transform.position + transform.forward * distGraveCreation, Color.red);


        //Block movement if dig or dig up
        if (Input.GetKey(KeyCode.Space))
        {
            Quaternion graveRot = Quaternion.FromToRotation(transform.position, transform.forward);
            graveRot.x = 0;
            graveRot.z = 0;
            Collider[] graveHit = Physics.OverlapBox(transform.position + transform.forward * distGraveCreation,
                graveToCreate.transform.localScale / 2, graveRot, graveLayer);
            if (graveHit.Length == 0)
            {
                Dig();
            }
        }

        if (Input.GetKeyUp(KeyCode.Space))
        {
            timerDig = timerDigInit;
        }


        if (isHit)
        {
            if (Input.GetKeyDown(KeyCode.E))
            {
                numberMashDigUp--;
                if (numberMashDigUp <= 0)
                {
                    DigUp(hit.collider.gameObject);
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
        if (corpse.activeSelf)
        {
            timerDig -= Time.deltaTime;
            if (timerDig <= 0)
            {
                // create grave in front of the player /!\ have to check the rotation in game
                Quaternion graveRot = Quaternion.FromToRotation(transform.position, transform.forward);
                graveRot.x = 0;
                graveRot.z = 0;

                graveCreated = Instantiate(graveToCreate, transform.position + transform.forward * distGraveCreation, graveRot);
                
                //if(graveCreated.GetComponent<Collider>().bounds)

                // Create grave at a certain position
                if (graveCreated.TryGetComponent(out Grave g))
                {
                    g.UpdateLocalisation();
                }

                timerDig = timerDigInit;

                // player doesn't carry a corpse anymore
                corpse.SetActive(false);
            }
        }
    }

    /// <summary>
    /// Hold an input to call this method
    /// </summary>
    public void DigUp(GameObject graveToDestroy)
    {
        // Remove info & collider from the corpse to carry it 
        if (graveCreated.TryGetComponent(out Grave c))
        {
            c.RemoveLocalisations();
        }
        // need to display QTE
        numberMashDigUp = numberMashDigUpInit;
        corpse.SetActive(true);
        Destroy(graveToDestroy);
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.DrawWireCube(transform.position + transform.forward * distGraveCreation, graveToCreate.transform.localScale);
        Gizmos.color = Color.black;
    }
}
