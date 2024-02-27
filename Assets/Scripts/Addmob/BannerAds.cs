using GoogleMobileAds.Api;
using UnityEngine;

public class BannerAds
{
    private BannerView _bannerView;
    private string _bannerId = "ca-app-pub-1385093244148841/2952458907";

    public BannerAds()
    {
        LoadBannerAd();
    }

    private void LoadBannerAd()
    {
        CreateBannerView();

        ListenToBannerEvents();

        // if (_bannerView == null)
        // {
        //     CreateBannerView();
        // }

        var adRequest = new AdRequest();
        adRequest.Keywords.Add("unity-admob-sample");

        Debug.Log("Loading banner Ad !!");
        _bannerView?.LoadAd(adRequest);
    }

    public void CreateBannerView()
    {
        if (_bannerView != null)
        {
            DestroyBannerAd();
        }

        _bannerView = new BannerView(_bannerId, AdSize.Banner, AdPosition.Top);
    }

    private void ListenToBannerEvents()
    {
        _bannerView.OnBannerAdLoaded += () =>
        {
            Debug.Log("Banner view loaded an ad with response : "
                      + _bannerView.GetResponseInfo());
        };
        _bannerView.OnBannerAdLoadFailed += (LoadAdError error) =>
        {
            Debug.LogError("Banner view failed to load an ad with error : "
                           + error);
        };
        _bannerView.OnAdPaid += (AdValue adValue) =>
        {
            Debug.Log("Banner view paid {0} {1}." +
                      adValue.Value +
                      adValue.CurrencyCode);
        };
        _bannerView.OnAdImpressionRecorded += () => { Debug.Log("Banner view recorded an impression."); };
        _bannerView.OnAdClicked += () => { Debug.Log("Banner view was clicked."); };
        _bannerView.OnAdFullScreenContentOpened += () => { Debug.Log("Banner view full screen content opened."); };
        _bannerView.OnAdFullScreenContentClosed += () => { Debug.Log("Banner view full screen content closed."); };
    }

    public void DestroyBannerAd()
    {

        if (_bannerView != null)
        {
            Debug.Log("Destroying banner Ad");
            _bannerView.Destroy();
            _bannerView = null;
        }
    }
}