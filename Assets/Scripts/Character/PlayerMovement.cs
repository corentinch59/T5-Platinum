using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    [Header("Player Movements")]
    [SerializeField] private float playerSpeed;

    [Header("Sprites")]
    [SerializeField] private SpriteRenderer spriteRenderer;
    public SpriteRenderer SpriteRenderer => spriteRenderer;

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
    private string currentInput;
    public string CurrentInput => currentInput;
    private PlayerInput playerInput;
    public PlayerInput getPlayerInput => playerInput;
    private Coroutine feedback;

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
            }
        }
        else
        {
            move = Vector2.zero;
        }

    }

    public void ChangeInput(string inputActionMap)
    {
        playerInput.SwitchCurrentActionMap(inputActionMap);
        currentInput = inputActionMap;
    }

}
