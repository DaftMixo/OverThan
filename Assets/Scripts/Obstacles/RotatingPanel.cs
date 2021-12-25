using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotatingPanel : MonoBehaviour, IObjController
{
    
    private bool isShown;
    
    public void Show()
    {
        isShown = true;
        StartCoroutine(ShowPanel());
    }

    private IEnumerator ShowPanel()
    {
        while (transform.localPosition != Vector3.zero)
        {
            transform.localPosition = Vector3.Lerp(transform.position,
                Vector3.zero, 
                Time.deltaTime);

            if (transform.localPosition.x < 0.01)
                break;
            
            yield return null;
        }
        Hide();
    }

    public void Hide()
    {
        isShown = false;
        StartCoroutine(HidePanel());
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
        transform.Rotate(0, 0, -1);
    }
}
