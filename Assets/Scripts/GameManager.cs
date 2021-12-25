using System;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [SerializeField] private GameObject _camera;
    [SerializeField] private GameObject ball;

    [SerializeField] private GameObject topZone;
    [SerializeField] private GameObject bottomZone;

    private bool isActiveTop = true;
    private int gameScore = 0;
    
    [SerializeField] private GameObject[] _gameObjects = new GameObject[3];

    private void Start()
    {
        _gameObjects[0].SendMessage("Show");
        
        bottomZone.SetActive(false);
        topZone.SetActive(true);

        var ballController = ball.GetComponent<BallController>();
        ballController.switchTriggerZone += SwitchTriggerZone;
        ballController.death += Death;
    }

    private void Death()
    {
        gameScore = 0;
        ball.transform.position = new Vector3(0, -.6f, 0);
    }

    private void SwitchTriggerZone()
    {
        gameScore++;
        if (isActiveTop)
        {
            isActiveTop = false;
            bottomZone.SetActive(true);
            topZone.SetActive(false);
        }
        else
        {
            isActiveTop = true;
            bottomZone.SetActive(false);
            topZone.SetActive(true);
        }
    }
}
