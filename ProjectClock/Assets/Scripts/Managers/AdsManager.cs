using UnityEngine.Advertisements;
using UnityEngine;
using RoadToAAA.ProjectClock.Core;

namespace RoadToAAA.ProjectClock.Managers
{
    public class AdsManager : MonoBehaviour, IUnityAdsInitializationListener, IUnityAdsLoadListener, IUnityAdsShowListener
    {
        [SerializeField] private string _androidGameId;
        [SerializeField] private bool _testMode = true;
        private string _gameId;

        #region UnityMessages
        void Awake()
        {
#if UNITY_ANDROID
            _gameId = _androidGameId;
#elif UNITY_EDITOR
            _gameId = _androidGameId; //Only for testing the functionality in the Editor
#endif
            if (!Advertisement.isInitialized && Advertisement.isSupported)
            {
                Advertisement.Initialize(_gameId, _testMode, this);
            }
        }

        private void OnEnable()
        {
            EventManager<EGameState, EGameState>.Instance.Subscribe(EEventType.OnGameStateChanged, GameStateChanged);
            EventManager.Instance.Subscribe(EEventType.OnAdsButtonClicked, AdsButtonClicked);
        }

        private void OnDisable()
        {
            EventManager<EGameState, EGameState>.Instance.Unsubscribe(EEventType.OnGameStateChanged, GameStateChanged);
            EventManager.Instance.Unsubscribe(EEventType.OnAdsButtonClicked, AdsButtonClicked);
        }

        #endregion

        public void OnInitializationComplete()
        {
#if UNITY_EDITOR
            Debug.Log("Unity Ads initialization complete.");
#endif
        }

        public void OnInitializationFailed(UnityAdsInitializationError error, string message)
        {
#if UNITY_EDITOR
            Debug.Log($"Unity Ads Initialization Failed: {error.ToString()} - {message}");
#endif
        }

        public void OnUnityAdsAdLoaded(string placementId)
        {
            EventManager.Instance.Publish(EEventType.OnAdLoaded);
        }

        public void OnUnityAdsFailedToLoad(string placementId, UnityAdsLoadError error, string message)
        {
            throw new System.NotImplementedException();
        }

        public void OnUnityAdsShowComplete(string placementId, UnityAdsShowCompletionState showCompletionState)
        {
            switch (showCompletionState)
            {
                case UnityAdsShowCompletionState.COMPLETED:
                    Debug.Log("AD Completed!");
                    break;
                case UnityAdsShowCompletionState.SKIPPED:
                    Debug.Log("AD Skipped!");
                    break;
                case UnityAdsShowCompletionState.UNKNOWN:
                    Debug.Log("AD ended with unknown state!");
                    break;

            }
        }

        public void OnUnityAdsShowFailure(string placementId, UnityAdsShowError error, string message)
        {
            Debug.Log($"Error showing Ad Unit {"Rewarded_Android"}: {error.ToString()} - {message}");
        }

        public void OnUnityAdsShowStart(string placementId) { }

        public void OnUnityAdsShowClick(string placementId) { }

        private void GameStateChanged(EGameState oldState, EGameState newState)
        {
            if (newState != EGameState.GameOver) return;

            Advertisement.Load("Rewarded_Android", this);
            Debug.Log("Load AD");
        }

        private void AdsButtonClicked()
        {
            Advertisement.Show("Rewarded_Android", this);
            Debug.Log("Show AD");
        }
    }
}