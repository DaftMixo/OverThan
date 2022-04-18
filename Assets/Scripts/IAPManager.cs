using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Purchasing;

public class IAPManager : MonoBehaviour, IStoreListener
{
    public static readonly string ProdIdDicePack = "dicepack";

    private IStoreController controller;
    private IExtensionProvider extensions;

    public void Awake()
    {
        var builder = ConfigurationBuilder.Instance(StandardPurchasingModule.Instance());
        builder.AddProduct(ProdIdDicePack, ProductType.Consumable);

        UnityPurchasing.Initialize(this, builder);
    }

    public void BuyDicePack()
    {
        controller.InitiatePurchase(ProdIdDicePack);
    }

    public void OnInitialized(IStoreController controller, IExtensionProvider extensions)
    {
        this.controller = controller;
        this.extensions = extensions;
    }

    public void OnInitializeFailed(InitializationFailureReason error)
    {
        Debug.LogError(error.ToString());
    }

    public void OnPurchaseFailed(Product product, PurchaseFailureReason failureReason)
    {
        Debug.LogWarning(product.ToString() + "\n" + failureReason.ToString());
    }

    public PurchaseProcessingResult ProcessPurchase(PurchaseEventArgs purchaseEvent)
    {
        return PurchaseProcessingResult.Complete;
    }

}
