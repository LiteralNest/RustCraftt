using UnityEngine;
using GoogleMobileAds.Api;
using TMPro;

public class AdmobAdsTEST : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI totalCoinsTxt;

    [SerializeField] private string _appId = "ca-app-pub-3940256099942544~3347511713";


#if UNITY_ANDROID
    private string _bannerId = "ca-app-pub-1385093244148841/2952458907";
    private string _interId = "ca-app-pub-3940256099942544/1033173712";
    private string _rewardedId = "ca-app-pub-3940256099942544/5224354917";
    private string _nativeId = "ca-app-pub-3940256099942544/2247696110";

#elif UNITY_IPHONE
   private string _bannerId = "ca-app-pub-3940256099942544/2934735716";
   private string _interId = "ca-app-pub-3940256099942544/4411468910";
   private string _rewardedId = "ca-app-pub-3940256099942544/1712485313";
   private string _nativeId = "ca-app-pub-3940256099942544/3986624511";

#endif

    private BannerView _bannerView;
    private InterstitialAd _interstitialAd;
    private RewardedAd _rewardedAd;


    private void Start()
    {
        ShowCoins();
        MobileAds.RaiseAdEventsOnUnityMainThread = true;
        MobileAds.Initialize(initStatus => {

            print("Ads Initialised !!");
        
        });
    }

    #region Banner

    public void LoadBannerAd() {
        //create a banner
        CreateBannerView();

        //listen to banner events
        ListenToBannerEvents();

        //load the banner
        if (_bannerView==null)
        {
            CreateBannerView();
        }

        var adRequest = new AdRequest();
        adRequest.Keywords.Add("unity-admob-sample");

        print("Loading banner Ad !!");
        _bannerView?.LoadAd(adRequest);
    }
    private void CreateBannerView() {

        if (_bannerView!=null)
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
        // Raised when an ad fails to load into the banner view.
        _bannerView.OnBannerAdLoadFailed += (LoadAdError error) =>
        {
            Debug.LogError("Banner view failed to load an ad with error : "
                + error);
        };
        // Raised when the ad is estimated to have earned money.
        _bannerView.OnAdPaid += (AdValue adValue) =>
        {
            Debug.Log("Banner view paid {0} {1}."+
                adValue.Value+
                adValue.CurrencyCode);
        };
        // Raised when an impression is recorded for an ad.
        _bannerView.OnAdImpressionRecorded += () =>
        {
            Debug.Log("Banner view recorded an impression.");
        };
        // Raised when a click is recorded for an ad.
        _bannerView.OnAdClicked += () =>
        {
            Debug.Log("Banner view was clicked.");
        };
        // Raised when an ad opened full screen content.
        _bannerView.OnAdFullScreenContentOpened += () =>
        {
            Debug.Log("Banner view full screen content opened.");
        };
        // Raised when the ad closed full screen content.
        _bannerView.OnAdFullScreenContentClosed += () =>
        {
            Debug.Log("Banner view full screen content closed.");
        };
    }
    public void DestroyBannerAd() {

        if (_bannerView!=null)
        {
            print("Destroying banner Ad");
            _bannerView.Destroy();
            _bannerView = null;
        }
    }
    #endregion

    #region Interstitial

    public void LoadInterstitialAd() {

        if (_interstitialAd!=null)
        {
            _interstitialAd.Destroy();
            _interstitialAd = null;
        }
        var adRequest = new AdRequest();
        adRequest.Keywords.Add("unity-admob-sample");

        InterstitialAd.Load(_interId, adRequest, (InterstitialAd ad, LoadAdError error) =>
        {
              if (error!=null||ad==null)
              {
                print("Interstitial ad failed to load"+error);
                return;
              }

              print("Interstitial ad loaded !!"+ad.GetResponseInfo());

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
            print("Intersititial ad not ready!!");
        }
    }
    private void InterstitialEvent(InterstitialAd ad) {
        // Raised when the ad is estimated to have earned money.
        ad.OnAdPaid += (AdValue adValue) => 
        {
            Debug.Log("Interstitial ad paid {0} {1}."+
                adValue.Value+
                adValue.CurrencyCode);
        };
        // Raised when an impression is recorded for an ad.
        ad.OnAdImpressionRecorded += () =>
        {
            Debug.Log("Interstitial ad recorded an impression.");
        };
        // Raised when a click is recorded for an ad.
        ad.OnAdClicked += () =>
        {
            Debug.Log("Interstitial ad was clicked.");
        };
        // Raised when an ad opened full screen content.
        ad.OnAdFullScreenContentOpened += () =>
        {
            Debug.Log("Interstitial ad full screen content opened.");
        };
        // Raised when the ad closed full screen content.
        ad.OnAdFullScreenContentClosed += () =>
        {
            Debug.Log("Interstitial ad full screen content closed.");
        };
        // Raised when the ad failed to open full screen content.
        ad.OnAdFullScreenContentFailed += (AdError error) =>
        {
            Debug.LogError("Interstitial ad failed to open full screen content " +
                           "with error : " + error);
        };
    }

    #endregion

    #region Rewarded

    public void LoadRewardedAd() {

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
                print("Rewarded failed to load"+error);
                return;
            }

            print("Rewarded ad loaded !!");
            _rewardedAd = ad;
            RewardedAdEvents(_rewardedAd);
        });
    }
    public void ShowRewardedAd() {

        if (_rewardedAd != null && _rewardedAd.CanShowAd())
        {
            _rewardedAd.Show(reward =>
            {
                print("Give reward to player !!");

                GrantCoins(100);
                Debug.Log("add");
            });
        }
        else {
            print("Rewarded ad not ready");
        }
    }

    private void RewardedAdEvents(RewardedAd ad)
    {
        // Raised when the ad is estimated to have earned money.
        ad.OnAdPaid += (AdValue adValue) =>
        {
            Debug.Log("Rewarded ad paid {0} {1}."+
                adValue.Value+
                adValue.CurrencyCode);
        };
        // Raised when an impression is recorded for an ad.
        ad.OnAdImpressionRecorded += () =>
        {
            Debug.Log("Rewarded ad recorded an impression.");
        };
        // Raised when a click is recorded for an ad.
        ad.OnAdClicked += () =>
        {
            Debug.Log("Rewarded ad was clicked.");
        };
        // Raised when an ad opened full screen content.
        ad.OnAdFullScreenContentOpened += () =>
        {
            Debug.Log("Rewarded ad full screen content opened.");
        };
        // Raised when the ad closed full screen content.
        ad.OnAdFullScreenContentClosed += () =>
        {
            Debug.Log("Rewarded ad full screen content closed.");
        };
        // Raised when the ad failed to open full screen content.
        ad.OnAdFullScreenContentFailed += (AdError error) =>
        {
            Debug.LogError("Rewarded ad failed to open full screen content " +
                           "with error : " + error);
        };
    }

    #endregion
    
    #region extra 

    private void GrantCoins(int coins) {
        int crrCoins = PlayerPrefs.GetInt("totalCoins");
        crrCoins += coins;
        PlayerPrefs.SetInt("totalCoins", crrCoins);

        ShowCoins();
    }
    private void ShowCoins() {
        totalCoinsTxt.text = PlayerPrefs.GetInt("totalCoins").ToString();
    }

    #endregion

}

