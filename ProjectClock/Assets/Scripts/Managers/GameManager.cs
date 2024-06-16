using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RoadToAAA.ProjectClock.Utilities;

namespace RoadToAAA.ProjectClock.Managers
{

    public class GameManager : MonoBehaviour
    {
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

        private EGameState oldState = EGameState.InitialState;
        private EGameState newState;


        private void OnEnable()
        {
            EventManager.Instance.Subscribe(EEventType.OnMenuTap, StartPlay);
        }
        private void OnDisable()
        {
            EventManager.Instance.Unsubscribe(EEventType.OnMenuTap, StartPlay);
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
    }
}
