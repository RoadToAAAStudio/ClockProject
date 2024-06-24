using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RoadToAAA.ProjectClock.Core;
using RoadToAAA.ProjectClock.Scriptables;

namespace RoadToAAA.ProjectClock.Managers
{

    public class GameManager : MonoBehaviour
    {
        private EGameState oldState = EGameState.InitialState;
        private EGameState newState;


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
            ChangeState(EGameState.MainMenu);
        }

        private void ChangeState(EGameState state)
        {           
            newState = state;
            EventManager<EGameState, EGameState>.Instance.Publish(EEventType.OnGameStateChanged, oldState, newState);
            oldState = newState;
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
            if (oldState == EGameState.MainMenu)
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
        //Initial state
        InitialState,
        //State when the game is in main menu
        MainMenu,
        //State when you go for any reason in the effective play (new game, revive, resume from pause)
        Playing,
        //Stated when the player "dies"
        GameOver
    }
}
