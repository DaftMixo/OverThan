using UnityEngine;

public abstract class Obstacle : MonoBehaviour, IObstacleController
{
    public virtual void Show()
    {
        Debug.LogError("Override Show method in obstacles");
    }

    public virtual void Hide()
    {
        Debug.LogError("Override Hide method in obstacles");
    }
}
