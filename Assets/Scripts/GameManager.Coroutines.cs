using System.Collections;
using UnityEngine;

public partial class GameManager
{
    private IEnumerator DelayedJump()
    {
        _ballRigidbody.velocity = new Vector3(0, 4, 0);
        yield return new WaitForSeconds(_jumpDealy);
        _jumpFlag = false;
    }
}
