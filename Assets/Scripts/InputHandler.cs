using System;
using UnityEngine;

public class InputHandler : MonoBehaviour
{
    public Action touched;

    private void Update()
    {
#if UNITY_EDITOR || UNITY_EDITOR_64
        
        if (Input.GetMouseButtonDown(0))
        {
            touched.Invoke();
        }

#elif PLATFORM_ANDROID || UNITY_ANDROID

        if (Input.touchCount > 0)
        {
            if (Input.GetTouch(0).phase == TouchPhase.Began)
            {
                touched.Invoke();
            }
        }

#endif
    }
}
