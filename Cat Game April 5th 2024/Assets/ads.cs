using UnityEngine;
using UnityEngine.UI;
using GoogleMobileAds.Api;
using System;

public class GoogleMobileAdsDemoScript : MonoBehaviour
{
    public Button button; // Reference to the button component

    public void Start()
    {
        // Initialize the Google Mobile Ads SDK.
        MobileAds.Initialize((InitializationStatus initStatus) =>
        {
            CreateBannerView();
            LoadAd();


            // This callback is called once the MobileAds SDK is initialized.
        });

        button.gameObject.SetActive(true);

        // Add listener to the button click event
        button.onClick.AddListener(OnButtonClick);
    }

    // These ad units are configured to always serve test ads.
#if UNITY_ANDROID
    private string _adUnitId = "ca-app-pub-3940256099942544/6300978111";//ca-app-pub-3940256099942544/6300978111
#elif UNITY_IPHONE
    private string _adUnitId = "ca-app-pub-3940256099942544/2934735716";
#else
    private string _adUnitId = "unused";
#endif

    BannerView _bannerView;

    /// <summary>
    /// Creates a 320x50 banner view at top of the screen.
    /// </summary>
    public void CreateBannerView()
    {
        Debug.Log("Creating banner view");

        // If we already have a banner, destroy the old one.
        if (_bannerView != null)
        {
            DestroyAd();
        }

        // Create a 320x50 banner at bottom of the screen
        _bannerView = new BannerView(_adUnitId, AdSize.Banner, AdPosition.Bottom);

        ListenToAdEvents();
    }

    /// <summary>
    /// Creates the banner view and loads a banner ad.
    /// </summary>
    public void LoadAd()
    {
        // create an instance of a banner view first.
        if (_bannerView == null)
        {
            CreateBannerView();
        }

        // create our request used to load the ad.
        var adRequest = new AdRequest();

        // send the request to load the ad.
        Debug.Log("Loading banner ad.");
        _bannerView.LoadAd(adRequest);
    }

    /// <summary>
    /// listen to events the banner view may raise.
    /// </summary>
    private void ListenToAdEvents()
    {
        // Event listeners for banner view events
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
            Debug.Log(String.Format("Banner view paid {0} {1}.",
                adValue.Value,
                adValue.CurrencyCode));
        };
        _bannerView.OnAdImpressionRecorded += () =>
        {
            Debug.Log("Banner view recorded an impression.");
        };
        _bannerView.OnAdClicked += () =>
        {
            Debug.Log("Banner view was clicked.");
        };
        _bannerView.OnAdFullScreenContentOpened += () =>
        {
            Debug.Log("Banner view full screen content opened.");
        };
        _bannerView.OnAdFullScreenContentClosed += () =>
        {
            Debug.Log("Banner view full screen content closed.");
        };
    }

    /// <summary>
    /// Destroys the banner view.
    /// </summary>
    public void DestroyAd()
    {
        if (_bannerView != null)
        {
            Debug.Log("Destroying banner view.");
            _bannerView.Destroy();
            _bannerView = null;

            // Disable the button after destroying the banner
            button.gameObject.SetActive(false);
        }
    }

    // Method to handle button click event
    private void OnButtonClick()
    {
        DestroyAd();
        gameObject.SetActive(false);
    }
}
