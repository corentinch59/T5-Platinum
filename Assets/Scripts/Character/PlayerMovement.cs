using DG.Tweening;
using System;
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

    [SerializeField] private SpriteRenderer spriteRenderer;
    public SpriteRenderer SpriteRenderer => spriteRenderer;
    private const float gravityValue = -9.81f;
    private CharacterController controller;
    public Vector2 orientationVect;
    private Vector2 move;
    private float rotate;
    private IInteractable interactable;
    [SerializeField] private Transform arrowOrientation;
    [SerializeField] private GameObject isPilote;
    public GameObject IsPilote => isPilote;

    [SerializeField] private GameObject isCoPilote;
    public GameObject IsCoPilote => isCoPilote;

    public Transform positionCopilote;
    private float moveUpDown;
    private Coroutine feedback;

    private string currentInput;
    public string CurrentInput => currentInput;

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
        IsPilote.SetActive(false);
        IsCoPilote.SetActive(false);
    }

    private void FixedUpdate()
    {
        if(transform.parent == null && positionCopilote == null)
        {
            if (currentInput != "Pilote")
            {
                moveDir = new Vector3(move.x, 0, move.y);
                controller.Move(moveDir * playerSpeed * Time.fixedDeltaTime);

                if(moveDir.magnitude > 0 && feedback == null)
                {
                    feedback = StartCoroutine(FeedBackPlayerMoves());
                }

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

        if (currentInput == "Co-Pilote" && canMove)
        {
            if (rotate < 0.5f)
            {
                transform.RotateAround(transform.parent.position, Vector3.up, 3 * rotate);
                IsCoPilote.transform.eulerAngles = new Vector3(90, 90, 0);
                spriteRenderer.transform.LookAt(Camera.main.transform);

                //transform.eulerAngles = new Vector3(0, 0, 0);

                //transform.eulerAngles = new Vector3(transform.eulerAngles.x, transform.eulerAngles.y, 0);
                //transform.LookAt(Camera.main.transform,);
                // goes left
            }
            else if (rotate > 0.5f)
            {
                // goes right
                transform.RotateAround(transform.parent.position, Vector3.up, 3 * rotate);
                IsCoPilote.transform.eulerAngles = new Vector3(90, 90, 0);
                spriteRenderer.transform.LookAt(Camera.main.transform);
                //transform.eulerAngles = new Vector3(0, 0, 0);

                //transform.eulerAngles = new Vector3(transform.eulerAngles.x, transform.eulerAngles.y, 0);
                //transform.LookAt(Camera.main.transform);
            }
        }
    }

    private IEnumerator FeedBackPlayerMoves()
    {
        transform.DOMoveY(2.8f, 0.1f);
        //transform.DOScaleY(1.3f, 0.3f);
        yield return new WaitForSeconds(0.1f);
        transform.DOMoveY(2.5f, 0.1f);
        //transform.DOScaleY(1f, 0.3f);
        yield return new WaitForSeconds(0.1f);
        feedback = null;
    }

    public void OnMove(InputAction.CallbackContext ctx)
    {
        if (canMove)
        {
            move = ctx.ReadValue<Vector2>();
            if (move.magnitude > 0)
            {
                //this.PlayerInput.GetDevice<Gamepad>().SetMotorSpeeds(0.1f, 0.05f);
            }
            else
            {
                //this.PlayerInput.GetDevice<Gamepad>().PauseHaptics();
            }

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

    public void OnMovePilote(InputAction.CallbackContext ctx)
    {
        if (canMove)
        {
            moveUpDown = ctx.ReadValue<float>();
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
