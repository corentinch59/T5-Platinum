using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    [Header("Player Controls")]
    [SerializeField] private float playerSpeed;
    [SerializeField] private float digDistance;
    [SerializeField] private float interactionRange;

    [HideInInspector] public Vector3 moveDir;
    [HideInInspector] public Vector3 playerVelocity;
    public bool canMove = true;

    private const float gravityValue = -9.81f;
    private CharacterController controller;
    public Vector2 orientationVect;
    private Vector2 move;
    private float rotate;
    private IInteractable interactable;
    [SerializeField] private Transform arrowOrientation;

    private string currentInput;
    public string CurrentInput { get; set; }

    private PlayerInput playerInput;
    public PlayerInput PlayerInput => playerInput;


    private void Awake()
    {
        DontDestroyOnLoad(gameObject);
    }
    private void Start()
    {
        controller = GetComponent<CharacterController>();
        playerInput = GetComponent<PlayerInput>();
    }

    private void Update()
    {

    }

    private void FixedUpdate()
    {
        if(transform.parent == null)
        {
            moveDir = new Vector3(move.x, 0, move.y);
            controller.Move(moveDir * playerSpeed * Time.fixedDeltaTime);

            if (controller.isGrounded && playerVelocity.y < 0)
                playerVelocity.y = 0f;

            playerVelocity.y += controller.isGrounded ? 0f : gravityValue * Time.fixedDeltaTime;
            controller.Move(playerVelocity * Time.fixedDeltaTime);
        }
    }

    public void OnMove(InputAction.CallbackContext ctx)
    {
        if (canMove)
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

                // Show orientation to the player
                if (orientationVect.x < 0)
                {
                    arrowOrientation.eulerAngles = new Vector3(90, 0, -90);
                    arrowOrientation.localPosition = Vector3.left;
                } else if (orientationVect.x > 0)
                {
                    arrowOrientation.eulerAngles = new Vector3(90, 0, 90);
                    arrowOrientation.localPosition = Vector3.right;
                }

                if (orientationVect.y < 0)
                {
                    arrowOrientation.eulerAngles = new Vector3(90, 0, 0);
                    arrowOrientation.localPosition = Vector3.down;
                }
                else if (orientationVect.y > 0){
                    arrowOrientation.eulerAngles = new Vector3(90, 0, 180);
                    arrowOrientation.localPosition = Vector3.up;
                }
            }
        }
        else
        {
            move = Vector2.zero;
        }

    }

    public void OnInteract(InputAction.CallbackContext ctx)
    {
        Debug.Log("Interacted");
    }

    public void OnMovePilote(InputAction.CallbackContext ctx)
    {
        if(canMove)
            OnMove(ctx);
    }

    public void OnMoveCoPilote(InputAction.CallbackContext ctx)
    {
        if (canMove)
        {
            rotate = ctx.ReadValue<float>();

        }
        Debug.Log(rotate);
    }

    public void ChangeInput(string inputActionMap)
    {
        playerInput.SwitchCurrentActionMap(inputActionMap);
        CurrentInput = inputActionMap;
    }
}
