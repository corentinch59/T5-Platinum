using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class PlayerCoco : MonoBehaviour
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
    public int getNumberOfTaps => numberOfTaps;

    [Header("Interaction Raycast Handling")]
    [SerializeField][Tooltip("The distance at which an interactible is detected.")] private float raycastRadius;
    [SerializeField] private LayerMask InteractableRaycastMask;

    [Header("Prefabs to spawn")]
    public GameObject holePrefab;

    private PlayerMovement playerMovement;
    public PlayerMovement getPlayerMovement => playerMovement;

    private GameObject interactableObject;
    private IRaycastBehavior raycastBehavior;
    private DiggingBehavior diggingBehavior;

    private void Start()
    {
        playerMovement = gameObject.GetComponent<PlayerMovement>();
        raycastBehavior = new RaycastEmptyHand();
        TransitionDigging(new StartDigging());
    }

    private void Update()
    {
        interactableObject = raycastBehavior.PerformRaycast(transform.position, raycastRadius, InteractableRaycastMask);
    }

    public void OnInteract(InputAction.CallbackContext ctx)
    {
        if (ctx.started)
        {
            diggingBehavior.PerformAction();
        }
    }

    public void OnDash(InputAction.CallbackContext ctx)
    {
        if (ctx.started)
        {
            diggingBehavior.CancelAction();
        }
    }

    public void EnableInput(string input)
    {
        playerMovement.getPlayerInput.currentActionMap.FindAction(input).Enable();
    }

    public void DisableInput(string input)
    {
        playerMovement.getPlayerInput.currentActionMap.FindAction(input).Disable();
    }

    public void TransitionDigging(DiggingBehavior newdiggingBehavior)
    {
        diggingBehavior = newdiggingBehavior;
        diggingBehavior.SetPlayer(this);
    }

    public void Dig(int modifier)
    {
        if (interactableObject != null && interactableObject.TryGetComponent(out Hole hole))
        {
            hole.SetHoleSize = modifier;
        }
        else if (interactableObject == null)
        {
            Instantiate(holePrefab, new Vector3(transform.position.x, transform.position.y - 0.5f, transform.position.z),
                holePrefab.transform.rotation);
        }
    }
}
