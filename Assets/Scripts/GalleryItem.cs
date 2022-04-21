using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using TMPro;

public class GalleryItem : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI label;
    [SerializeField] private GameObject model;
    [SerializeField] private Image lockImage;

    private string _key;
    private Material _material;
    private Button _button;
    private UnityAction<string, bool> _action;
    private UnlockCondition _condition;

    private bool _unLocked;

    public void Init(GalleryItemInitData data)
    {
        _key = data.Key;
        _condition = data.Condition;
        _action = data.Action;
        _material = data.Material;
        model.GetComponent<MeshFilter>().mesh = data.Mesh;
        model.transform.localScale = data.Scale * 200;
        

        _button = GetComponent<Button>();
        _button.onClick.AddListener(ChangeModel);

        if (_condition.InAppKey == string.Empty)
            lockImage.color = Color.white;
        else 
            lockImage.color = Color.yellow;

        label.text = _key;

        GameManager.Instance.OnDataUpdate.AddListener(UpdateState);
    }

    private void FixedUpdate()
    {
        model.transform.Rotate(.5f, -.5f, 1);
    }

    public void UpdateState(GameData data)
    {
        if (_condition.InAppKey != string.Empty)
        {
            if (PlayerPrefs.HasKey(_condition.InAppKey))
                if (PlayerPrefs.GetString(_condition.InAppKey) != string.Empty)
                    Unlock();
            return;
        }

        if (data.Score >= _condition.ScoreToUnlock && data.ViewedAds >= _condition.ViewedAdsToUnlock)
        {
            Unlock();
        }
    }

    private void Unlock()
    {
        _unLocked = true;
        lockImage.gameObject.SetActive(false);
        model.GetComponent<MeshRenderer>().material = _material;
    }

    private void ChangeModel()
    {
        _action?.Invoke(_key, _unLocked);
    }
}

public struct GalleryItemInitData
{
    public string Key;
    public Mesh Mesh;
    public Material Material;
    public Vector3 Scale;
    public UnlockCondition Condition;
    public UnityAction<string, bool> Action;
}
