using System;
using UnityEngine;
using UnityEngine.UI;

public class UIController : MonoBehaviour
{
    public Action OnButtonClick;
    
    [SerializeField] private GameObject _menu;
    [SerializeField] private GameObject _game;
    [SerializeField] private GameObject _shop;
    [SerializeField] private GameObject _settings;
    [SerializeField] private GameObject _pauseMenu;
    [SerializeField] private GameObject _deathScreen;

    [SerializeField] private Button[] _buttons;

    private void Start()
    {
        if (_buttons.Length > 0)
            foreach (var item in _buttons)
            {
                item.onClick.AddListener(OnButtonClick.Invoke);
            }
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
        }
    }
}
