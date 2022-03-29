using System;
using Cysharp.Threading.Tasks;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public Action switchTriggerZone;
    public Action death;

    [SerializeField] private GameData.Model modelData;
    [SerializeField] private float _jumpForce = 2f;

    private bool _isInteractable = false;
    private bool _jumpFlag;

    private Rigidbody _rb;
    private bool _fixedJump = true;

    public bool IsUnlocked => modelData.IsUnlocked;
    public GameData.Model ModelData
    {
        get { return modelData; }
        set { modelData = value; }
    }

    public bool Interactable
    {
        get { return _isInteractable; }
        set { _isInteractable = value; }
    }

    public void SetFixedJump(bool value)
    {
        _fixedJump = value;
    }

    private void Awake()
    {
        _rb = GetComponent<Rigidbody>();
    }
    
    private void Update()
    {
        if (!GameManager.Instance.gameIsActive && transform.position.y <= GameManager.Instance.pausePosition && !_jumpFlag)
        {
            _jumpFlag = true;
            DelayedJump();
        }
        
        if (!GameManager.Instance.gameIsActive && transform.position.y > GameManager.Instance.pausePosition + 1 && _rb.velocity.y > 0)
            _rb.velocity = Vector3.zero;
    }
    private async void DelayedJump()
    {
        _rb.velocity = new Vector3(0, 4, 0);
        await UniTask.Delay(TimeSpan.FromSeconds(GameManager.Instance.jumpDealy));
        _jumpFlag = false;
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
        if (!_isInteractable)
            return;
        
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
