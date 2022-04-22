using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class SlidingPanel : Obstacle
{
    public override string Key { get; } = "SlidingPanel";
    
    [SerializeField] private float _scalingTime = 1f;
    [SerializeField] private bool _rightStart;

    private Vector3 _rightPosition = Vector3.right * 3f;
    private bool _isShown;


    private void Start()
    {
        if(_rightStart)
            MoveRight();
        else
            MoveLeft();
    }

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

    private void MoveRight()
    {
        transform.DOMove(_rightPosition, _scalingTime).OnComplete(() => MoveLeft());
    }

    private void MoveLeft()
    {
        transform.DOMove(-_rightPosition, _scalingTime).OnComplete(() => MoveRight());
    }

    private void OnDestroy()
    {
        DOTween.KillAll();
    }
}
