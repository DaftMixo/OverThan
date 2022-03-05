using System;
using System.Collections;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;
using UnityEngine.UI;

public class Settings : MonoBehaviour
{
    public event Action<GameData.SettingsData> OnSettingsUpdate;

    [SerializeField] private Slider _soundSlider;
    [SerializeField] private Slider _musicSlider;

    private GameData.SettingsData _data = new GameData.SettingsData();

    public async void Initialize(GameData data)
    {
        _data.Sound = data.Settings.Sound;
        _data.Music = data.Settings.Music;
        _data.Vinration = data.Settings.Vinration;
        
        _soundSlider.onValueChanged.AddListener(UpdateSound);
        _musicSlider.onValueChanged.AddListener(UpdateMusic);

        await UniTask.Delay(TimeSpan.FromSeconds(.5f));
        
        _soundSlider.value = _data.Sound;
        _musicSlider.value = _data.Music;
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
    }
}

