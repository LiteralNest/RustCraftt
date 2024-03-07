using GoogleMobileAds.Api;
using UnityEngine;

public class InterstitialAds
{
    private InterstitialAd _interstitialAd;
    private string _interId = "ca-app-pub-3940256099942544/1033173712";

    public InterstitialAds()
    {
        LoadInterstitialAd();
    }

    private void LoadInterstitialAd() {

        if (_interstitialAd!=null)
        {
            _interstitialAd.Destroy();
            _interstitialAd = null;
        }
        var adRequest = new AdRequest();
        adRequest.Keywords.Add("unity-admob-sample");

        InterstitialAd.Load(_interId, adRequest, (ad, error) =>
        {
            if (error!=null||ad==null)
            {
                Debug.LogError("Interstitial ad failed to load"+error);
                return;
            }

            Debug.Log("Interstitial ad loaded !!"+ad.GetResponseInfo());

            _interstitialAd = ad;
            InterstitialEvent(_interstitialAd);
        });

    }

    public void ShowInterstitialAd() {

        if (_interstitialAd != null && _interstitialAd.CanShowAd())
        {
            _interstitialAd.Show();
        }
        else {
            Debug.LogError("Intersititial ad not ready!!");
        }
    }

    private void InterstitialEvent(InterstitialAd ad) {
        ad.OnAdPaid += adValue => 
        {
            Debug.Log("Interstitial ad paid {0} {1}."+
                      adValue.Value+
                      adValue.CurrencyCode);
        };
        ad.OnAdImpressionRecorded += () =>
        {
            Debug.Log("Interstitial ad recorded an impression.");
        };
        ad.OnAdClicked += () =>
        {
            Debug.Log("Interstitial ad was clicked.");
        };
        ad.OnAdFullScreenContentOpened += () =>
        {
            Debug.Log("Interstitial ad full screen content opened.");
        };
        ad.OnAdFullScreenContentClosed += () =>
        {
            Debug.Log("Interstitial ad full screen content closed.");
        };
        ad.OnAdFullScreenContentFailed += error =>
        {
            Debug.LogError("Interstitial ad failed to open full screen content " +
                           "with error : " + error);
        };
    }
}