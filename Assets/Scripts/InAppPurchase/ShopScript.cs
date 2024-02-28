using UnityEngine;
using TMPro;
using System;
using System.Collections;
using UnityEngine.Purchasing;
using System.Collections.Generic;
using Cloud.DataBaseSystem.UserData;
using UnityEngine.Purchasing.Extension;

[Serializable]
public class ConsumableItem 
{
    public string Name;
    public string Id;
    public string desc;
    public float price;
}
[Serializable]
public class NonConsumableItem
{
    public string Name;
    public string Id;
    public string desc;
    public float price;
}
[Serializable]
public class SubscriptionItem
{
    public string Name;
    public string Id;
    public string desc;
    public float price;
    public int timeDuration;// in Days
}

public class ShopScript : MonoBehaviour, IDetailedStoreListener
{
    [SerializeField] private ConsumableItem _consumableItem;
    [SerializeField] private NonConsumableItem _nonConsumableItem;
    [SerializeField] private SubscriptionItem _subscriptionItem;
    [SerializeField] private Data _data;
    [SerializeField] private Payload _payload;
    [SerializeField] private PayloadData _payloadData;

    [Header("Consumable")]
    [SerializeField] private TextMeshProUGUI _inGameMoneyText;

    [Header("Non Consumable")]
    [SerializeField] private GameObject _adsPurchasedWindow;
    [SerializeField] private GameObject _adsBanner;

    [Header("Subscription")]
    [SerializeField] private  GameObject _subActivatedWindow;
    [SerializeField] private  GameObject _premiumBanner;

    private IStoreController _mStoreController;

    private void Start()
    {
        var inGameMoney = UserDataHandler.Singleton.UserData.GoldValue;
        _inGameMoneyText.text = inGameMoney.ToString();
        SetupBuilder();
    }

    #region setup and initialize

    private void SetupBuilder()
    {

        var builder = ConfigurationBuilder.Instance(StandardPurchasingModule.Instance());

        builder.AddProduct(_consumableItem.Id, ProductType.Consumable);
        builder.AddProduct(_nonConsumableItem.Id, ProductType.NonConsumable);
        builder.AddProduct(_subscriptionItem.Id, ProductType.Subscription);

        UnityPurchasing.Initialize(this, builder);
    }

    public void OnInitialized(IStoreController controller, IExtensionProvider extensions)
    {
        Debug.Log("Success");
        _mStoreController = controller;
        CheckNonConsumable(_nonConsumableItem.Id);
        CheckSubscription(_subscriptionItem.Id);
    }

    #endregion


    #region Button clicks

    public void Consumable_Btn_Pressed() => _mStoreController.InitiatePurchase(_consumableItem.Id);

    public void NonConsumable_Btn_Pressed() => _mStoreController.InitiatePurchase(_nonConsumableItem.Id);

    public void Subscription_Btn_Pressed() => _mStoreController.InitiatePurchase(_subscriptionItem.Id);

    #endregion


    #region Main

    public PurchaseProcessingResult ProcessPurchase(PurchaseEventArgs purchaseEvent)
    {
        var product = purchaseEvent.purchasedProduct;

        Debug.Log("Purchase Complete" + product.definition.id);

        if (product.definition.id == _consumableItem.Id)
        {
            var receipt = product.receipt;
            _data = JsonUtility.FromJson<Data>(receipt);
            _payload = JsonUtility.FromJson<Payload>(_data.Payload);
            _payloadData = JsonUtility.FromJson<PayloadData>(_payload.json);

            var quantity = _payloadData.quantity;

            for (var i = 0; i < quantity; i++)
            {
                AddInGameMoney(50);
            }
            
        }
        else if (product.definition.id == _nonConsumableItem.Id)
        {
            RemoveAds();
        }
        else if (product.definition.id == _subscriptionItem.Id)
        {
            ActivateElitePass();
        }

        return PurchaseProcessingResult.Complete;
    }

    #endregion


