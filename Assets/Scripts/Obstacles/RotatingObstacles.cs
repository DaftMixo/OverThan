using UnityEngine;

public class RotatingObstacles : Obstacle
{
    public override string Key { get; } = "Rotating";

    [SerializeField] private float _scalingTime = 1f;

    private bool _isShown;


    public override void Show()
    {
        gameObject.SetActive(true);
        _isShown = true;
    }

    public override void Hide()
    {
        if (!_isShown)
            return;
        gameObject.SetActive(false);
        _isShown = false;

    }

    private void FixedUpdate()
    {
        if (!_isShown)
            return;
        transform.Rotate(0, 0, -1.5f);
    }
}
