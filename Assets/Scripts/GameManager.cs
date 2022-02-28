using System;
using Cysharp.Threading.Tasks;
using UnityEngine;
using Random = UnityEngine.Random;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    public event Action<GameData> OnDataUpdate;

    [Header("Controllers")]
    [SerializeField] private UIController _uiController;
    [SerializeField] private AudioFX _audioFx;
    [SerializeField] private RewardedAdsButton _ads;
    
    [Header("Game objects")]
    [SerializeField] private GameObject player;
    [SerializeField] private GameObject topZone;
    [SerializeField] private GameObject bottomZone;
    
    [Header("Configs")]
    [SerializeField] private ObstaclesConfig _obstaclesConfig;
    [SerializeField] private PlayerModelsConfig _playerModelsConfig;

    private GalleryController _gallery;
    private SaveManager _saveManager;
    private Obstacle _activeObstacle;
    private GameData _data;

    private float pausePosition = 0f;
    private InputHandler _inputHandler;
    private PlayerController _playerController;
    private Rigidbody _ballRigidbody;
    private bool _isActiveTop = true; //flag for trigger zone
    private GameState _gameState = GameState.Unknown;
    private bool _gameIsActive;
    private bool _jumpFlag;
    private bool _topStart;

    private int _scoreToRewardedContinue;
    private int _gameScore = 0;
    private float _jumpDealy = .25f;

    private void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;

        _saveManager = GetComponent<SaveManager>();
        _data = _saveManager.LoadData();
        _gameScore = _data.LastScore;
        
        _gallery = GetComponent<GalleryController>();
        _gallery.Instantiate(_playerModelsConfig);
    }
    
    private void Start()
    {
        OnDataUpdate?.Invoke(_data);
        
        _ballRigidbody = player.GetComponent<Rigidbody>();
        _audioFx = GetComponent<AudioFX>();
        _inputHandler = GetComponent<InputHandler>();
        _playerController = player.GetComponent<PlayerController>();

        _playerController.Interactable = false;

        _playerController.switchTriggerZone += SwitchTriggerZone;
        _playerController.death += Death;
        _inputHandler.touched += PlayGame;
        _uiController.OnButtonClick += PlayButtonSound;
        
        _uiController.UpdateMenu(_data.Score, _data.LastScore);
        _playerController.SetModel(_gallery.GetPlayerModel());

        _gameState = GameState.Menu;
        _uiController.SetUI(_gameState);
    }

    private void Update()
    {
        if (!_gameIsActive && player.transform.position.y <= pausePosition && !_jumpFlag)
        {
            _jumpFlag = true;
            DelayedJump();
        }
        
        if (!_gameIsActive && player.transform.position.y > pausePosition + 1 && _ballRigidbody.velocity.y > 0)
            _ballRigidbody.velocity = Vector3.zero;
    }
    private async void DelayedJump()
    {
        _ballRigidbody.velocity = new Vector3(0, 4, 0);
        await UniTask.Delay(TimeSpan.FromSeconds(_jumpDealy));
        _jumpFlag = false;
    }

    public async void StartGame()
    {
        if(_gameState == GameState.Game)
            return;

        _activeObstacle = Instantiate(_obstaclesConfig.GetRandomObstacle(), transform);
        
        _inputHandler.touched += _playerController.Jump;
        _gameState = GameState.Game;
        _uiController.SetUI(_gameState);

        _uiController.SetRewardedContinue(true);

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

        await UniTask.Delay(TimeSpan.FromSeconds(1f));

        _playerController.Interactable = true;
        _activeObstacle.gameObject.SetActive(true);
        _activeObstacle.Show();
    }

    public void PauseGame()
    {
        _gameState = GameState.PauseMenu;
        _uiController.SetUI(_gameState);
        _gameIsActive = false;
        _jumpDealy = .25f;
        _playerController.SetFixedJump(true);
        
        _playerController.Interactable = false;
        
        if (!_isActiveTop)
        {
            pausePosition = 2.5f;
        }
        else
        {
            pausePosition = -2.5f;
        }

        if (_inputHandler.touched != null) _inputHandler.touched -= _playerController.Jump;
    }

    public void ContinueGame()
    {
        _gameState = GameState.Game;
        _uiController.SetUI(_gameState);
        
        _playerController.Interactable = true;
        _inputHandler.touched += _playerController.Jump;
    }
    
    public void ContinueGameOnDeath()
    {
        StartGame();
        _gameScore = _scoreToRewardedContinue;
        _uiController.SetRewardedContinue(false);
    }

    public void ExitToMenu()
    {
        _gameState = GameState.Menu;
        _uiController.SetUI(_gameState);
        _gameIsActive = false;
        pausePosition = 0;
        _gameScore = 0;

        _playerController.Interactable = false;

        if (_inputHandler.touched != null) _inputHandler.touched -= _playerController.Jump;

        _activeObstacle.Hide();
    }

    public void RestartGame()
    {
        _gameScore = 0;
        _data.LastScore = 0;
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
        
        if (_data.Score < _gameScore)
            _data.Score = _gameScore;

        _scoreToRewardedContinue = _gameScore;
        _gameScore = 0;
        _data.LastScore = 0;
        _saveManager.SaveData(_data);
        OnDataUpdate?.Invoke(_data);
        
        _uiController.UpdateMenu(_data.Score, _data.LastScore);

        _playerController.SetFixedJump(true);
        if (_inputHandler != null) _inputHandler.touched -= _playerController.Jump;

        _playerController.Interactable = false;
        _activeObstacle.Hide();
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

    private void OnApplicationFocus(bool focus)
    {
        if (!focus)
        {
            _data.LastScore = _gameScore;
            _saveManager.SaveData(_data);
            OnDataUpdate?.Invoke(_data);
        }
    }

    private void OnDestroy()
    {
        _playerController.switchTriggerZone -= SwitchTriggerZone;
        _playerController.death -= Death;
        _inputHandler.touched -= PlayGame;
        _uiController.OnButtonClick -= PlayButtonSound;
    }
}
