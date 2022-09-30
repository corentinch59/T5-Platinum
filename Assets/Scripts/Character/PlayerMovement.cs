using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    [Header("PlayerControls")]
    [SerializeField] private float playerSpeed;

    private CharacterController controller;
    private Vector2 orientation;

    private Vector3 moveDir;
    private Vector2 move;

    private void Start()
    {
        controller = GetComponent<CharacterController>();
    }

    private void FixedUpdate()
    {
        moveDir = new Vector3(move.x, 0, move.y);
        controller.Move(moveDir * playerSpeed * Time.deltaTime);
    }

    public void OnMove(InputAction.CallbackContext ctx)
    {
        move = ctx.ReadValue<Vector2>();
        orientation = ctx.ReadValue<Vector2>();
        orientation = orientation.normalized;
    }
}
