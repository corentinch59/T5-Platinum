using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Interactions;

public class Player : MonoBehaviour
{
    public LayerMask graveLayer;
    public float distGraveCreation;

    private PlayerMovement playerMovement;
    public PlayerMovement getPlayerMovement => playerMovement;

    public Sprite playerNotCarrying;
    public Sprite spriteCarry;
    private Carryable carriedObj;
    public Carryable CarriedObj
    {
        get { return carriedObj; }
        set { carriedObj = value; }
    }

    private IRaycastBehavior raycastBehavior;
    [SerializeField] private GameObject objectFound;
    [SerializeField] private GameObject lastObjectFound;

    [Header("Debug")]
    public float radiusSphere = 5f;
    public LayerMask interactableLayer;

    [Header("Hole Section")]
    [SerializeField] [Tooltip("The distance at which a hole is detected.")] private float raycastRadius;
    public GameObject holePrefab;
    [SerializeField] private float distanceSpawnHole = 2f;

    [SerializeField] private int numberOfTaps;
    public int getNumbersOfTaps => numberOfTaps;


    private PlayerVFX vfx;
    
    private void Start() 
    {
        playerMovement = GetComponent<PlayerMovement>();
        raycastBehavior = new RaycastEmptyHand();
        vfx = GetComponent<PlayerVFX>();
    }

    private void Update()
    {
        //Debug player vision
        Debug.DrawLine(transform.position, transform.position + transform.forward * distGraveCreation, Color.red);

        objectFound = raycastBehavior.PerformRaycast(transform.position, raycastRadius, interactableLayer);
        //Test Outline
        if (objectFound != null && objectFound != lastObjectFound)
        {
            CallOutline(true, objectFound.GetComponent<SpriteRenderer>());
            if (lastObjectFound != null)
            {
                CallOutline(false,lastObjectFound.GetComponent<SpriteRenderer>());
            }
            lastObjectFound = objectFound;
        }
        else if (objectFound == null && lastObjectFound != null)
        {
            CallOutline(false,lastObjectFound.GetComponent<SpriteRenderer>());
            lastObjectFound = null;
        }
 
    }

    public void InteractInput(InputAction.CallbackContext ctx)
    {
        if (ctx.performed)
        {
            if(objectFound != null)
            {
                Debug.Log("Object found : " + objectFound.name);
                if (objectFound.TryGetComponent(out Hole hole))
                {
                    hole.Interact(this);
                }
                else if (objectFound.TryGetComponent(out GriefPNJInteractable griefPnj) && carriedObj == null)
                {
                    griefPnj.Interact(this);
                }
                else if (objectFound.TryGetComponent(out Corpse corpse))
                {
                    if(carriedObj == null)
                    {
                        corpse.Interact(this);
                    }
                    else if(carriedObj != null && carriedObj.TryGetComponent(out GriefPNJInteractable pnj))
                    {
                        pnj.CheckLocationWanted(this);
                    }
                }
            }
            else if (objectFound == null && carriedObj == null)
            {
                Dig(1);
            }
        }
    }

    public void DashCancelInput(InputAction.CallbackContext ctx)
    {
        if (ctx.performed)
        {
            if(carriedObj != null)
            {
                if (carriedObj.TryGetComponent(out Corpse corpse))
                {
                    corpse.PutDown(this);
                }
                else if (carriedObj.TryGetComponent(out GriefPNJInteractable griefPnj))
                {
                    griefPnj.PutDown(this);
                }
            }
            else if (objectFound != null && objectFound.TryGetComponent(out Hole hole) && carriedObj == null)
            {
                Dig(-1);
            }
            else
            {
                // player dash
            }
        }
    }

    public void Dig(int modifier)
    {
        if (objectFound != null && objectFound.TryGetComponent(out Hole hole))
        {
            hole.SetHoleSize = modifier;
        }
        else if(objectFound == null)
        {
            // Instantiate Hole to where the player is looking
            Instantiate(holePrefab, new Vector3(transform.position.x + getPlayerMovement.getOrientation.x * distanceSpawnHole, transform.position.y - 1f, 
                transform.position.z + getPlayerMovement.getOrientation.y * distanceSpawnHole), holePrefab.transform.rotation);
        }
    }

    private void CallOutline(bool active, SpriteRenderer renderer)
    {
        StartCoroutine(vfx.Outline(active, renderer));
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.black;
        Gizmos.DrawWireSphere(transform.position, radiusSphere);
    }
}