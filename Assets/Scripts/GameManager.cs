using System;
using System.Collections.Generic;
using System.Linq;
using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Rendering;
using Random = UnityEngine.Random;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;
    public UnityEvent<GameData> OnDataUpdate;
    public GameData GameData => _data;
    public GameState GameState => _gameState;
    public float ObstacleTimer => timerValue;
    
    [HideInInspector] public float jumpDealy = .25f;
    [HideInInspector] public float pausePosition = 0f;

    
    [Header("Controllers")]
    [SerializeField] private UIController _uiController;
    [SerializeField] private AudioFX _audioFx;
    [SerializeField] private RewardedAdsButton _ads;
    
    [Header("Game objects")]
    [SerializeField] private GameObject topZone;
    [SerializeField] private GameObject bottomZone;
    
    [Header("Configs")]
    [SerializeField] private ObstaclesConfig _obstaclesConfig;
    [SerializeField] private PlayerModelsConfig _playerModelsConfig;

    [Header("Gameplay settings")] 
    [SerializeField] private float timerValue = 5;

    private GalleryController _gallery;
    private ObstaclesController _obstaclesController;
    private SaveManager _saveManager;
    private GameData _data = new GameData();
    private Settings _settings;

    private InputHandler _inputHandler;
    private PlayerController _playerController;
    private bool _isActiveTop = true; //flag for trigger zone
    private GameState _gameState = GameState.Unknown;
    private bool _topStart;

    private int _scoreToRewardedContinue;
    private int _gameScore = 0;

    private List<Obstacle> _spawnedObstacles = new List<Obstacle>();

    private void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;

        _saveManager = GetComponent<SaveManager>();
        _settings = GetComponent<Settings>();
        _gallery = GetComponent<GalleryController>();
        _audioFx = GetComponent<AudioFX>();
        _inputHandler = GetComponent<InputHandler>();
        _obstaclesController = GetComponent<ObstaclesController>();

        _data = _saveManager.LoadData();
        _gameScore = _data.LastScore;
        
        _gallery.OnChangePlayer.AddListener(ChangePlayer);
        _gallery.Initialize(_playerModelsConfig, _data.PlayerKey);

    }
    
    private void Start()
    {
        OnDataUpdate?.Invoke(_data);

        _obstaclesController.Initialize(_obstaclesConfig);
        _settings.Initialize(_data);

        _inputHandler.touched += Vibrate;
        _uiController.OnButtonClick += PlayButtonSound;
        _settings.OnSettingsUpdate += SaveSettings;
        
        _uiController.UpdateMenu(_data.Score, _data.LastScore);

        _gameState = GameState.Menu;
        _uiController.SetUI(_gameState);

    }

    private void SignPlayer()
    {
        if (_playerController == null)
        {
            Debug.LogError("Player is NULL");
            return;
        }

        _playerController.Interactable = false;
        _playerController.switchTriggerZone += SwitchTriggerZone;
        _playerController.death += Death;
    }

    private void UnSignPlayer()
    {
        _playerController.switchTriggerZone -= SwitchTriggerZone;
        _playerController.death -= Death;
    }


    public async void StartGame()
    {
        if(_gameState == GameState.Game)
            return;

        foreach (Obstacle obstacle in _spawnedObstacles)
        {
            Destroy(obstacle.gameObject);
        }

        _spawnedObstacles.Clear();
        _gameState = GameState.Game;
        _uiController.SetUI(_gameState);

        _uiController.SetRewardedContinue(true);
        _uiController.UpdateTimer(0);

        _topStart = Random.Range(0, 2) == 1;
        _isActiveTop = !_topStart;

        if (_topStart)
        {
            pausePosition = 2.5f;
            jumpDealy = .4f;
            
            bottomZone.SetActive(true);
            topZone.SetActive(false);
        }
        else
        {
            pausePosition = -2.5f;
            jumpDealy = .25f;
            
            bottomZone.SetActive(false);
            topZone.SetActive(true);
        }

        await UniTask.Delay(TimeSpan.FromSeconds(1f));
        
        NextObstacle();

        _inputHandler.touched += _playerController.Jump;
    }

    private async void NextObstacle()
    {
        if (_gameState != GameState.Game) return;

        _obstaclesController.NextObstacle();
        
        await GameDelay(timerValue);

        NextObstacle();
    }

    private async UniTask GameDelay(float delay)
    {
        float timer = 0;

        while (timer < delay)
        {
            if (_gameState != GameState.Game) return;
            _uiController.UpdateTimer(timer);
            timer += .01f;
            await UniTask.Delay(TimeSpan.FromSeconds(.01f));
        }
    }
    
    public void PauseGame()
    {
        _gameState = GameState.PauseMenu;
        _uiController.SetUI(_gameState);
        _obstaclesController.ClearScene();

        jumpDealy = .25f;
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

        _inputHandler.touched -= _playerController.Jump;
    }

    public async void ContinueGame()
    {
        _gameState = GameState.Game;
        _uiController.SetUI(_gameState);
        _inputHandler.touched += _playerController.Jump;

        _obstaclesController.LastObstacle();
        await GameDelay(timerValue);
        NextObstacle();
        
    }
    
    public void ContinueGameOnDeath()
    {
        StartGame();
        _gameScore = _scoreToRewardedContinue;
        _data.ViewedAds++;
        _uiController.SetRewardedContinue(false);
    }

    public void ExitToMenu()
    {
        _gameState = GameState.Menu;
        _uiController.SetUI(_gameState);
        pausePosition = 0;
        _gameScore = 0;

        _playerController.Interactable = false;
        
        _inputHandler.touched -= _playerController.Jump;
        
        bottomZone.SetActive(true);
        topZone.SetActive(true);


        _obstaclesController.ClearScene();
    }

    public void ShowGallery()
    {
        _gameState = GameState.Shop;
        _uiController.SetUI(_gameState);
    }

    public void ChangePlayer(PlayerController playerController)
    {
        if (_playerController != null)
        {
            UnSignPlayer();
            Destroy(_playerController.gameObject);
        }

        _data.PlayerKey = playerController.Key;
        _playerController = playerController;
        SignPlayer();
    }

    public void ShowSettings()
    {
        _gameState = GameState.Settings;
        _uiController.SetUI(_gameState);
    }

    public void RestartGame()
    {
        _gameScore = 0;
        _data.LastScore = 0;
        StartGame();
    }

    private void Death()
    {
        _gameState = GameState.Death;
        _uiController.SetUI(_gameState);
        _uiController.SetDeathScreenScore(_gameScore);
        pausePosition = 0;

        _obstaclesController.HideAll();
        
        if (_data.Score < _gameScore)
            _data.Score = _gameScore;

        _scoreToRewardedContinue = _gameScore;
        _gameScore = 0;
        _data.LastScore = 0;
        _saveManager.SaveData(_data);
        OnDataUpdate?.Invoke(_data);
        
        _uiController.UpdateMenu(_data.Score, _data.LastScore);

        _playerController.SetFixedJump(true);
        
        _inputHandler.touched -= _playerController.Jump;

        _playerController.Interactable = false;

        _ads.LoadAd();
    }

    public void InvokeDataUpdate()
    {
        OnDataUpdate?.Invoke(_data);
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

    private void SaveSettings(GameData.SettingsData data)
    {
        _audioFx.SetSound(data.Sound);
        _audioFx.SetMusic(data.Music);


        _data.Settings.Sound = data.Sound;
        _data.Settings.Music = data.Music;
        _data.Settings.Vibration = data.Vibration;
        _saveManager.SaveData(_data);
    }

    private void PlayButtonSound()
    {
        _audioFx.PlayButtonSound();
    }

    private void Vibrate()
    {
        if (_data.Settings.Vibration)
        {
            Vibration.Vibrate(50);
        }
    }

    private void OnDestroy()
    {
        UnSignPlayer();
        _inputHandler.touched -= _playerController.Jump;
        _uiController.OnButtonClick -= PlayButtonSound;
        _settings.OnSettingsUpdate -= SaveSettings;
        _inputHandler.touched -= Vibrate;
        _gallery.OnChangePlayer.RemoveAllListeners();
        OnDataUpdate.RemoveAllListeners();
    }
}
