using System;
using UnityEngine;

public class BallController : MonoBehaviour
{
    public Action switchTriggerZone;
    public Action death;
    
    [SerializeField] private float jumpForce = 2f;
    
    private Rigidbody _rb;
    private InputHandler _inputHandler;

    private void Start()
    {
        _rb = GetComponent<Rigidbody>();
    }
    public void Init(InputHandler inputHandler)
    {
        _inputHandler = inputHandler;
        _inputHandler.touched += Jump;
    }
    private void OnDestroy()
    {
        if (_inputHandler != null) _inputHandler.touched -= Jump;
    }
    
    public void Jump()
    {
        _rb.AddForce(Vector3.up * jumpForce);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("TriggerZone"))
        {
            switchTriggerZone.Invoke();
        }

        if (other.CompareTag("DeathObject"))
        {
            death.Invoke();
        }
    }
}
