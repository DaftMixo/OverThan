using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GalleryItem : MonoBehaviour
{
    private int _index;
    private Button _button;

    public void Init(int index)
    {
        _index = index;
        _button = GetComponent<Button>();
        _button.onClick.AddListener(ChangeModel);
    }

    private void ChangeModel()
    {
        Debug.Log(_index);
        GameManager.Instance.ChangePlayer(_index);
    }
}
