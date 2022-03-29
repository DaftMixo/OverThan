using System.Collections.Generic;
using UnityEngine;

public class GalleryController : MonoBehaviour
{
    [SerializeField] private GameObject bar;
    [SerializeField] private GalleryItem barItemPrefab;

    private PlayerModelsConfig _config;
    private List<GameData.Model> _modelsList;


    public void Instantiate(PlayerModelsConfig config, List<GameData.Model> modelsList)
    {
        _config = config;
        _modelsList = modelsList;
        Debug.Log(_modelsList.Count);
        for (int i = 0; i <= _modelsList.Count; i++)
        {
            var item = Instantiate(barItemPrefab, bar.transform);
            item.Init(i);
        }
    }

    public GameObject GetPlayerModel(int index)
    {
        if (_modelsList[index] != null && _modelsList[index].IsUnlocked)
            return _config.GetModel(index).gameObject;
        return null;
    }
}
