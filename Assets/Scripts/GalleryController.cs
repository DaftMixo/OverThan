using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class GalleryController : MonoBehaviour
{
    [HideInInspector] public UnityEvent<PlayerController> OnChangePlayer; 

    [SerializeField] private GameObject bar;
    [SerializeField] private GalleryItem barItemPrefab;

    private PlayerModelsConfig _config;
    private PlayerController _currentPlayer;

    private List<GalleryItem> _items = new List<GalleryItem>();

    public void Initialize(PlayerModelsConfig config, string startKey)
    {
        _config = config;

        for (int i = 0; i < _config.ArrayLength; i++)
        {
            var item = Instantiate(barItemPrefab, bar.transform);
            var key = _config.GetModel(i).Key;
            var condition = _config.GetPlayer(key).UnlockCondition;

            GalleryItemInitData data = new GalleryItemInitData()
            {
                Key = key,
                Mesh = _config.GetMesh(key),
                Material = _config.GetMaterial(key),
                Scale = _config.GetScale(key),
                Condition = _config.GetPlayer(key).UnlockCondition,
                Action = ChangeModel
        };

            item.Init(data);

            //item.Init(key, condition, ChangeModel);
            _items.Add(item);
        }

        ChangeModel(startKey);
    }

    private void ChangeModel(string key)
    {
        _currentPlayer = Instantiate(_config.GetPlayer(key));
        OnChangePlayer?.Invoke(_currentPlayer);
    }
}
