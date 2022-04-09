using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using TMPro;

public class GalleryController : MonoBehaviour
{
    [HideInInspector] public UnityEvent<PlayerController> OnChangePlayer; 

    [SerializeField] private GameObject bar;
    [SerializeField] private GalleryItem barItemPrefab;
    [SerializeField] private Button unlockNotify;

    private PlayerModelsConfig _config;
    private PlayerController _currentPlayer;

    private List<GalleryItem> _items = new List<GalleryItem>();

    public void Initialize(PlayerModelsConfig config, string startKey)
    {
        _config = config;
        unlockNotify.onClick.AddListener(() => unlockNotify.gameObject.SetActive(false));
        unlockNotify.gameObject.SetActive(false);

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

        ChangeModel(startKey, true);
    }

    private void ChangeModel(string key, bool state)
    {
        if (state)
        {
            _currentPlayer = Instantiate(_config.GetPlayer(key));
            OnChangePlayer?.Invoke(_currentPlayer);
        }
        else
        {
            unlockNotify.gameObject.SetActive(true);
            var note = unlockNotify.gameObject.GetComponentInChildren<TextMeshProUGUI>();
            var condition = _config.GetPlayer(key).UnlockCondition;
            string text = "TO UNLOCK:\n\n";

            if (condition.ViewedAdsToUnlock > 0) text += $"View {condition.ViewedAdsToUnlock} ADS in total\n\n";
            if (condition.ScoreToUnlock > 0) text += $"Have score over than {condition.ScoreToUnlock}\n\n";
            note.text = text;
        }
    }
}
