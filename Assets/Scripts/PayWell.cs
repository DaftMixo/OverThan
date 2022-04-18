using UnityEngine;
using UnityEngine.Purchasing;

public class PayWell : MonoBehaviour
{
    public void OnDicePackPurchaseComplete(Product prod)
    {
        PlayerPrefs.SetString("dicepack", prod.transactionID);
        Debug.Log("complete");
    }
}
