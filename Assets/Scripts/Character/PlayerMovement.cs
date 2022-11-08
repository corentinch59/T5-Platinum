using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    [Header("Player Controls")]
    [SerializeField] private float playerSpeed;

    private Vector3 moveDir;
    private Vector3 playerVelocity;
    public bool canMove = true;

    private const float gravityValue = -9.81f;
    private CharacterController controller;
    public CharacterController getController => controller;
    private Vector2 orientationVect;
    public Vector2 getOrientation => orientationVect;
    private Vector2 move;
    public Vector2 getMove => move;
    private Vector2 rotate;
    [SerializeField] private Transform arrowOrientation;

    private string currentInput;

    private PlayerInput playerInput;
    public PlayerInput GetPlayerInput => playerInput;

    private void Awake()
    {
        DontDestroyOnLoad(gameObject);
        controller = GetComponent<CharacterController>();
        playerInput = GetComponent<PlayerInput>();
    }

    private void FixedUpdate()
    {
        if(transform.parent == null && canMove)
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

    public void OnMovePilote(InputAction.CallbackContext ctx)
    {
        if(canMove)
            OnMove(ctx);
    }

    public void OnMoveCoPilote(InputAction.CallbackContext ctx)
    {
        if (ctx.performed)
        {
            if (canMove)
            {
                rotate = ctx.ReadValue<Vector2>();
                float newAngle = Mathf.Atan2(transform.parent.position.y, transform.parent.position.x);
                if (rotate.x < 0)
                {
                    transform.RotateAround(transform.parent.position, Vector3.up, newAngle * rotate.x);
                    // goes left
                }
                else if (rotate.x > 0)
                {
                    // goes right
                    transform.RotateAround(transform.parent.position, Vector3.up, newAngle * rotate.x);
                }
            }
        }
    }

    public void ChangeInput(string inputActionMap)
    {
        playerInput.SwitchCurrentActionMap(inputActionMap);
        currentInput = inputActionMap;
    }

}
