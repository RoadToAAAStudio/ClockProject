using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RoadToAAA.ProjectClock.Core;
using static RoadToAAA.ProjectClock.Managers.GameManager;

namespace RoadToAAA.ProjectClock.Managers
{
    public class UiManager : MonoBehaviour
    {
        [SerializeField] private GameObject MainMenuPanel;
        [SerializeField] private GameObject GameOverPanel;
        [SerializeField] private GameObject ShopPanel;
        [SerializeField] private GameObject LeaderboardPanel;

        [SerializeField] private GameObject GameplayPanel;

        private void OnEnable()
        {
            EventManager<EGameState, EGameState>.Instance.Subscribe(EEventType.OnGameStateChanged, UpdateUIFromGameState);
            EventManager.Instance.Subscribe(EEventType.OnShopButtonPressed, OpenShop);
            EventManager.Instance.Subscribe(EEventType.OnReturnButtonPressed, OpenMainMenu);
            EventManager.Instance.Subscribe(EEventType.OnLeaderboardButtonPressed, OpenLeaderboard);
            EventManager.Instance.Subscribe(EEventType.OnLeaderboardReturnButtonPressed, OpenMainMenu);
            EventManager.Instance.Subscribe(EEventType.OnRetryButtonPressed, OpenMainMenu);
            EventManager.Instance.Subscribe(EEventType.OnPlayButtonPressed, StartPlaying);
        }

        private void OnDisable()
        {
            EventManager<EGameState, EGameState>.Instance.Unsubscribe(EEventType.OnGameStateChanged, UpdateUIFromGameState);
            EventManager.Instance.Unsubscribe(EEventType.OnShopButtonPressed, OpenShop);
            EventManager.Instance.Unsubscribe(EEventType.OnReturnButtonPressed, OpenMainMenu);
            EventManager.Instance.Unsubscribe(EEventType.OnLeaderboardButtonPressed, OpenLeaderboard);
            EventManager.Instance.Unsubscribe(EEventType.OnLeaderboardReturnButtonPressed, OpenMainMenu);
            EventManager.Instance.Unsubscribe(EEventType.OnRetryButtonPressed, OpenMainMenu);
            EventManager.Instance.Unsubscribe(EEventType.OnPlayButtonPressed, StartPlaying);
        }

        private void UpdateUIFromGameState(EGameState oldState, EGameState newState)
        {
            switch (newState)
            {
                case EGameState.MainMenu:
                OpenMainMenu();
                return;

                case EGameState.Playing:
                StartPlaying();
                return;

                case EGameState.GameOver:
                OpenGameOver();
                return;
            }
        }

        private void OpenMainMenu()
        {
            DisableAllPanels();
            MainMenuPanel.SetActive(true);
        }

        private void OpenGameOver()
        {
            DisableAllPanels();
            GameOverPanel.SetActive(true);
        }

        private void OpenShop()
        {
            DisableAllPanels();
            ShopPanel.SetActive(true);
        }

        private void OpenLeaderboard()
        {
            DisableAllPanels();
            LeaderboardPanel.SetActive(true);
        }

        private void StartPlaying()
        {
            DisableAllPanels();
            GameplayPanel.SetActive(true);
        }

        private void DisableAllPanels()
        {
            MainMenuPanel.SetActive(false);
            GameOverPanel.SetActive(false);
            ShopPanel.SetActive(false);
            LeaderboardPanel.SetActive(false);
            GameplayPanel.SetActive(false);
        }
    }
}