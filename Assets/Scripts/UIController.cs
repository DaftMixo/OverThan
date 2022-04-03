using System;
using Cysharp.Threading.Tasks;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIController : MonoBehaviour
{
    public Action OnButtonClick;
    
    [Header("Panels")]
    [SerializeField] private GameObject _menu;
    [SerializeField] private GameObject _game;
    [SerializeField] private GameObject _gallery;
    [SerializeField] private GameObject _settings;
    [SerializeField] private GameObject _pauseMenu;
    [SerializeField] private GameObject _deathScreen;

    [Header("UI items")] 
    [SerializeField] private TMP_Text _startMenuLabel;
    [SerializeField] private TMP_Text _menuScreenScore;
    [SerializeField] private TMP_Text _deathScreenScore;
    [SerializeField] private Image _switchTimer;
    
    private UiButton[] _uiButtons;

    private void Awake()
    {
        _uiButtons = FindObjectsOfType<UiButton>();
    }

    private void Start()
    {
        foreach (var button in _uiButtons)
        {
            button.AddListner(OnButtonClick);
        }
    }

    public void UpdateMenu(int score, int lastScore)
    {
        SetMenuScore(score);
        
        _startMenuLabel.text = "START";
        if (lastScore != 0)
            _startMenuLabel.text = "CONTINUE";
    }

    public void UpdateTimer(float time)
    {
        _switchTimer.fillAmount = Map(time, GameManager.Instance.ObstacleTimer, 0, 1, 0);
    }

    public float Map(float value, float fromSource, float toSource, float fromTarget, float toTarget)
    {
        return (value - fromSource) / (toSource - fromSource) * (toTarget - fromTarget) + fromTarget;
    }

    private void SetMenuScore(int value)
    {
        _menuScreenScore.text = $"Score: {value}";
    }

    public void SetDeathScreenScore(int score)
    {
        _deathScreenScore.text = score.ToString();
    }

    public void SetRewardedContinue(bool value)
    {
        foreach (var button in _uiButtons)
        {
            if (button.Key == "Continue_death") button.gameObject.SetActive(value);
        }
    }
    
    private async void DeathScreenButtonDelay()
    {
        foreach (var button in _uiButtons)
        {
            if (button.Key == "Restart_death") button.interactable = false;
            if (button.Key == "Exit_death") button.interactable = false;
        }
        await UniTask.Delay(TimeSpan.FromSeconds(2));
        foreach (var button in _uiButtons)
        {
            if (button.Key == "Restart_death") button.interactable = true;
            if (button.Key == "Exit_death") button.interactable = true;
        }

    }

    public void SetUI(GameState gameState)
    {
        _menu.SetActive(false);
        _gallery.SetActive(false);
        _game.SetActive(false);
        _settings.SetActive(false);
        _pauseMenu.SetActive(false);
        _deathScreen.SetActive(false);
        
        switch (gameState)
        {
            case GameState.Menu :
                _menu.SetActive(true);
                break;
            case GameState.Game :
                _game.SetActive(true);
                break;
            case GameState.PauseMenu :
                _pauseMenu.SetActive(true);
                break;
            case GameState.Shop :
                _gallery.SetActive(true);
                break;
            case GameState.Settings :
                _settings.SetActive(true);
                break;
            case GameState.Death :
                _deathScreen.SetActive(true);
                DeathScreenButtonDelay();
                break;
        }
        
    }

    private void OnDestroy()
    {
        foreach (var button in _uiButtons)
        {
            button.OnDestroy();
        }
    }
}


