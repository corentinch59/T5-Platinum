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

    [Header("Interaction Raycast Handling")]
    [SerializeField][Tooltip("The distance at which an interactible is detected.")] private float raycastRadius;
    [SerializeField] private LayerMask InteractableRaycastMask;

    [Header("Prefabs to spawn")]
    public GameObject holePrefab;

    private PlayerMovement playerMovement;
    public PlayerMovement getPlayerMovement => playerMovement;

    private Transform canvaQte;
    private Image qteFillImage;
    private IEnumerator myCoroutine;

    private GameObject interactableObject;
    private IRaycastBehavior raycastBehavior;
    private bool canInteract = true;

    private void Start()
    {
        //canvaQte = gameObject.transform.GetChild(0);
        //qteFillImage = canvaQte.GetComponentsInChildren<Image>()[1];
        //canvaQte.gameObject.SetActive(false);
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
        }
    }

    private IEnumerator InteractionCooldown(float time)
    {
        canInteract = false;
        yield return new WaitForSeconds(time);
        canInteract = true;
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
