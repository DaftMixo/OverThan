using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using TMPro;

public class GalleryItem : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI label;

    private string _key;
    private Button _button;
    private UnityAction<string> _action;
    private UnlockCondition _condition;

    public void Init(string key, UnlockCondition condition, UnityAction<string> action)
    {
        _key = key;
        _condition = condition;
        _action = action;

        _button = GetComponent<Button>();
        _button.onClick.AddListener(ChangeModel);
        _button.interactable = false;

        label.text = _key;

        GameManager.Instance.OnDataUpdate.AddListener(UpdateState);
    }

    public void UpdateState(GameData data)
    {
        if (_condition.ByToUnlock)
        {
            //TODO: Check to buy game
            return;
        }

        if (_condition.ViewedAdsToUnlock > 0 && data.ViewedAds >= _condition.ViewedAdsToUnlock)
        {
            _button.interactable = true;
        }
        
        if(data.Score >= _condition.ScoreToUnlock)
            _button.interactable = true;
    }

    private void ChangeModel()
    {
        _action?.Invoke(_key);
    }
}
