using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Interactions;

public class Player : MonoBehaviour
{
    public GameObject graveToCreate;
    private GameObject graveCreated;

    [Tooltip("Add this GameObject as a child to the player")] public GameObject corpse;

    public float timerDig;

    private int numberMashDigUpInit;
    public int numberMashDigUp;
    
    public LayerMask graveLayer;
    public float distGraveCreation;

    [HideInInspector] public PlayerMovement playerMovement;

    public Sprite playerNotCarrying;
    public Sprite spriteCarry;
    public Carryable carriedObj;

    [Header("Debug")]
    public float radiusSphere = 5f;
    public IInteractable interactableObj;
    public LayerMask interactableLayer;
    public bool isCarrying = false;

    [Header("Hole")]
    public GameObject holePrefab;
    [SerializeField][Tooltip("The distance at which a hole is detected.")] private float raycastRadius;
    [SerializeField] private LayerMask holeRaycastMask;
    private Hole detectedHole;
    private Hole lastdetectedHole;
    


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

        playerMovement = GetComponent<PlayerMovement>();
         
        numberMashDigUpInit = numberMashDigUp;
    }

    private void Update()
    {
        //Debug player vision
        Debug.DrawLine(transform.position, transform.position + transform.forward * distGraveCreation, Color.red);

        if (!corpse.activeSelf)
        {
            // can carry corpse
        }

        // check interactables
        if (!isCarrying)
        {
            Collider[] interactables = Physics.OverlapSphere(transform.position, radiusSphere, interactableLayer);

            if (interactables.Length > 0)
            {
                float min = float.MaxValue;

                foreach (Collider col in interactables)
                {
                    if (col.gameObject.TryGetComponent(out IInteractable interactable))
                    {
                        float dist = Vector3.Distance(col.gameObject.transform.position, transform.position);
                        if (dist < min)
                        {
                            min = dist;
                            interactableObj = interactable;
                            if(interactableObj is Hole)
                            {
                                detectedHole = (Hole)interactableObj;
                                if (lastdetectedHole != null)
                                {
                                    StartCoroutine(GetComponent<PlayerVFX>().Outline(false, lastdetectedHole.GetComponent<SpriteRenderer>()));
                                }
                            }
                        }
                    }
                }
                
                if (interactableObj != null && interactableObj is Hole)
                {
                    lastdetectedHole = detectedHole;
                    StartCoroutine(GetComponent<PlayerVFX>().Outline(true, detectedHole.GetComponent<SpriteRenderer>()));
                }
                
            }
            else
            {
                // all the time -> NOPE
                if (lastdetectedHole != null)
                {
                    StartCoroutine(GetComponent<PlayerVFX>().Outline(false, lastdetectedHole.GetComponent<SpriteRenderer>()));
                    lastdetectedHole = null;
                }
                interactableObj = null;
                detectedHole = null;
            }
        }

        // Hole
        /*RaycastHit[] colliders = Physics.SphereCastAll(transform.position, raycastRadius, transform.forward, Mathf.Infinity, holeRaycastMask);
        if (colliders.Length > 0)
        {
            detectedHole = colliders[0].collider.GetComponent<Hole>();
            for (int i = 0; i < colliders.Length; i++)
            {
                float distanceCurrent = (colliders[i].transform.position - transform.position).magnitude;
                float distancePrevious = (detectedHole.transform.position - transform.position).magnitude;

                if (distanceCurrent > distancePrevious)
                {
                    detectedHole = colliders[i].collider.GetComponent<Hole>();
                }
            }
        }
        else
        {
            detectedHole = null;
        }*/
    }

    /// <summary>
    /// Hold an input to call this method
    /// </summary>
    public void Dig(InputAction.CallbackContext ctx)
    {
        if (corpse.activeSelf)
        {
            if (ctx.started)
            {
                playerMovement.canMove = false;
            }

            if (ctx.performed)
            {
                // need to display QTE
                Quaternion graveRot = Quaternion.FromToRotation(transform.position, transform.forward);
                graveRot.x = 0;
                graveRot.z = 0;

                Collider[] graveHit = Physics.OverlapBox(transform.position + transform.forward * distGraveCreation,
                    graveToCreate.transform.localScale / 2, graveRot, graveLayer);

                if (graveHit.Length == 0)
                {
                    if (ctx.duration >= timerDig)
                    {
                        // create grave in front of the player /!\ have to check the rotation in game
                        graveCreated = Instantiate(graveToCreate, transform.position + transform.forward * distGraveCreation, graveRot);

                        // Create grave at a certain position
                        if (graveCreated.TryGetComponent(out Grave g))
                        {
                            g.UpdateLocalisation();
                        }

                        // player doesn't carry a corpse anymore
                        corpse.SetActive(false);
                        playerMovement.canMove = true;
                    }
                }
            }

            if (ctx.canceled)
                playerMovement.canMove = true;
        }

    }

    /// <summary>
    /// Mash an input to call this method
    /// </summary>
    public void DigUp(InputAction.CallbackContext ctx)
    {
        if (!corpse.activeSelf)
        {
            bool isHit = Physics.Linecast(transform.position, transform.position + transform.forward * distGraveCreation, out RaycastHit hit,
                        graveLayer);

            if (isHit)
            {
                if (ctx.started)
                {
                    playerMovement.canMove = false;
                }

                if (ctx.performed && ctx.interaction is MultiTapInteraction)
                {
                    // Remove info & collider from the corpse to carry it 
                    if (graveCreated.TryGetComponent(out Grave c))
                    {
                        c.RemoveLocalisations();
                    }
                    // need to display QTE
                    corpse.SetActive(true);
                    Destroy(hit.collider.gameObject); 
                    playerMovement.canMove = true;
                }
            }

            if (ctx.canceled)
                playerMovement.canMove = true;

        }
       
    }

    public void InteractInput(InputAction.CallbackContext ctx)
    {
        if (ctx.performed)
        {
            if (interactableObj == null && carriedObj != null)
            {
                carriedObj.PutDown(this);
            } else if (interactableObj != null && isCarrying == false)
            {
                interactableObj.Interact(this);
            } else if (interactableObj == null && isCarrying == false)
            {
                Dig(1);
            }
        }
    }

    public void BurryInput(InputAction.CallbackContext ctx)
    {
        if (ctx.performed)
        {
            if(detectedHole != null)
            {
                detectedHole.Burry();
                detectedHole = null;
            }
        }
    }

    private void Dig(int modifier)
    {
        //Debug.Log(detectedHole);
        if (detectedHole)
        {
            detectedHole.SetHoleSize = modifier;
        }
        else
        {
            detectedHole = Instantiate(holePrefab, new Vector3(transform.position.x, transform.position.y - 0.5f,transform.position.z), 
                holePrefab.transform.rotation).GetComponent<Hole>();
        }
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.black;
        Gizmos.DrawWireCube(transform.position + transform.forward * distGraveCreation, graveToCreate.transform.localScale);
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(transform.position, radiusSphere);
    }
    
    
}
