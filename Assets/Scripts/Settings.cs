using System;
using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;

public class Settings : MonoBehaviour
{
    public event Action<GameData.SettingsData> OnSettingsUpdate;

    [SerializeField] private Slider _soundSlider;
    [SerializeField] private Slider _musicSlider;
    [SerializeField] private Toggle _vibrationToggle;

    private GameData.SettingsData _data = new GameData.SettingsData();

    public async void Initialize(GameData data)
    {
        _data.Sound = data.Settings.Sound;
        _data.Music = data.Settings.Music;
        _data.Vibration = data.Settings.Vibration;
        
        _soundSlider.onValueChanged.AddListener(UpdateSound);
        _musicSlider.onValueChanged.AddListener(UpdateMusic);
        _vibrationToggle.onValueChanged.AddListener(UpdateVibration);

        await UniTask.Delay(TimeSpan.FromSeconds(.5f));
        
        _soundSlider.value = _data.Sound;
        _musicSlider.value = _data.Music;
        _vibrationToggle.isOn = _data.Vibration;
    }

    private void UpdateVibration(bool value)
    {
        _data.Vibration = value;
        OnSettingsUpdate?.Invoke(_data);
    }

    private void UpdateSound(float value)
    {
        _data.Sound = value;
        OnSettingsUpdate?.Invoke(_data);
    }
    
    private void UpdateMusic(float value)
    {
        _data.Music = value;
        OnSettingsUpdate?.Invoke(_data);
    }

    private void OnDestroy()
    {
        _soundSlider.onValueChanged.RemoveAllListeners();
        _musicSlider.onValueChanged.RemoveAllListeners();
        _vibrationToggle.onValueChanged.RemoveAllListeners();
    }
}

