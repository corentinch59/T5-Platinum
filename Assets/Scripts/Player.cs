using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Interactions;

public class Player : MonoBehaviour
{
    [SerializeField] private float distanceSpawnHole = 2f;

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
    [SerializeField] private int numberOfTaps;
    public int getNumbersOfTaps => numberOfTaps;

    private DiggingBehavior diggingBehavior;

    private PlayerVFX vfx;

    private void Start() 
    {
        playerMovement = GetComponent<PlayerMovement>();
        raycastBehavior = new RaycastEmptyHand();
        vfx = GetComponent<PlayerVFX>();
        TransitionDigging(new StartDigging());
    }

    private void Update()
    {
        //Debug player vision
        Debug.DrawLine(transform.position, transform.position + transform.forward * distGraveCreation, Color.red);

        if(carriedObj != null)
        {
            objectFound = raycastBehavior.PerformRaycast(transform.position, raycastRadius, interactableLayer, "Hole");
        }
        else
        {
            objectFound = raycastBehavior.PerformRaycast(transform.position, raycastRadius, interactableLayer);
        }

        //Test Outline
        if (objectFound != null && objectFound != lastObjectFound)
        {
            if(objectFound.GetComponent<SpriteRenderer>() != null)
            {
                Outline(objectFound, true);
                Outline(lastObjectFound, false);
                lastObjectFound = objectFound;
            }
        }
        else if (objectFound == null && lastObjectFound != null)
        {
            Outline(lastObjectFound, false);
            lastObjectFound = null;
        }
    }

    private void Outline(GameObject obj, bool active)
    {
        if(obj != null)
        {
            CallOutline(active, obj.GetComponent<SpriteRenderer>());
        }
    }

    public void InteractInput(InputAction.CallbackContext ctx)
    {
        if (ctx.performed)
        {
            if(objectFound != null)
            {
                //Debug.Log("Object found : " + objectFound.name);
                if (objectFound.TryGetComponent(out Hole hole))
                {
                    hole.Interact(this);
                }
                else if (objectFound.TryGetComponent(out GriefPNJInteractable griefPnj) && carriedObj == null)
                {
                    objectFound.layer = 0; // <- not interactable for now
                    griefPnj.Interact(this);
                    int randomint = UnityEngine.Random.Range(1, 3);
                    SoundManager.instance.Play("Pickup_Npc" + randomint);
                }
                else if (objectFound.TryGetComponent(out Corpse corpse) && corpse.BigCorpse == null)
                {
                    objectFound.layer = 0; // <- not interactable for now
                    if (carriedObj == null && corpse.IsInteractable)
                    {
                        corpse.Interact(this);
                        int randomint = UnityEngine.Random.Range(1, 3);
                        SoundManager.instance.Play("Grab_Corpse" + randomint);
                    }
                    else if (carriedObj != null && carriedObj.TryGetComponent(out GriefPNJInteractable pnj))
                    {
                        pnj.CheckLocationWanted(this);
                    }
                }
                else if (objectFound.TryGetComponent(out BigCorpse bigcorpse) && carriedObj == null)
                {
                    Debug.Log("BigCorpse Interact");
                    bigcorpse.Interact(this);
                    carriedObj = bigcorpse.gameObject.GetComponent<Corpse>();
                    carriedObj.Interact(this);
                }
            }
            else if (objectFound == null && carriedObj == null)
            {
                diggingBehavior.PerformAction();
                int randomint = UnityEngine.Random.Range(1, 4);
                SoundManager.instance.Play("Dig" + randomint);
            }
        }
    }

    public void DashCancelInput(InputAction.CallbackContext ctx)
    {
        if (ctx.performed)
        {
            if(carriedObj != null)
            {
                carriedObj.gameObject.layer = 7; // <- Interactable layer 
                if (carriedObj.TryGetComponent(out Corpse corpse))
                {
                    if(carriedObj.TryGetComponent(out BigCorpse bc))
                    {
                        carriedObj = null;
                        bc.Interact(this);
                    }
                    else
                    {
                        carriedObj.gameObject.layer = 7; // <- Interactable layer 
                        int randomint = UnityEngine.Random.Range(1, 3);
                        SoundManager.instance.Play("Drop_Corpse" + randomint);
                        corpse.PutDown(this);
                    }
                }
                else if (carriedObj.TryGetComponent(out GriefPNJInteractable griefPnj))
                {
                    carriedObj.gameObject.layer = 7; // <- Interactable layer 
                    griefPnj.PutDown(this);
                }
            }
            else if (objectFound != null && objectFound.TryGetComponent(out Hole hole) && carriedObj == null)
            {
                Dig(-1);
            }
            else
            {
                diggingBehavior.CancelAction();
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

    public void TransitionDigging(DiggingBehavior newdiggingBehavior)
    {
        diggingBehavior = newdiggingBehavior;
        diggingBehavior.SetPlayer(this);
    }

    public void EnableInput(string input)
    {
        playerMovement.getPlayerInput.currentActionMap.FindAction(input).Enable();
    }

    public void DisableInput(string input)
    {
        playerMovement.getPlayerInput.currentActionMap.FindAction(input).Disable();
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.black;
        Gizmos.DrawWireSphere(transform.position, radiusSphere);
    }
}