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



        // Start is called before the first frame update
        void Start()
        {
            ChangeState(EGameState.MainMenu);
        }

        // Update is called once per frame
        void Update()
        {

        }

        private void ChangeState(EGameState state)
        {           
            newState = state;
            EventManager<EGameState, EGameState>.Instance.Publish(EEventType.OnGameStateChanged, oldState, newState);
            oldState = newState;
        }

        public void QuitPlay()
        {
            newState = EGameState.MainMenu;
        }

        private void SetPlayingState()
        {
            newState = EGameState.Playing;
        }

        private void GameOver()
        {
            newState = EGameState.GameOver;
        }
    }
}
