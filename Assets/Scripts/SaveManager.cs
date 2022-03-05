using System;
using UnityEngine;

public class SaveManager : MonoBehaviour
{
    private const string DataKey = "GameData";
    
    public void SaveData(GameData data)
    {
        var jsonData = JsonUtility.ToJson(data);
        PlayerPrefs.SetString(DataKey, jsonData);
    }

    public GameData LoadData()
    {
        if (!PlayerPrefs.HasKey(DataKey))
            return new GameData();
        
        var jsonData = PlayerPrefs.GetString(DataKey);
        var _data = JsonUtility.FromJson<GameData>(jsonData);
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

    public SettingsData Settings = new SettingsData();

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
        public float Music = .7f;
        public float Sound = .7f;
        public bool Vibration;
    }
}
