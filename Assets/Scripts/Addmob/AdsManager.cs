using GoogleMobileAds.Api;
using TMPro;
using UnityEngine;

public class AdsManager : MonoBehaviour
{
    [SerializeField] private string _appId = "ca-app-pub-3940256099942544~3347511713";
    [SerializeField] private TextMeshProUGUI _goldTextMeshProUGUI;
    
    private BannerAds _bannerAds;
    private InterstitialAds _interstitialAds;
    private RewardedAds _rewardedAds;

    private void Start()//should init every time when open RewardUI
    {
        MobileAds.RaiseAdEventsOnUnityMainThread = true;
        MobileAds.Initialize(initStatus => {
            Debug.Log("Ads Initialised !!");
            InitializeAdManagers();
        });
    }

    private void InitializeAdManagers()
    {
        // ShowBannerAd();
        // _interstitialAds = new InterstitialAds();
        _rewardedAds = new RewardedAds();
    }

    public void ShowBannerAd()
    {
        _bannerAds = new BannerAds();
    }

    public void ShowInterstitialAd()
    {
        _interstitialAds.ShowInterstitialAd();
    }

    public void ShowRewardedAd()
    {
        _rewardedAds.ShowRewardedAd(_goldTextMeshProUGUI);
    }
}