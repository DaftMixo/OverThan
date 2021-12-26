using System;
using System.Collections;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    [SerializeField] private UIController _uiController;
    
    [SerializeField] private GameObject ball;
    [SerializeField] private GameObject topZone;
    [SerializeField] private GameObject bottomZone;

    [SerializeField] private float pausePosition = -2.5f;
    
    [SerializeField] private GameObject[] _gameObjects = new GameObject[3];
    
    private InputHandler _inputHandler;
    private BallController _ballController;
    private bool _isActiveTop = true; //flag for trigger zone
    private GameState _gameState = GameState.Unknown;
    private bool _gameIsActive;
    private bool _jumpFlag;
    
    private int _gameScore = 0;
    
    public int GetScore => _gameScore;

    private void Awake()
    {
        if (Instance != null)
            return;
        Instance = this;
    }
    
    private void Start()
    {
        _inputHandler = GetComponent<InputHandler>();
        _inputHandler.touched += PlayGame;
        
        bottomZone.SetActive(false);
        topZone.SetActive(true);

        _ballController = ball.GetComponent<BallController>();
        
        _ballController.Init(_inputHandler);
        
        _ballController.switchTriggerZone += SwitchTriggerZone;
        _ballController.death += Death;
        _gameState = GameState.Menu;
        _uiController.SetUI(_gameState);
    }

    private void Update()
    {
        if (!_gameIsActive && ball.transform.position.y <= pausePosition && !_jumpFlag)
        {
            _jumpFlag = true;
            StartCoroutine(DelayedJump());
        }
    }

    private IEnumerator DelayedJump()
    {
        _ballController.Jump();
        yield return new WaitForSeconds(.2f);
        _jumpFlag = false;
    }

    public void StartGame()
    {
        _gameState = GameState.Game;
        _uiController.SetUI(_gameState);
        _gameObjects[0].SendMessage("Show");
        
        //TODO: Delay to PlayGame
    }

    private void PlayGame()
    {
        if (_gameState == GameState.Game)
            _gameIsActive = true;
    }

    private void Death()
    {
        _gameScore = 0;
        ball.transform.position = new Vector3(0, -.6f, 0);
    }

    private void SwitchTriggerZone()
    {
        _gameScore++;
        if (_isActiveTop)
        {
            _isActiveTop = false;
            bottomZone.SetActive(true);
            topZone.SetActive(false);
        }
        else
        {
            _isActiveTop = true;
            bottomZone.SetActive(false);
            topZone.SetActive(true);
        }
    }
}
