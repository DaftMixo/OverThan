using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlidingPanel : MonoBehaviour, IObstacleController
{
    [SerializeField] private float newPos = -2f;

    private bool isShown;

    public void Show()
    {
        isShown = true;
    }

    public void Hide()
    {
        if (isShown)
            StartCoroutine(HidePanel());
        isShown = false;
    }
    
    private IEnumerator HidePanel()
    {
        var outPose = new Vector3(-7, 0, 0);
        while (transform.localPosition != outPose)
        {
            transform.localPosition = Vector3.Lerp(transform.position,
                outPose, 
                Time.deltaTime);
            
            if (transform.position.x < -6)
                break;
            
            yield return null;
        }
        transform.position = new Vector3(7, 0, 0);
        transform.rotation = new Quaternion(0, 0, 0, 0);
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
