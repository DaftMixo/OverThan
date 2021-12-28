using System;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIController : MonoBehaviour
{
    public Action OnButtonClick;
    
    [Header("Panels")]
    [SerializeField] private GameObject _menu;
    [SerializeField] private GameObject _game;
    [SerializeField] private GameObject _shop;
    [SerializeField] private GameObject _settings;
    [SerializeField] private GameObject _pauseMenu;
    [SerializeField] private GameObject _deathScreen;

    [Header("UI items")]
    [SerializeField] private TMP_Text _deathScreenScore;

    [SerializeField] private UiButtons _uiButtons;

    private void Start()
    {
        _uiButtons.SetAudio(OnButtonClick);
    }

    public void SetDeathScreenScore(int score)
    {
        _deathScreenScore.text = score.ToString();
    }
    
    
    private IEnumerator DeathScreenButtonDelay()
    {
        _uiButtons.deathRestart.interactable = false;
        _uiButtons.deathExit.interactable = false;
        yield return new WaitForSeconds(3f);
        _uiButtons.deathRestart.interactable = true;
        _uiButtons.deathExit.interactable = true;

    }

    public void SetUI(GameState gameState)
    {
        if (gameState == GameState.Menu)
        {
            _menu.SetActive(true);
            _shop.SetActive(false);
            _game.SetActive(false);
            _settings.SetActive(false);
            _pauseMenu.SetActive(false);
            _deathScreen.SetActive(false);
        }
        else if (gameState == GameState.Game)
        {
            _menu.SetActive(false);
            _shop.SetActive(false);
            _game.SetActive(true);
            _settings.SetActive(false);
            _pauseMenu.SetActive(false);
            _deathScreen.SetActive(false);
        }
        else if (gameState == GameState.PauseMenu)
        {
            _menu.SetActive(false);
            _shop.SetActive(false);
            _game.SetActive(false);
            _settings.SetActive(false);
            _pauseMenu.SetActive(true);
            _deathScreen.SetActive(false);
        }
        else if (gameState == GameState.Shop)
        {
            _menu.SetActive(false);
            _shop.SetActive(true);
            _game.SetActive(false);
            _settings.SetActive(false);
            _pauseMenu.SetActive(false);
            _deathScreen.SetActive(false);
        }
        else if (gameState == GameState.Settings)
        {
            _menu.SetActive(false);
            _shop.SetActive(false);
            _game.SetActive(false);
            _settings.SetActive(true);
            _pauseMenu.SetActive(false);
            _deathScreen.SetActive(false);
        }
        else if (gameState == GameState.Death)
        {
            _menu.SetActive(false);
            _shop.SetActive(false);
            _game.SetActive(false);
            _settings.SetActive(false);
            _pauseMenu.SetActive(false);
            _deathScreen.SetActive(true);
            StartCoroutine(DeathScreenButtonDelay());
        }
    }
    
    [Serializable] public class UiButtons
    {
        public Button play;
        public Button shop;
        public Button settings;
        public Button pause;
        public Button pauseContinue;
        public Button pauseExit;
        public Button deathContinue;
        public Button deathRestart;
        public Button deathExit;

        public void SetAudio(Action action)
        {
            play.onClick.AddListener(action.Invoke);
            shop.onClick.AddListener(action.Invoke);
            settings.onClick.AddListener(action.Invoke);
            pause.onClick.AddListener(action.Invoke);
            pauseContinue.onClick.AddListener(action.Invoke);
            pauseExit.onClick.AddListener(action.Invoke);
            deathContinue.onClick.AddListener(action.Invoke);
            deathRestart.onClick.AddListener(action.Invoke);
            deathExit.onClick.AddListener(action.Invoke);
        }
    }
}


