using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;
using DG.Tweening;

public class PlayerProto2 : MonoBehaviour
{
    [Header("Player Controls")]
    [SerializeField] [Tooltip("The player's movement speed.")]
    private float playerSpeed;
    [SerializeField] [Tooltip("The time in seconds it takes for the QTE to finish.")]
    private float interactionDuration;
    [SerializeField] [Tooltip("The height at which the hole will spawn in. (can be negative and is adviced to be)")]
    private float HeightOfHole;
    [SerializeField] [Tooltip("How much time it takes to be able to press again to cancel or start digging again")]
    private float TimeBetweenInteractionInputs;
    

    [Header("Hole Raycast Handling")]
    [SerializeField] [Tooltip("The distance at which a hole is detected.")] private float raycastRadius;
    [SerializeField] private LayerMask holeRaycastMask;

    [Header("Prefabs to spawn")]
    public GameObject holePrefab;

    [HideInInspector] public Vector3 moveDir;
    [HideInInspector] public Vector3 playerVelocity;
    public bool canMove = true;

    private const float gravityValue = -9.81f;
    private CharacterController controller;
    private Vector2 orientationVect;
    private Vector2 move;
    public Vector2 getMove => move;
    private Transform canvaQte;
    private Image qteFillImage;
    private IEnumerator myCoroutine;
    private Hole detectedHole;
    private PlayerInput playerInput;
    private bool canInteract = true;

    private void Start()
    {
        controller = GetComponent<CharacterController>();
        canvaQte = gameObject.transform.GetChild(0);
        qteFillImage = canvaQte.GetComponentsInChildren<Image>()[1];
        canvaQte.gameObject.SetActive(false);
        playerInput = GetComponent<PlayerInput>();
    }

    private void Update()
    {
        Debug.DrawLine(transform.position, new Vector3(transform.position.x + orientationVect.x, transform.position.y, transform.position.z + orientationVect.y));

        RaycastHit[] colliders = Physics.SphereCastAll(transform.position, raycastRadius, transform.forward, Mathf.Infinity, holeRaycastMask);
        if(colliders.Length > 0)
        {
            detectedHole = colliders[0].collider.GetComponent<Hole>();
            for (int i = 0; i < colliders.Length; i++)
            {
                float distanceCurrent = (colliders[i].transform.position - transform.position).magnitude;
                float distancePrevious = (detectedHole.transform.position - transform.position).magnitude;

                if(distanceCurrent > distancePrevious)
                {
                    detectedHole = colliders[i].collider.GetComponent<Hole>();
                }
            }
        }
        else
        {
            detectedHole = null;
        }
    }

    private void FixedUpdate()
    {
        moveDir = new Vector3(move.x, 0, move.y);
        controller.Move(moveDir * playerSpeed * Time.fixedDeltaTime);

        if (controller.isGrounded && playerVelocity.y < 0)
            playerVelocity.y = 0f;

        playerVelocity.y += controller.isGrounded ? 0f : gravityValue * Time.fixedDeltaTime;
        controller.Move(playerVelocity * Time.fixedDeltaTime);
    }

    public void OnMove(InputAction.CallbackContext ctx)
    {
        move = ctx.ReadValue<Vector2>();

        if(ctx.ReadValue<Vector2>().sqrMagnitude > (controller.minMoveDistance * controller.minMoveDistance))
        {
            orientationVect = ctx.ReadValue<Vector2>();
            if(Mathf.Abs(orientationVect.x) > Mathf.Abs(orientationVect.y))
            {
                orientationVect = new Vector2(orientationVect.x + orientationVect.y, 0);
            }
            else
            {
                orientationVect = new Vector2(0, orientationVect.y + orientationVect.x);
            }
            orientationVect.Normalize();
        }
    }

    public void OnInteract(InputAction.CallbackContext ctx)
    {
        // Start of QTE code
        if (ctx.started)
        {
            if (myCoroutine == null && canInteract) // Code of iteration 2
            {
                StartCoroutine(InteractionCooldown(TimeBetweenInteractionInputs)); // Code of Iteration 2
                myCoroutine = StartTimer(ctx);
                StartCoroutine(myCoroutine);
                playerInput.currentActionMap.FindAction("Move").Disable();
            }
            else if(myCoroutine != null && ctx.action.name == "Dash" && canInteract) // Code of iteration 2
            {
                StartCoroutine(InteractionCooldown(TimeBetweenInteractionInputs)); // Code of Iteration 2
                StopCoroutine(myCoroutine);
                canvaQte.gameObject.SetActive(false);
                playerInput.currentActionMap.FindAction("Move").Enable();
                myCoroutine = null; // Code of iteration 2
            }
        }
    }

    private IEnumerator StartTimer(InputAction.CallbackContext ctx)
    {
        canvaQte.gameObject.SetActive(true);
        qteFillImage.fillAmount = 0f;
        float elapsedTime = 0f;

        while (elapsedTime < interactionDuration)
        {
            qteFillImage.fillAmount = Mathf.Lerp(0f, 1f, (elapsedTime / interactionDuration));
            elapsedTime += Time.deltaTime;

            yield return null;
        }

        if(ctx.action.name == "Interact")
        {
            Dig(1); // Diggy diggy hole
        }
        if(ctx.action.name == "Dash")
        {
            Dig(-1);
        }

        canvaQte.gameObject.SetActive(false);
        playerInput.currentActionMap.FindAction("Move").Enable();
        myCoroutine = null; // Code of iteration 2
        yield return null;
    }

    //End of QTE code

    private void Dig(int modifier)
    {
        //Debug.Log(detectedHole);
        if (detectedHole)
        {
            detectedHole.SetHoleSize = modifier;
        }
        else
        {
            Instantiate(holePrefab, transform.position + new Vector3(orientationVect.x, HeightOfHole, orientationVect.y), Quaternion.identity);
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
        playerInput.currentActionMap.FindAction(input).Enable();
    }

    public void DisableInput(string input)
    {
        playerInput.currentActionMap.FindAction(input).Disable();
    }

    private void StartDigging()
    {

    }
}
