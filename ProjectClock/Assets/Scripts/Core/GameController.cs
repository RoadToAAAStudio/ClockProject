using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RoadToAAA.ProjectClock.Core
{

    public class GameController : MonoBehaviour
    {
        private enum eGameState
        {
            MainMenu,
            Start,
            Idle,
            Resume,
            Pause,
            GameOver
        }

        private eGameState State;



        // Start is called before the first frame update
        void Start()
        {
            ChangeState(eGameState.MainMenu);
        }

        // Update is called once per frame
        void Update()
        {

        }

        private void ChangeState(eGameState state)
        {
            State = state;
            switch (State)
            {
                case eGameState.MainMenu:
                    EventManager.Instance.Publish(EventType.OnMainMenu);
                    //Time.timeScale = 0;
                    //UIController.DisablePanels();
                    //UIController.EnableTitlePanel();
                    break;

                case eGameState.Start:
                    //UIController.DisablePanels();
                    //UIController.EnablePlayerHUD();
                    // UIController.ResetScore();
                    Time.timeScale = 1;
                    SetIdleState();
                    break;

                case eGameState.Pause:
                    Time.timeScale = 0;
                    //UIController.DisablePanels();
                    //UIController.EnablePausePanel();
                    break;

                case eGameState.Resume:
                    Time.timeScale = 1;
                    //UIController.DisablePanels();
                    //UIController.EnablePlayerHUD();
                    SetIdleState();
                    break;

                case eGameState.GameOver:
                    Time.timeScale = 0;
                    //UIController.EnableGameOverPanel();
                    break;

                case eGameState.Idle:
                    break;
            }
        }

        public void StartGame()
        {
            State = eGameState.Start;
        }

        public void Resume()
        {
            State = eGameState.Resume;
        }

        public void QuitPlay()
        {
            State = eGameState.MainMenu;
        }

        private void SetIdleState()
        {
            State = eGameState.Idle;
        }

        private void GameOver()
        {
            State = eGameState.GameOver;
        }
    }
}
