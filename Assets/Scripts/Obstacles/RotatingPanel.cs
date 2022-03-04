using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.PlayerLoop;

public class RotatingPanel : Obstacle
{
    public override string Key { get; } = "RotatingPanel";

    [SerializeField] private float _scalingTime = 1f;
    
    private bool _isShown;
    private Vector3 _minimalScale = Vector3.zero;
    private Vector3 _normalScale = Vector3.one;

    private void Start()
    {
        transform.localScale = _minimalScale;
        gameObject.SetActive(false);
    }
    
    public override void Show()
    {
        gameObject.SetActive(true);
        _isShown = true;
        transform.DOScale(_normalScale, _scalingTime);
    }

    public override void Hide()
    {
        if (!_isShown)
            return;
        transform.DOScale(_minimalScale, _scalingTime).OnComplete(() => gameObject.SetActive(false));
        _isShown = false;
        
    }

    private void Update()
    {
        if(!_isShown)
            return;
        transform.Rotate(0, 0, -1);
    }
}
