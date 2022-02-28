using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "PlayerModelsConfig", menuName = "Configs/PlayerModelsConfig", order = 1)]
public class PlayerModelsConfig : ScriptableObject
{
    [SerializeField] private GameData.Model[] models;

    public int ArrayLength => models.Length;

    public Mesh GetModel(int index)
    {
        if (index < 0) index = 0;
        return index < models.Length ? models[index].Mesh : models[models.Length - 1].Mesh;
    }

    public bool GetModelStatus(int index)
    {
        if (index < 0) index = 0;
        return index < models.Length ? models[index].IsUnlocked : models[models.Length - 1].IsUnlocked;
    }
}
