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

    private Coroutine currentCoroutine = null;

    private void Start()
    {
        _rigidbody = GetComponent<Rigidbody>();
        _controller = GetComponent<CharacterController>();
    }

    public void OnDash(InputAction.CallbackContext ctx)
    {
        
    }

    private IEnumerator DashCoroutine()
    {
        float timer = 0f;
        _controller.enabled = false;

        while (timer < dashTime)
        {
            _rigidbody.AddForce(transform.forward * forceDash);
            yield return null;
        }
        yield return null;  
    }
}
