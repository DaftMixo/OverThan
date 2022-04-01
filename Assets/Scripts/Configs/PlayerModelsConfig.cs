using System;
using UnityEngine;

[CreateAssetMenu(fileName = "PlayerModelsConfig", menuName = "Configs/PlayerModelsConfig", order = 1)]
public class PlayerModelsConfig : ScriptableObject
{
    [SerializeField] private PlayerController[] modelsPrefab;

    public int ArrayLength => modelsPrefab.Length;

    public PlayerController GetModel(int index)
    {
        if (index < 0) index = 0;
        if (index >= modelsPrefab.Length) index = modelsPrefab.Length;
        return modelsPrefab[index];
    }

    public PlayerController GetPlayer(string key)
    {
        var player = Array.Find(modelsPrefab, p => p.Key == key);
        return player;
    }

    public UnlockCondition GetModelStatus(string key)
    {
        return GetPlayer(key).UnlockCondition;
    }
}
