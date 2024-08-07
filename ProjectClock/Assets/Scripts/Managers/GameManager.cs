using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RoadToAAA.ProjectClock.Core;
using RoadToAAA.ProjectClock.Scriptables;
using System;

namespace RoadToAAA.ProjectClock.Managers
{
    public class GameManager : MonoBehaviour
    {
        [SerializeField] private EGameState InitialState = EGameState.MainMenu;

        private EGameState _currentState;

        #region UnityMessages
        private void OnEnable()
        {
            EventManager.Instance.Subscribe(EEventType.OnClockSuccessZonePassed, SuccessZonePassed);
            EventManager<ECheckResult, ComboResult>.Instance.Subscribe(EEventType.OnCheckerResult, CheckGameOver);
            EventManager.Instance.Subscribe(EEventType.OnTutorialGotItButtonPressed, TutorialGotItButtonPressed);
            EventManager.Instance.Subscribe(EEventType.OnPlayButtonPressed, PlayButtonPressed);
            EventManager.Instance.Subscribe(EEventType.OnTutorialButtonPressed, TutorialButtonPressed);
            EventManager.Instance.Subscribe(EEventType.OnMainMenuButtonClicked, ToMainMenuButtonPressed);
            EventManager.Instance.Subscribe(EEventType.OnReturnButtonPressed, CheckReturnFromShop);

            DataRequestManager<EGameState>.Instance.Subscribe(ERequestType.GameStateRequest, ProvideGameState);
        }

        private void OnDisable()
        {
            EventManager.Instance.Unsubscribe(EEventType.OnClockSuccessZonePassed, SuccessZonePassed);
            EventManager<ECheckResult, ComboResult>.Instance.Unsubscribe(EEventType.OnCheckerResult, CheckGameOver);
            EventManager.Instance.Unsubscribe(EEventType.OnTutorialGotItButtonPressed, TutorialGotItButtonPressed);
            EventManager.Instance.Unsubscribe(EEventType.OnPlayButtonPressed, PlayButtonPressed);
            EventManager.Instance.Unsubscribe(EEventType.OnTutorialButtonPressed, TutorialButtonPressed);
            EventManager.Instance.Unsubscribe(EEventType.OnMainMenuButtonClicked, ToMainMenuButtonPressed);
            EventManager.Instance.Unsubscribe(EEventType.OnReturnButtonPressed, CheckReturnFromShop);

            DataRequestManager<EGameState>.Instance.Unsubscribe(ERequestType.GameStateRequest, ProvideGameState);
        }

        // Start is called before the first frame update
        void Start()
        {
            if (InitialState == EGameState.Tutorial)
            {
                if (PlayerDataManager.Instance.IsFirstTimeApplicationIsStarted)
                {
                    ChangeState(EGameState.Tutorial);
                }
                else
                {
                    ChangeState(EGameState.MainMenu);
                }
            }
            else
            {
                ChangeState(InitialState);
            }
        }
        #endregion

        private void ChangeState(EGameState state)
        {           
            EventManager<EGameState, EGameState>.Instance.Publish(EEventType.OnGameStateChanged, _currentState, state);
            _currentState = state;
        }

        private void SuccessZonePassed()
        {
            ChangeState(EGameState.GameOver);
        }

        private void TutorialGotItButtonPressed()
        {
            ChangeState(EGameState.MainMenu);
        }

        private void TutorialButtonPressed()
        {
            ChangeState(EGameState.Tutorial);
        }

        private void PlayButtonPressed()
        {
            ChangeState(EGameState.Playing);
        }

        private void ToMainMenuButtonPressed()
        {
            ChangeState(EGameState.MainMenu);
        }

        private void CheckGameOver(ECheckResult checkResult, ComboResult comboResult)
        {
            if (checkResult == ECheckResult.Unsuccess)
            {
                ChangeState(EGameState.GameOver);
            }
        }

        private void CheckReturnFromShop()
        {
            if (_currentState == EGameState.MainMenu)
            {
                return;
            }
            else
            {
                ChangeState(EGameState.MainMenu);
            }
        }

        private EGameState ProvideGameState()
        {
            return _currentState;
        }
    }

    public enum EGameState
    {
        //Non valid
        None,
        //Only the first time the application is started
        Tutorial,
        //State when the game is in main menu
        MainMenu,
        //State when you go for any reason in the effective play (new game, revive, resume from pause)
        Playing,
        //Stated when the player "dies"
        GameOver
    }
}
