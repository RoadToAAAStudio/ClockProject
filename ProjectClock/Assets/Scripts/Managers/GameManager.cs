using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RoadToAAA.ProjectClock.Core;
using RoadToAAA.ProjectClock.Scriptables;

namespace RoadToAAA.ProjectClock.Managers
{

    public class GameManager : MonoBehaviour
    {
        [SerializeField] private EGameState InitialState = EGameState.MainMenu;

        private EGameState _currentState;

        #region UnityMessages
        private void OnEnable()
        {
            EventManager<ECheckResult, ComboResult>.Instance.Subscribe(EEventType.OnCheckerResult, CheckGameOver);
            EventManager.Instance.Subscribe(EEventType.OnPlayButtonPressed, StartPlay);
            EventManager.Instance.Subscribe(EEventType.OnRetryButtonPressed, QuitPlay);
            EventManager.Instance.Subscribe(EEventType.OnReturnButtonPressed, CheckReturnFromShop);
        }
        private void OnDisable()
        {
            EventManager<ECheckResult, ComboResult>.Instance.Unsubscribe(EEventType.OnCheckerResult, CheckGameOver);
            EventManager.Instance.Unsubscribe(EEventType.OnPlayButtonPressed, StartPlay);
            EventManager.Instance.Unsubscribe(EEventType.OnRetryButtonPressed, QuitPlay);
            EventManager.Instance.Unsubscribe(EEventType.OnReturnButtonPressed, CheckReturnFromShop);
        }

        // Start is called before the first frame update
        void Start()
        {
            ChangeState(InitialState);
        }
        #endregion

        private void ChangeState(EGameState state)
        {           
            EventManager<EGameState, EGameState>.Instance.Publish(EEventType.OnGameStateChanged, _currentState, state);
            _currentState = state;
        }

        public void StartPlay()
        {
            ChangeState(EGameState.Playing);
        }
        public void QuitPlay()
        {
            ChangeState(EGameState.MainMenu);
        }

        private void GameOver()
        {
            ChangeState(EGameState.GameOver);
        }

        private void CheckGameOver(ECheckResult checkResult, ComboResult comboResult)
        {
            if (checkResult == ECheckResult.Unsuccess)
            {
                GameOver();
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
    }

    public enum EGameState
    {
        //State when the game is in main menu
        MainMenu,
        //State when you go for any reason in the effective play (new game, revive, resume from pause)
        Playing,
        //Stated when the player "dies"
        GameOver
    }
}
