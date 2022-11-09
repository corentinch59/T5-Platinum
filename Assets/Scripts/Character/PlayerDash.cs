using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerDash : MonoBehaviour
{

    [SerializeField] private float forceDash;
    //[SerializeField] private float dashTime;

    private Rigidbody _rigidbody;
    private CharacterController _controller;

    private Coroutine currentCoroutine = null;

    private void Start()
    {
        _rigidbody = GetComponent<Rigidbody>();
        _controller = GetComponent<CharacterController>();
    }

    public void OnDash(InputAction.CallbackContext ctx)
    {

        if (currentCoroutine != null) return;
        currentCoroutine = StartCoroutine(DashCoroutine());
    }

    private IEnumerator DashCoroutine()
    {
        _controller.enabled = false;

        //_rigidbody.AddForce(transform.forward, ForceMode.Impulse);
        _rigidbody.velocity += transform.forward * forceDash;
        yield return new WaitForSeconds(1f);

        currentCoroutine = null;
    }
}
