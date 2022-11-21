using UnityEngine;
using UnityEngine.Advertisements;
using System.Collections;

public class adsManager : MonoBehaviour, IUnityAdsLoadListener, IUnityAdsShowListener
{
    string gameId = "5011365";

    [SerializeField] private GameObject player;
    [SerializeField] private DeathScreen deathScreen;
    private GameObject healthHolder;

    void Awake()
    {
        DontDestroyOnLoad(this.gameObject);
        Advertisement.Initialize(gameId, true);
    }

    void Start()
    {
        StartCoroutine(LateFindPlayer());
    }

    private IEnumerator LateFindPlayer()
    {
        yield return new WaitForSeconds(1f);
        player = GameObject.FindGameObjectWithTag("Player");
        deathScreen = GameObject.Find("DeathScreenCanvas").GetComponent<DeathScreen>();
        healthHolder = GameObject.Find("HealthHolder");
        if (player == null)
        {
            StartCoroutine(LateFindPlayer());
        }
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.F1))
        {
            ShowInterstitialAd();
        }

        if (Input.GetKeyDown(KeyCode.F2))
        {
            ShowRewardedAd();
        }
    }

    // Load content to the Ad Unit:
    public void LoadInterstitialAd()
    {
        // IMPORTANT! Only load content AFTER initialization (in this example, initialization is handled in a different script).
        Debug.Log("Loading Ad: " + "Interstitial_Android");
        Advertisement.Load("Interstitial_Android", this);
    }

    public void LoadRewardedAd()
    {
        // IMPORTANT! Only load content AFTER initialization (in this example, initialization is handled in a different script).
        Debug.Log("Loading Ad: " + "Rewarded_Android");
        Advertisement.Load("Rewarded_Android", this);
    }

    // Show the loaded content in the Ad Unit:
    public void ShowInterstitialAd()
    {
        LoadInterstitialAd();
        // Note that if the ad content wasn't previously loaded, this method will fail
        Debug.Log("Showing Ad: " + "Interstitial_Android");
        Advertisement.Show("Interstitial_Android", this);
    }

    public void ShowRewardedAd()
    {
        LoadRewardedAd();
        // Note that if the ad content wasn't previously loaded, this method will fail
        Debug.Log("Showing Ad: " + "Rewarded_Android");
        Advertisement.Show("Rewarded_Android", this);
    }
    // Implement Load Listener and Show Listener interface methods: 
    public void OnUnityAdsAdLoaded(string adUnitId)
    {
        // Optionally execute code if the Ad Unit successfully loads content.
    }

    public void OnUnityAdsFailedToLoad(string adUnitId, UnityAdsLoadError error, string message)
    {
        Debug.Log($"Error loading Ad Unit: {adUnitId} - {error.ToString()} - {message}");
        // Optionally execute code if the Ad Unit fails to load, such as attempting to try again.
    }

    public void OnUnityAdsShowFailure(string adUnitId, UnityAdsShowError error, string message)
    {
        Debug.Log($"Error showing Ad Unit {adUnitId}: {error.ToString()} - {message}");
        // Optionally execute code if the Ad Unit fails to show, such as loading another ad.
    }

    public void OnUnityAdsShowStart(string adUnitId)
    {
    }
    public void OnUnityAdsShowClick(string adUnitId) { }

    public void OnUnityAdsShowComplete(string adUnitId, UnityAdsShowCompletionState showCompletionState)
    {
        if (adUnitId.Equals("Rewarded_Android") && showCompletionState.Equals(UnityAdsShowCompletionState.COMPLETED))
        {
            Debug.Log("Unity Ads Rewarded Ad Completed");
            // Grant a reward.

            player.SetActive(true);
            healthHolder.SetActive(true);

            GameObject.Find("HeartContainers").GetComponent<HeartManager>().revivePlayer();

            player.GetComponent<PlayerMovement>().PlayerIsDead = false;
            player.GetComponent<PlayerMovement>().currentState = PlayerState.idle;
            StartCoroutine(DeathIFrames());


            Debug.Log("C");
            deathScreen.HideDeathScreen();
            FindObjectOfType<LevelMusic>().SceneMusic();

            // Load another ad:
            Advertisement.Load("Rewarded_Android", this);
        }
    }

    IEnumerator DeathIFrames(){
        player.GetComponent<PlayerMovement>().triggerCollider.enabled = false;
        yield return new WaitForSeconds(1f);
        player.GetComponent<PlayerMovement>().triggerCollider.enabled = true;
    }


}