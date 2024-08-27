using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RoadToAAA.ProjectClock.Core;

namespace RoadToAAA.ProjectClock.Managers
{
    public class UIManager : MonoBehaviour
    {
        [SerializeField] private GameObject MainMenuPanel;
        [SerializeField] private GameObject GameOverPanel;
        [SerializeField] private GameObject ShopPanel;
        [SerializeField] private GameObject LeaderboardPanel;
        [SerializeField] private GameObject GameplayPanel;
        [SerializeField] private GameObject TutorialPanel;

        private void Awake()
        {
            DisableAllPanels();
        }

        private void OnEnable()
        {
            EventManager<EGameState, EGameState>.Instance.Subscribe(EEventType.OnGameStateChanged, UpdateUIFromGameState);
            EventManager.Instance.Subscribe(EEventType.OnShopButtonPressed, OpenShop);
            EventManager.Instance.Subscribe(EEventType.OnReturnButtonPressed, OpenMainMenu);
            EventManager.Instance.Subscribe(EEventType.OnLeaderboardButtonPressed, OpenLeaderboard);
            EventManager.Instance.Subscribe(EEventType.OnLeaderboardReturnButtonPressed, OpenMainMenu);
            EventManager.Instance.Subscribe(EEventType.OnMainMenuButtonClicked, OpenMainMenu);
            EventManager.Instance.Subscribe(EEventType.OnPlayButtonPressed, StartPlaying);
        }

        private void OnDisable()
        {
            EventManager<EGameState, EGameState>.Instance.Unsubscribe(EEventType.OnGameStateChanged, UpdateUIFromGameState);
            EventManager.Instance.Unsubscribe(EEventType.OnShopButtonPressed, OpenShop);
            EventManager.Instance.Unsubscribe(EEventType.OnReturnButtonPressed, OpenMainMenu);
            EventManager.Instance.Unsubscribe(EEventType.OnLeaderboardButtonPressed, OpenLeaderboard);
            EventManager.Instance.Unsubscribe(EEventType.OnLeaderboardReturnButtonPressed, OpenMainMenu);
            EventManager.Instance.Unsubscribe(EEventType.OnMainMenuButtonClicked, OpenMainMenu);
            EventManager.Instance.Unsubscribe(EEventType.OnPlayButtonPressed, StartPlaying);
        }

        private void UpdateUIFromGameState(EGameState oldState, EGameState newState)
        {
            switch (newState)
            {
                case EGameState.Tutorial:
                    OpenTutorial();
                    break;

                case EGameState.MainMenu:
                    OpenMainMenu();
                    break;

                case EGameState.Playing:
                    StartPlaying();
                    break;

                case EGameState.GameOver:
                    OpenGameOver();
                    break;
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

        private void OpenTutorial()
        {
            DisableAllPanels();
            TutorialPanel.SetActive(true);
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
            TutorialPanel.SetActive(false);
        }
    }
}