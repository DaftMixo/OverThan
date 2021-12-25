using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlidingPanel : MonoBehaviour, IObjController
{
    [SerializeField] private float newPos = -2f;

    private bool isShown;

    public void Show()
    {
        isShown = true;
    }

    public void Hide()
    {
        isShown = false;
    }

    private void Update()
    {
        if(!isShown)
            return;
        if (transform.position.x > 1.9 || transform.position.x < -1.9)
            newPos = -newPos;
            
        var nextPos = new Vector3(newPos, transform.position.y, transform.position.z);
        transform.position = Vector3.Lerp(transform.position,
                                            nextPos,
                                            Time.deltaTime);
    }
}
