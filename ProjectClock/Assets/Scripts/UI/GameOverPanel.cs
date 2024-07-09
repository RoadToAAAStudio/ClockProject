using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RoadToAAA.ProjectClock.Core;
using TMPro;
using UnityEngine.UI;
using System;

namespace RoadToAAA.ProjectClock.UI
{
    public class GameOverPanel : MonoBehaviour
    {
        [SerializeField] private Button _mainMenuButton;
        [SerializeField] private Button _adsButton;
        [SerializeField] private TextMeshProUGUI _scoreText;
        [SerializeField] private TextMeshProUGUI _highScoreText;

        private void OnEnable()
        {
            Initialize();

            _mainMenuButton.onClick.AddListener(MainMenuButtonClicked);
            _adsButton.onClick.AddListener(AdsButtonClicked);

            EventManager.Instance.Subscribe(EEventType.OnAdLoaded, AdLoaded);
        }

        private void OnDisable()
        {
            _mainMenuButton.onClick.RemoveAllListeners();
            _adsButton.onClick.RemoveAllListeners();

            EventManager.Instance.Unsubscribe(EEventType.OnAdLoaded, AdLoaded);
        }

        private void Initialize()
        {
            _scoreText.text = PlayerDataManager.Instance.Score.ToString();
            _highScoreText.text = PlayerDataManager.Instance.BestScore.ToString();
            _adsButton.interactable = false;
        }

        private void MainMenuButtonClicked()
        {
            EventManager.Instance.Publish(EEventType.OnMainMenuButtonClicked);
        }

        private void AdsButtonClicked()
        {
            _adsButton.interactable = false;
            EventManager.Instance.Publish(EEventType.OnAdsButtonClicked);
        }

        private void AdLoaded()
        {
            _adsButton.interactable = true;
        }
    }
}
