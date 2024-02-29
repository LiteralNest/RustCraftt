using Cysharp.Threading.Tasks;
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

    // private void Start()//should init every time when open RewardUI
    // {
    //     MobileAds.RaiseAdEventsOnUnityMainThread = true;
    //     MobileAds.Initialize(initStatus => {
    //         Debug.Log("Ads Initialised !!");
    //         InitializeAdManagers();
    //     });
    // }

    private UniTask InitializeAdManagers()
    {
        // ShowBannerAd();
        // _interstitialAds = new InterstitialAds();
    
        MobileAds.RaiseAdEventsOnUnityMainThread = true;
        MobileAds.Initialize(initStatus => {
            Debug.Log("Ads Initialised !!");
            _rewardedAds = new RewardedAds();
        });

        return UniTask.CompletedTask; 
    }

    public void ShowBannerAd()
    {
        _bannerAds = new BannerAds();
    }

    public void ShowInterstitialAd()
    {
        _interstitialAds.ShowInterstitialAd();
    }

    public async void ShowRewardedAd()
    {
        await InitializeAdManagers();
        _rewardedAds.ShowRewardedAd(_goldTextMeshProUGUI);
    }
}

