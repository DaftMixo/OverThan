using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class DoubleSlidingPanel : Obstacle
{
    public override string Key { get; } = "DoubleSlidingPanel";
    
    [SerializeField] private float _scalingTime = 1f;
    [SerializeField] private Transform topPanel;
    [SerializeField] private Transform bottomPanel;

    private Vector3 _rightPosition = Vector3.right * 3f;
    private bool _isShown;


    private void Start()
    {
        MoveRight();
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
        Sequence sequence = DOTween.Sequence();
        sequence.Append(topPanel.transform.DOMove(_rightPosition + Vector3.up, _scalingTime));
        sequence.Append(bottomPanel.transform.DOMove(-_rightPosition + Vector3.down, _scalingTime));
        sequence.OnComplete(MoveLeft);
    }

    private void MoveLeft()
    {
        Sequence sequence = DOTween.Sequence();
        sequence.Append(topPanel.transform.DOMove(-_rightPosition + Vector3.up, _scalingTime));
        sequence.Append(bottomPanel.transform.DOMove(_rightPosition + Vector3.down, _scalingTime));
        sequence.OnComplete(MoveRight);
    }

    private void OnDestroy()
    {
        DOTween.KillAll();
    }
}
