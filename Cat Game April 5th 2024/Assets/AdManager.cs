using UnityEngine;
using UnityEngine.UI;
using GoogleMobileAds.Api;
using System;
using UnityEngine.UIElements;

public class AdManager : MonoBehaviour
{
    public static AdManager Instance { get; private set; }

    private BannerView _bannerView;
    private bool adEnabled = true; // Initially set to true

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void InitializeAd()
    {
        MobileAds.Initialize((InitializationStatus initStatus) =>
        {
            CreateBannerView();
            LoadAd();
        });
    }

    public void ToggleAdVisibility()
    {
        adEnabled = !adEnabled;
        if (adEnabled)
        {
            _bannerView.Show();
        }
        else
        {
            _bannerView.Hide();
        }
    }

    private void CreateBannerView()
    {
        Debug.Log("Creating banner view");

        // If we already have a banner, destroy the old one.
        if (_bannerView != null)
        {
            DestroyAd();
        }

        // Create a 320x50 banner at bottom of the screen
        _bannerView = new BannerView("ca-app-pub-3940256099942544/6300978111", AdSize.Banner, AdPosition.Bottom);

        ListenToAdEvents();
    }

    private void LoadAd()
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

    private void ListenToAdEvents()
    {
        // Event listeners for banner view events
        // Add your event listeners here
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

    public void DestroyAd()
    {
        if (_bannerView != null)
        {
            Debug.Log("Destroying banner view.");
            _bannerView.Destroy();
            _bannerView = null;

        }
    }

}
