using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RoadToAAA.ProjectClock.Utilities;
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
            EventManager.Instance.Subscribe(EEventType.OnShopReturnButtonPressed, OpenMainMenu);
            EventManager.Instance.Subscribe(EEventType.OnLeaderboardButtonPressed, OpenLeaderboard);
            EventManager.Instance.Subscribe(EEventType.OnLeaderboardReturnButtonPressed, OpenMainMenu);
        }

        private void OnDisable()
        {
            EventManager<EGameState, EGameState>.Instance.Unsubscribe(EEventType.OnGameStateChanged, UpdateUIFromGameState);
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