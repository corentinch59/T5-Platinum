using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class Player : MonoBehaviour
{
    [Header("Player Controls")]
    [SerializeField]
    [Tooltip("The height at which the hole will spawn in. (can be negative and is adviced to be)")]
    private float HeightOfHole;
    [SerializeField]
    [Tooltip("The time in seconds it takes for the QTE to finish.")]
    private float interactionDuration;
    [SerializeField]
    [Tooltip("How much time it takes to be able to press again to cancel or start digging again")]
    private float TimeBetweenInteractionInputs;
    [SerializeField]
    [Tooltip("How much times the player has to mash the button")]
    private int numberOfTaps;

    [Header("Interaction Raycast Handling")]
    [SerializeField][Tooltip("The distance at which an interactible is detected.")] private float raycastRadius;
    [SerializeField] private LayerMask InteractableRaycastMask;

    [Header("Prefabs to spawn")]
    public GameObject holePrefab;

    private PlayerMovement playerMovement;
    public PlayerMovement getPlayerMovement => playerMovement;

    private GameObject interactableObject;
    private IRaycastBehavior raycastBehavior;
    private bool isDigging = false;
    private int internalTaps = 0;

    private void Start()
    {
        playerMovement = gameObject.GetComponent<PlayerMovement>();
        raycastBehavior = new RaycastEmptyHand();
    }

    private void Update()
    {
        interactableObject = raycastBehavior.PerformRaycast(this, transform.position, raycastRadius, InteractableRaycastMask);
    }

    public void OnInteract(InputAction.CallbackContext ctx)
    {
        if (ctx.started)
        {
            if(interactableObject != null)
                interactableObject.GetComponent<IInteractable>().Interact(this);
            else if(isDigging)
            {
                ++internalTaps;
                if(numberOfTaps == internalTaps)
                {
                    isDigging = false;
                    internalTaps = 0;
                    //Dig & animation
                }
            }
            else
            {
                isDigging = true;
                internalTaps = 0;
            }
        }
    }

    public void EnableInput(string input)
    {
        playerMovement.GetPlayerInput.currentActionMap.FindAction(input).Enable();
    }

    public void DisableInput(string input)
    {
        playerMovement.GetPlayerInput.currentActionMap.FindAction(input).Disable();
    }


}
