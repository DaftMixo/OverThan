using UnityEngine;

[CreateAssetMenu(fileName = "UnlockConditionConfig", menuName = "Configs/UnlockConditionConfig", order = 2)]
public class UnlockCondition : ScriptableObject
{
    [SerializeField] private int _scoreToUnlock = 0;
    [SerializeField] private int _viewedAdsToUnlock = 0;
    [SerializeField] private string _inAppKey;

    public int ScoreToUnlock => _scoreToUnlock;
    public int ViewedAdsToUnlock => _viewedAdsToUnlock;
    public string InAppKey => _inAppKey;
}
