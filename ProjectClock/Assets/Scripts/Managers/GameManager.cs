using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RoadToAAA.ProjectClock.Core
{

    public class GameManager : MonoBehaviour
    {
        public enum EGameState
        {
            MainMenu,
            Start,
            Idle,
            Resume,
            Pause,
            GameOver
        }

        private EGameState State;



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
            State = state;
            switch (State)
            {
                case EGameState.MainMenu:
                    EventManager.Instance.Publish(EventType.OnMainMenu);
                    //Time.timeScale = 0;
                    //UIController.DisablePanels();
                    //UIController.EnableTitlePanel();
                    break;

                case EGameState.Start:
                    //UIController.DisablePanels();
                    //UIController.EnablePlayerHUD();
                    // UIController.ResetScore();
                    Time.timeScale = 1;
                    SetIdleState();
                    break;

                case EGameState.Pause:
                    Time.timeScale = 0;
                    //UIController.DisablePanels();
                    //UIController.EnablePausePanel();
                    break;

                case EGameState.Resume:
                    Time.timeScale = 1;
                    //UIController.DisablePanels();
                    //UIController.EnablePlayerHUD();
                    SetIdleState();
                    break;

                case EGameState.GameOver:
                    Time.timeScale = 0;
                    //UIController.EnableGameOverPanel();
                    break;

                case EGameState.Idle:
                    break;
            }
        }

        public void StartGame()
        {
            State = EGameState.Start;
        }

        public void Resume()
        {
            State = EGameState.Resume;
        }

        public void QuitPlay()
        {
            State = EGameState.MainMenu;
        }

        private void SetIdleState()
        {
            State = EGameState.Idle;
        }

        private void GameOver()
        {
            State = EGameState.GameOver;
        }
    }
}
