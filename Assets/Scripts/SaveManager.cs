using System;
using UnityEngine;

public class SaveManager : MonoBehaviour
{
    private const string DataKey = "GameData";
    private const string SettingsKey = "GameData";
    
    public void SaveData(GameData data)
    {
        var jsonData = JsonUtility.ToJson(data);
        PlayerPrefs.SetString(DataKey, jsonData);
    }
    
    public void SaveSettings(GameData.SettingsData data)
    {
        var jsonData = JsonUtility.ToJson(data);
        PlayerPrefs.SetString(SettingsKey, jsonData);
    }

    public GameData LoadData()
    {
        if (!PlayerPrefs.HasKey(DataKey))
            return new GameData();
        
        var jsonData = PlayerPrefs.GetString(DataKey);
        var _data = JsonUtility.FromJson<GameData>(jsonData);
        return _data;
    }
    
    public GameData.SettingsData LoadSettings()
    {
        if (!PlayerPrefs.HasKey(SettingsKey))
            return new GameData.SettingsData();
        
        var jsonData = PlayerPrefs.GetString(SettingsKey);
        var _data = JsonUtility.FromJson<GameData.SettingsData>(jsonData);
        return _data;
    }
}

[Serializable]
public class GameData
{
    public bool IsFirstStart = true;
    public int Score;
    public int LastScore;

    public int PlayerIndex;

    [Serializable]
    public class Model
    {
        public string Key;
        public Mesh Mesh;
        public bool IsUnlocked;
    }
    
    [Serializable]
    public class SettingsData
    {
        public float Volume;
        public float Color;
        public bool AutoColor;
    }
}
