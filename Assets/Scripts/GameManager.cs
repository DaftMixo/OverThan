using System;
using System.Collections.Generic;
using System.Linq;
using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.Android;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;
using Random = UnityEngine.Random;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;
    public event Action<GameData> OnDataUpdate;
    
    [HideInInspector] public float jumpDealy = .25f;
    [HideInInspector] public bool gameIsActive;
    [HideInInspector] public float pausePosition = 0f;

    
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

    [Header("Gameplay settings")] 
    [SerializeField] private float timerValue = 5;

    private float timer;

    private GalleryController _gallery;
    private SaveManager _saveManager;
    private Obstacle _activeObstacle;
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
        _data = _saveManager.LoadData();
        _gameScore = _data.LastScore;
        
        _gallery = GetComponent<GalleryController>();
        _gallery.Instantiate(_playerModelsConfig);
    }
    
    private void Start()
    {
        OnDataUpdate?.Invoke(_data);

        _audioFx = GetComponent<AudioFX>();
        _inputHandler = GetComponent<InputHandler>();
        _playerController = player.GetComponent<PlayerController>();

        _playerController.Interactable = false;
        
        _settings.Initialize(_data);

        _playerController.switchTriggerZone += SwitchTriggerZone;
        _playerController.death += Death;
        _inputHandler.touched += Vibrate;
        _uiController.OnButtonClick += PlayButtonSound;
        _settings.OnSettingsUpdate += SaveSettings;
        
        _uiController.UpdateMenu(_data.Score, _data.LastScore);
        _playerController.SetModel(_gallery.GetPlayerModel());

        _gameState = GameState.Menu;
        _uiController.SetUI(_gameState);

        timer = timerValue;
    }

    private void Vibrate()
    {
        if (_data.Settings.Vibration)
        {
            Vibrator.Vibrate();
        }
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

        _inputHandler.touched += PlayGame;
        _inputHandler.touched += _playerController.Jump;
        _playerController.Interactable = true;
    }

    private async void NextObstacle()
    {
        if(_gameState != GameState.Game)
            return;

        var newObstacle = _obstaclesConfig.GetRandomObstacle();
        
        if (_activeObstacle != null)
        {
            while (timer > 0)
            {
                if(_gameState == GameState.Game)
                    timer -= 1;
                if(_gameState != GameState.Game && _gameState != GameState.PauseMenu) break;
                await UniTask.Delay(TimeSpan.FromSeconds(1f));
            }
            while (_activeObstacle.Key == newObstacle.Key)
            {
                newObstacle = _obstaclesConfig.GetRandomObstacle();
            }
            
            _activeObstacle.Hide();
            await UniTask.Delay(TimeSpan.FromSeconds(1f));
        }

        if (_spawnedObstacles.Exists(x => x.Key == newObstacle.Key))
        {
            _activeObstacle = _spawnedObstacles.Find(x => x.Key == newObstacle.Key);
        }
        else
        {
            _activeObstacle = Instantiate(newObstacle, transform);
            _spawnedObstacles.Add(_activeObstacle);
        }
        
        if(_gameState != GameState.Game)
            return;
        
        await UniTask.Delay(TimeSpan.FromSeconds(.5f));
        _activeObstacle.gameObject.SetActive(true);
        _activeObstacle.Show();
        timer = timerValue;
        NextObstacle();
    }

    public void PauseGame()
    {
        _gameState = GameState.PauseMenu;
        _uiController.SetUI(_gameState);
        gameIsActive = false;
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

        _inputHandler.touched -= PlayGame;
        _inputHandler.touched -= _playerController.Jump;
    }

    public void ContinueGame()
    {
        _gameState = GameState.Game;
        _uiController.SetUI(_gameState);
        
        _playerController.Interactable = true;
        _inputHandler.touched += PlayGame;
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
        gameIsActive = false;
        pausePosition = 0;
        _gameScore = 0;

        _playerController.Interactable = false;
        
        _inputHandler.touched -= PlayGame;
        _inputHandler.touched -= _playerController.Jump;
        
        bottomZone.SetActive(true);
        topZone.SetActive(true);

        _activeObstacle?.Hide();
    }

    public void ShowGallery()
    {
        _gameState = GameState.Shop;
        _uiController.SetUI(_gameState);
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

    private void PlayGame()
    {
        if (_gameState == GameState.Game)
            gameIsActive = true;
    }

    private void Death()
    {
        _gameState = GameState.Death;
        _uiController.SetUI(_gameState);
        _uiController.SetDeathScreenScore(_gameScore);
        gameIsActive = false;
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
        
        _inputHandler.touched -= PlayGame;
        _inputHandler.touched -= _playerController.Jump;

        _playerController.Interactable = false;
        _activeObstacle?.Hide();
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

    private void SaveSettings(GameData.SettingsData data)
    {
        _audioFx.SetSound(data.Sound);
        _audioFx.SetMusic(data.Music);
        
        _data.Settings.Sound = data.Sound;
        _data.Settings.Music = data.Music;
        _data.Settings.Vibration = data.Vibration;
        _saveManager.SaveData(_data);
    }

    private void OnDestroy()
    {
        _playerController.switchTriggerZone -= SwitchTriggerZone;
        _playerController.death -= Death;
        _inputHandler.touched -= PlayGame;
        _inputHandler.touched -= _playerController.Jump;
        _uiController.OnButtonClick -= PlayButtonSound;
        _settings.OnSettingsUpdate -= SaveSettings;
        _inputHandler.touched -= Vibrate;
    }
}
