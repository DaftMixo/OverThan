using UnityEngine;

public class UIController : MonoBehaviour
{
    [SerializeField] private GameObject _menu;
    [SerializeField] private GameObject _game;
    [SerializeField] private GameObject _shop;
    [SerializeField] private GameObject _settings;

    public void SetUI(GameState gameState)
    {
        if (gameState == GameState.Menu)
        {
            _menu.SetActive(true);
            _shop.SetActive(false);
            _game.SetActive(false);
            _settings.SetActive(false);
        }
        else if (gameState == GameState.Game)
        {
            _menu.SetActive(false);
            _shop.SetActive(false);
            _game.SetActive(true);
            _settings.SetActive(false);
        }
        else if (gameState == GameState.Shop)
        {
            _menu.SetActive(false);
            _shop.SetActive(true);
            _game.SetActive(false);
            _settings.SetActive(false);
        }
        else if (gameState == GameState.Settings)
        {
            _menu.SetActive(false);
            _shop.SetActive(false);
            _game.SetActive(false);
            _settings.SetActive(true);
        }
    }
}
