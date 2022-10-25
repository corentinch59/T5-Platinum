using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerIncineration : MonoBehaviour
{
    [Header("Player Controls")]
    [SerializeField]
    [Tooltip("The player's movement speed.")]
    private float playerSpeed;

    [Header("Hole Raycast Handling")]
    [SerializeField][Tooltip("The distance at which a hole is detected.")] private float raycastRadius;
    [SerializeField] private LayerMask LeverRaycastMask;

    [HideInInspector] public Vector3 moveDir;
    [HideInInspector] public Vector3 playerVelocity;
    public bool canMove = true;

    private const float gravityValue = -9.81f;
    private CharacterController controller;
    private Vector2 orientationVect;
    private Vector2 move;
    public Vector2 getMove => move;
    private IncineratorLever detectedLever;
    private PlayerInput playerInput;

    private void Start()
    {
        controller = GetComponent<CharacterController>();
        playerInput = GetComponent<PlayerInput>();
    }

    private void Update()
    {
        Debug.DrawLine(transform.position, new Vector3(transform.position.x + orientationVect.x, transform.position.y, transform.position.z + orientationVect.y));

        RaycastHit[] colliders = Physics.SphereCastAll(transform.position, raycastRadius, transform.forward, Mathf.Infinity, LeverRaycastMask);

        if (colliders.Length > 0)
        {
            detectedLever = colliders[0].collider.GetComponent<IncineratorLever>();
            for (int i = 0; i < colliders.Length; i++)
            {
                float distanceCurrent = (colliders[i].transform.position - transform.position).magnitude;
                float distancePrevious = (detectedLever.transform.position - transform.position).magnitude;

                if (distanceCurrent > distancePrevious)
                {
                    detectedLever = colliders[i].collider.GetComponent<IncineratorLever>();
                }
            }
        }
        else
        {
            detectedLever = null;
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

        if (ctx.ReadValue<Vector2>().sqrMagnitude > (controller.minMoveDistance * controller.minMoveDistance))
        {
            orientationVect = ctx.ReadValue<Vector2>();
            if (Mathf.Abs(orientationVect.x) > Mathf.Abs(orientationVect.y))
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
        if (ctx.started)
        {
            detectedLever.LeverInteraction(this);
        }
    }

    public void EnableInput(string input)
    {
        playerInput.currentActionMap.FindAction(input).Enable();
    }

    public void DisableInput(string input)
    {
        playerInput.currentActionMap.FindAction(input).Disable();
    }
}
