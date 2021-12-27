using System;
using UnityEngine;

public class BallController : MonoBehaviour
{
    public Action switchTriggerZone;
    public Action death;

    public void SetFixedJump(bool value)
    {
        _fixedJump = value;
    }
    
    [SerializeField] private float _jumpForce = 2f;
    
    private Rigidbody _rb;
    private bool _fixedJump = true;

    private void Start()
    {
        _rb = GetComponent<Rigidbody>();
    }

    public void Jump()
    {
        Jump(1);
    }
    
    public void Jump(float jumpForce)
    {
        if (_fixedJump)
        {
            _rb.velocity = new Vector3(0, 4, 0);
            _fixedJump = false;
        }
        else
        {
            _rb.AddForce(Vector3.up * jumpForce * _jumpForce);
        }
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
