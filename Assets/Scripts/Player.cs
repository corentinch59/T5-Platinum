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
    private GameObject objectFound;

    [Header("Debug")]
    public float radiusSphere = 5f;
    public IInteractable interactableObj;
    public LayerMask interactableLayer;
    public bool isCarrying = false;

    [Header("Hole Section")]
    public GameObject holePrefab;
    [SerializeField][Tooltip("The distance at which a hole is detected.")] private float raycastRadius;

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
        raycastBehavior = new RaycastEmptyHand();
    }

    private void Update()
    {
        //Debug player vision
        Debug.DrawLine(transform.position, transform.position + transform.forward * distGraveCreation, Color.red);

        if (!corpse.activeSelf)
        {
            // can carry corpse
        }

        objectFound = raycastBehavior.PerformRaycast(transform.position, raycastRadius, interactableLayer);
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

    private void Dig(int modifier)
    {
        //if (detectedHole)
        //{
        //    detectedHole.SetHoleSize = modifier;
        //}
        //else
        //{
        //    detectedHole = Instantiate(holePrefab, new Vector3(transform.position.x, transform.position.y - 0.5f,transform.position.z), 
        //        holePrefab.transform.rotation).GetComponent<Hole>();
        //}
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.black;
        Gizmos.DrawWireCube(transform.position + transform.forward * distGraveCreation, graveToCreate.transform.localScale);
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(transform.position, radiusSphere);
    }
}
