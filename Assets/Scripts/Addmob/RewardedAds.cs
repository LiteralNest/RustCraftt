using Cloud.DataBaseSystem.UserData;
using GoogleMobileAds.Api;
using TMPro;
using UnityEngine;

public class RewardedAds
{
    private RewardedAd _rewardedAd;
    private string _rewardedId = "ca-app-pub-3940256099942544/5224354917";
    
    public RewardedAds()
    {
        LoadRewardedAd();
    }

    private void LoadRewardedAd() {

        if (_rewardedAd!=null)
        {
            _rewardedAd.Destroy();
            _rewardedAd = null;
        }
        var adRequest = new AdRequest();
        adRequest.Keywords.Add("unity-admob-sample");

        RewardedAd.Load(_rewardedId, adRequest, (ad, error) =>
        {
            if (error != null || ad == null)
            {
                Debug.LogError("Rewarded failed to load"+error);
                return;
            }

            Debug.Log("Rewarded ad loaded !!");
            _rewardedAd = ad;
            RewardedAdEvents(_rewardedAd);
        });
    }
    
    public void ShowRewardedAd(TextMeshProUGUI goldTextMeshProUGUI)
    {
        if (_rewardedAd != null && _rewardedAd.CanShowAd())
        {
            _rewardedAd.Show(reward =>
            {
                UserDataHandler.Singleton.AddGold(15);
                Debug.Log("Give reward to player !!");
                goldTextMeshProUGUI.text = "" + UserDataHandler.Singleton.UserData.GoldValue;
            });
        }
        else {
            Debug.LogError("Rewarded ad not ready");
        }
    }

    
    private void RewardedAdEvents(RewardedAd ad)
    {
        ad.OnAdPaid += (AdValue adValue) =>
        {
            Debug.Log("Rewarded ad paid {0} {1}."+
                      adValue.Value+
                      adValue.CurrencyCode);
        };
        ad.OnAdImpressionRecorded += () =>
        {
            Debug.Log("Rewarded ad recorded an impression.");
        };
        ad.OnAdClicked += () =>
        {
            Debug.Log("Rewarded ad was clicked.");
        };
        ad.OnAdFullScreenContentOpened += () =>
        {
            Debug.Log("Rewarded ad full screen content opened.");
        };
        ad.OnAdFullScreenContentClosed += () =>
        {
            Debug.Log("Rewarded ad full screen content closed.");
        };
        ad.OnAdFullScreenContentFailed += (AdError error) =>
        {
            Debug.LogError("Rewarded ad failed to open full screen content " +
                           "with error : " + error);
        };
    }
}
