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
    [SerializeField] private ParticleSystem steps;
    public Transform positionCopilote;
    private float moveUpDown;

    private string currentInput;

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
        currentInput = playerInput.currentActionMap.name;
    }

    private void FixedUpdate()
    {
        Debug.Log("MoveDir : " + moveDir);
        if(transform.parent == null && positionCopilote == null)
        {
            if (currentInput != "Pilote")
            {
                moveDir = new Vector3(move.x, 0, move.y);
                controller.Move(moveDir * playerSpeed * Time.fixedDeltaTime);

                if (controller.isGrounded && playerVelocity.y < 0)
                    playerVelocity.y = 0f;

                playerVelocity.y += controller.isGrounded ? 0f : gravityValue * Time.fixedDeltaTime;
                controller.Move(playerVelocity * Time.fixedDeltaTime);
            }
                
        } 
        else if(transform.parent == null && positionCopilote != null)
        {
            if(currentInput == "Pilote")
            {
                moveDir = Vector3.zero;
                controller.Move(moveDir);

                if (moveUpDown > 0.5f)
                {
                    moveDir = (positionCopilote.position - transform.position).normalized;
                    controller.Move(moveDir * playerSpeed * Time.fixedDeltaTime);
                }
                else if (moveUpDown < -0.5f)
                {
                    moveDir = (-(positionCopilote.position - transform.position)).normalized;
                    controller.Move(moveDir * playerSpeed * Time.fixedDeltaTime);
                }
                else
                {
                    controller.Move(Vector3.zero);

                }
            }
        }

        if (currentInput == "Co-Pilote")
        {
            if (rotate < 0.5f)
            {
                transform.RotateAround(transform.parent.position, Vector3.up, 3 * rotate);
                // goes left
            }
            else if (rotate > 0.5f)
            {
                // goes right
                transform.RotateAround(transform.parent.position, Vector3.up, 3 * rotate);
            }
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
                    steps.gameObject.transform.localPosition = new Vector3(0.5f, 0f, -0.2f);
                    steps.gameObject.transform.eulerAngles = new Vector3(0, 90, 0);
                } else if (orientationVect.x > 0)
                {
                    arrowOrientation.eulerAngles = new Vector3(90, 0, 90);
                    arrowOrientation.localPosition = Vector3.right;
                    steps.gameObject.transform.localPosition = new Vector3(-0.5f, 0f, -0.2f);
                    steps.gameObject.transform.eulerAngles = new Vector3(0, -90, 0);
                }

                if (orientationVect.y < 0)
                {
                    arrowOrientation.eulerAngles = new Vector3(90, 0, 0);
                    arrowOrientation.localPosition = Vector3.down;
                    steps.gameObject.transform.localPosition = new Vector3(0, 0.5f, -0.2f);
                    steps.gameObject.transform.eulerAngles = new Vector3(0, 0, 0);
                }
                else if (orientationVect.y > 0){
                    arrowOrientation.eulerAngles = new Vector3(90, 0, 180);
                    arrowOrientation.localPosition = Vector3.up;
                    steps.gameObject.transform.localPosition = new Vector3(0, -0.5f, -0.2f);
                    steps.gameObject.transform.eulerAngles = new Vector3(0, 180, 0);
                }
            }
        }
        else
        {
            move = Vector2.zero;
        }

    }

    public void OnMovePilote(InputAction.CallbackContext ctx)
    {
        if (canMove)
        {
            moveUpDown = ctx.ReadValue<float>();
            Debug.Log("MoveUpDown : " + moveUpDown);
            /*if(moveUpDown > 0.5f)
            {
                moveDir = positionCopilote.position - transform.position;
                moveDir.Normalize();
            } else if(moveUpDown < 0.5f)
            {
                moveDir = -(positionCopilote.position - transform.position);
                moveDir.Normalize();
            }*/
        }
    }

    public void OnMoveCoPilote(InputAction.CallbackContext ctx)
    {
        if (canMove)
        {
            rotate = ctx.ReadValue<float>();
        }

    }

    public void ChangeInput(string inputActionMap)
    {
        playerInput.SwitchCurrentActionMap(inputActionMap);
        currentInput = inputActionMap;
    }
}
