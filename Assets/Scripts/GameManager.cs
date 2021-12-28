using UnityEngine;
using Random = UnityEngine.Random;

public partial class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    [Header("Controllers")]
    [SerializeField] private UIController _uiController;
    [SerializeField] private AudioFX _audioFx;
    [SerializeField] private RewardedAdsButton _ads;
    
    [Header("Game objects")]
    [SerializeField] private GameObject ball;
    [SerializeField] private GameObject topZone;
    [SerializeField] private GameObject bottomZone;
    
    [SerializeField] private GameObject[] obstacles = new GameObject[3];

    private static string s_scoreKey = "MaxScore";
    
    private float pausePosition = 0f;
    private InputHandler _inputHandler;
    private BallController _ballController;
    private Rigidbody _ballRigidbody;
    private bool _isActiveTop = true; //flag for trigger zone
    private GameState _gameState = GameState.Unknown;
    private bool _gameIsActive;
    private bool _jumpFlag;
    private bool _topStart;

    private int _maxScore = 0;
    private int _gameScore = 0;
    private float _jumpDealy = .25f;

    private void Awake()
    {
        if (Instance != null)
            return;
        Instance = this;
    }
    
    private void Start()
    {
        _ballRigidbody = ball.GetComponent<Rigidbody>();
        _audioFx = GetComponent<AudioFX>();
        _inputHandler = GetComponent<InputHandler>();
        _ballController = ball.GetComponent<BallController>();
        
        _maxScore = PlayerPrefsSafe.GetInt(s_scoreKey, 0);
        
        _ballController.switchTriggerZone += SwitchTriggerZone;
        _ballController.death += Death;
        _inputHandler.touched += PlayGame;
        _uiController.OnButtonClick += PlayButtonSound;
        _uiController.SetMenuScore(_maxScore);
        
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
        
        if (!_gameIsActive && ball.transform.position.y > pausePosition + 1 && _ballRigidbody.velocity.y > 0)
            _ballRigidbody.velocity = Vector3.zero;
    }

    public void StartGame()
    {
        _inputHandler.touched += _ballController.Jump;
        _gameState = GameState.Game;
        _uiController.SetUI(_gameState);
        obstacles[0].SendMessage("Show");

        _topStart = Random.Range(0, 2) == 1;
        _isActiveTop = !_topStart;

        if (_topStart)
        {
            pausePosition = 2.5f;
            _jumpDealy = .4f;
            
            bottomZone.SetActive(true);
            topZone.SetActive(false);
        }
        else
        {
            pausePosition = -2.5f;
            _jumpDealy = .25f;
            
            bottomZone.SetActive(false);
            topZone.SetActive(true);
        }
    }

    public void PauseGame()
    {
        _gameState = GameState.PauseMenu;
        _uiController.SetUI(_gameState);
        _gameIsActive = false;
        _jumpDealy = .25f;
        _ballController.SetFixedJump(true);
        
        if (!_isActiveTop)
        {
            pausePosition = 2.5f;
        }
        else
        {
            pausePosition = -2.5f;
        }
        _inputHandler.touched -= _ballController.Jump;
    }

    public void ContinueGame()
    {
        _gameState = GameState.Game;
        _uiController.SetUI(_gameState);
        _inputHandler.touched += _ballController.Jump;
    }
    
    public void ContinueGameOnDeath()
    {
        StartGame();
        _uiController.SetRewardedContinue(false);
    }

    public void ExitToMenu()
    {
        _gameState = GameState.Menu;
        _uiController.SetUI(_gameState);
        _gameIsActive = false;
        pausePosition = 0;
        _gameScore = 0;

        _inputHandler.touched -= _ballController.Jump;

        foreach (var item in obstacles)
        {
            item.SendMessage("Hide");
        }
    }

    public void RestartGame()
    {
        _gameScore = 0;
        StartGame();
    }

    private void PlayGame()
    {
        if (_gameState == GameState.Game)
            _gameIsActive = true;
    }

    private void Death()
    {
        _gameState = GameState.Death;
        _uiController.SetUI(_gameState);
        _uiController.SetDeathScreenScore(_gameScore);
        _gameIsActive = false;
        pausePosition = 0;
        _ads.LoadAd();
        
        if (_maxScore < _gameScore)
            _maxScore = _gameScore;

        PlayerPrefsSafe.SetInt(s_scoreKey, _maxScore);
        _uiController.SetMenuScore(_maxScore);

        _ballController.SetFixedJump(true);
        _inputHandler.touched -= _ballController.Jump;

        foreach (var item in obstacles)
        {
            item.SendMessage("Hide");
        }
    }

    private void PlayButtonSound()
    {
        _audioFx.PlayButtonSound();
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
