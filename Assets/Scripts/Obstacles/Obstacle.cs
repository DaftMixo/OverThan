using UnityEngine;

public abstract class Obstacle : MonoBehaviour
{
    public abstract string Key { get; }
    public abstract void Show();
    public abstract void Hide();
}
