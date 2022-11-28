using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
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

    private bool _isDashing = false;
    private float _t = 0f;
    public bool isDashing { get{ return _isDashing; } set { _isDashing = value; } }

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
        if (canMove)
        {
            moveDir = new Vector3(move.x, 0, move.y);
            controller.Move(moveDir * playerSpeed * Time.fixedDeltaTime);

            if(moveDir.magnitude > 0)
            {
                if (feedback == null)
                {
                    feedback = StartCoroutine(FeedBackPlayerMoves());
                }

                _t += Time.fixedDeltaTime;
                if (_t >= 0.75f)
                {
                    int randomInt = UnityEngine.Random.Range(1, 9);
                    SoundManager.instance.Play("FootStep" + randomInt);
                    _t = 0f;
                }

            }

            if (controller.isGrounded && playerVelocity.y < 0)
                playerVelocity.y = 0f;

            playerVelocity.y += controller.isGrounded ? 0f : gravityValue * Time.fixedDeltaTime;
            controller.Move(playerVelocity * Time.fixedDeltaTime);
        }
    }

    public IEnumerator FeedBackPlayerMoves()
    {
        if (!_isDashing)
        {
            transform.DOScale(new Vector3(1.3f, 0.7f), 0.15f);
            yield return new WaitForSeconds(0.15f);
            //transform.DOMoveY(transform.position.y, 0.15f);
            transform.DOScale(new Vector3(1f, 1f), 0.1f);
            //transform.DOScaleY(1f, 0.3f);
            yield return new WaitForSeconds(0.1f);
        }
        else yield return null;
        //transform.DOMoveY(transform.position.y + 1f, 0.15f);
        
        feedback = null;
    }

    public void OnMove(InputAction.CallbackContext ctx)
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

    public void ChangeInput(string inputActionMap)
    {
        playerInput.SwitchCurrentActionMap(inputActionMap);
        currentInput = inputActionMap;
    }

}
