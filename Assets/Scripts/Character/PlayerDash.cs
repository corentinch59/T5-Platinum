using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerDash : MonoBehaviour
{

    [SerializeField] private float forceDash;
    [SerializeField] private float dashTime;

    private Rigidbody _rigidbody;
    private CharacterController _controller;
    private PlayerMovement _playerMouvement;

    private Coroutine currentCoroutine = null;

    private void Start()
    {
        _rigidbody = GetComponent<Rigidbody>();
        _controller = GetComponent<CharacterController>();
        _playerMouvement = GetComponent<PlayerMovement>();
    }

    public void OnDash(InputAction.CallbackContext ctx)
    {
        if (ctx.performed)
        {
            if (currentCoroutine != null) return;
            currentCoroutine = StartCoroutine(DashCoroutine());
        }
    }

    private IEnumerator DashCoroutine()
    {
        _controller.enabled = false;

        //_rigidbody.AddForce(transform.forward, ForceMode.Impulse);
        Debug.Log(_playerMouvement.getMove.normalized);
        _rigidbody.velocity += new Vector3(_playerMouvement.getMove.normalized.x * forceDash, 0f,_playerMouvement.getMove.normalized.y * forceDash);
        yield return new WaitForSeconds(dashTime);

        _controller.enabled = true;
        currentCoroutine = null;
    }
}