    private void CheckNonConsumable(string id)
    {
        if (_mStoreController == null) return;
        var product = _mStoreController.products.WithID(id);
        if (product == null) return;
        if (product.hasReceipt)
        {
            RemoveAds();
        }
        else {
            ShowAds();
        }
    }

    private void CheckSubscription(string id) 
    {
        var subProduct = _mStoreController.products.WithID(id);
        if (subProduct != null)
        {
            try
            {
                if (subProduct.hasReceipt)
                {
                    var subManager = new SubscriptionManager(subProduct, null);
                    var info = subManager.getSubscriptionInfo();
                    /*print(info.getCancelDate());
                    print(info.getExpireDate());
                    print(info.getFreeTrialPeriod());
                    print(info.getIntroductoryPrice());
                    print(info.getProductId());
                    print(info.getPurchaseDate());
                    print(info.getRemainingTime());
                    print(info.getSkuDetails());
                    print(info.getSubscriptionPeriod());
                    print(info.isAutoRenewing());
                    print(info.isCancelled());
                    print(info.isExpired());
                    print(info.isFreeTrial());
                    print(info.isSubscribed());*/


                    if (info.isSubscribed() == Result.True)
                    {
                        print("We are subscribed");
                        ActivateElitePass();
                    }
                    else {
                        print("Un subscribed");
                        DeActivateElitePass();
                    }

                }
                else{
                    print("receipt not found !!");
                }
            }
            catch (Exception)
            {

                print("It only work for Google store, app store, amazon store, you are using fake store!!");
            }
        }
        else {
            print("product not found !!");
        }
    }


    #region Error handeling

    public void OnInitializeFailed(InitializationFailureReason error)
    {
        print("failed" + error);
    }

    public void OnInitializeFailed(InitializationFailureReason error, string message)
    {
        print("initialize failed" + error + message);
    }


    public void OnPurchaseFailed(Product product, PurchaseFailureReason failureReason)
    {
        print("purchase failed" + failureReason);
    }

    public void OnPurchaseFailed(Product product, PurchaseFailureDescription failureDescription)
    {
        print("purchase failed" + failureDescription);
    }

    #endregion


    #region extra

    private void AddInGameMoney(int num)
    {
        UserDataHandler.Singleton.AddGold(num);
    }

    private void RemoveAds()
    {
        DisplayAds(false);
    }

    private void ShowAds()
    {
        DisplayAds(true);

    }

    private void DisplayAds(bool value)
    {
        if (!value)
        {
            _adsPurchasedWindow.SetActive(true);
            _adsBanner.SetActive(false);
        }
        else
        {
            _adsPurchasedWindow.SetActive(false);
            _adsBanner.SetActive(true);
        }
    }

    private void ActivateElitePass()
    {
        SetupElitePass(true);
    }
    private void DeActivateElitePass()
    {
        SetupElitePass(false);
    }
    
    private void SetupElitePass(bool value)
    {
        if (value)
        {
            _subActivatedWindow.SetActive(true);
            _premiumBanner.SetActive(true);
        }
        else
        {
            _subActivatedWindow.SetActive(false);
            _premiumBanner.SetActive(false);
        }
    }
    
    #endregion

}


[Serializable]
public class SkuDetails
{
    public string productId;
    public string type;
    public string title;
    public string name;
    public string iconUrl;
    public string description;
    public string price;
    public long price_amount_micros;
    public string price_currency_code;
    public string skuDetailsToken;
}

[Serializable]
public class PayloadData 
{
    public string orderId;
    public string packageName;
    public string productId;
    public long purchaseTime;
    public int purchaseState;
    public string purchaseToken;
    public int quantity;
    public bool acknowledged;
}

[Serializable]
public class Payload
{
    public string json;
    public string signature;
    public List<SkuDetails> skuDetails;
    public PayloadData payloadData;
}

[Serializable]
public class Data
{
    public string Payload;
    public string Store;
    public string TransactionID;
}