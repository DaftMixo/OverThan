using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class SlidingPanel : Obstacle
{
    [SerializeField] private float _scalingTime = 1f;

    private Vector3 _rightPosition = Vector3.right * 3f;
    private Vector3 _minimalScale = Vector3.zero;
    private Vector3 _normalScale = Vector3.one;
    private bool _isShown;


    private void Start()
    {
        transform.localScale = _minimalScale;
        gameObject.SetActive(false);
    }

    public override void Show()
    {
        gameObject.SetActive(true);
        transform.DOScale(_normalScale, _scalingTime).OnComplete(() =>
        {
            _isShown = true;
            MoveRight();
        });
    }

    public override void Hide()
    {
        if (!_isShown)
            return;
        
        transform.DOScale(_minimalScale, _scalingTime).OnComplete(() => gameObject.SetActive(false));
        _isShown = false;
        
    }

    private void MoveRight()
    {
        transform.DOMove(_rightPosition, _scalingTime).OnComplete(() => MoveLeft());
    }

    private void MoveLeft()
    {
        transform.DOMove(-_rightPosition, _scalingTime).OnComplete(() => MoveRight());
    }

}
