using System;
using UnityEngine;
using UnityEngine.UI;

public class UiButton : MonoBehaviour
{
    [SerializeField] private string _key;
    private Button _button;
    public string Key => _key;

    public bool interactable
    {
        get { return _button.interactable; }
        set { _button.interactable = value; }
    }

    private void Awake()
    {
        _button = GetComponent<Button>();
    }

    public void AddListner(Action action)
    {
        _button.onClick.AddListener(action.Invoke);
    }

    public void OnDestroy()
    {
        _button.onClick.RemoveAllListeners();
    }
}
