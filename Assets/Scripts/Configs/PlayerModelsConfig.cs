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
        if (player == null) return modelsPrefab[0];
        return player;
    }

    public UnlockCondition GetModelStatus(string key)
    {
        return GetPlayer(key).UnlockCondition;
    }

    public Mesh GetMesh(string key)
    {
        return GetPlayer(key).GetComponent<MeshFilter>().sharedMesh;
    }

    public Material GetMaterial(string key)
    {
        return GetPlayer(key).GetComponent<MeshRenderer>().sharedMaterial;
    }

    public Vector3 GetScale(string key)
    {
        return GetPlayer(key).transform.localScale;
    }
}
