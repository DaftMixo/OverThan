using UnityEngine;

public class GalleryController : MonoBehaviour
{
    private PlayerModelsConfig _config;
    private GameData _data;
    
    public void Instantiate(PlayerModelsConfig config)
    {
        _config = config;
        GameManager.Instance.OnDataUpdate += UpdateData;
    }

    private void UpdateData(GameData data)
    {
        _data = data;
    }

    public Mesh GetPlayerModel()
    {
        return _config.GetModel(_data.PlayerIndex);
    }

    private void OnDestroy()
    {
        GameManager.Instance.OnDataUpdate -= UpdateData;
    }
    
    
}
