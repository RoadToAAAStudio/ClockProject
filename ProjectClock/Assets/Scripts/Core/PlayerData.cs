using RoadToAAA.ProjectClock.Managers;
using RoadToAAA.ProjectClock.Scriptables;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RoadToAAA.ProjectClock.Core
{
    /*
     * Holds and handles all the data relative to the player
     */

    public class PlayerData : Singleton<PlayerData>
    {
        private int _score;
        public int Score
        {
            get { return _score; }
            private set 
            { 
                _score = value;
                EventManager<int>.Instance.Publish(EEventType.OnScoreChanged, _score);
            }
        }

        private int _bestScore;
        public int BestScore
        {
            get { return _bestScore; }
            private set 
            { 
                _bestScore = value;
                DataManager.Instance.SaveInt("bestScore", _bestScore);
                EventManager<int>.Instance.Publish(EEventType.OnBestScoreChanged, _bestScore);
            }
        }

        private int _currency;
        public int Currency
        {
            get { return _currency; }
            set 
            { 
                _currency = value;
                EventManager<int>.Instance.Publish(EEventType.OnCurrencyChanged, _currency);
            }
        }

        protected override void Awake()
        {
            base.Awake();
            Score = 0;
        }

        private void OnEnable()
        {
            EventManager<EGameState, EGameState>.Instance.Subscribe(EEventType.OnGameStateChanged, UpdateBestScore);
            EventManager<ECheckResult, ComboResult>.Instance.Subscribe(EEventType.OnCheckerResult, UpdateScore);
        }

        private void OnDisable()
        {
            EventManager<EGameState, EGameState>.Instance.Unsubscribe(EEventType.OnGameStateChanged, UpdateBestScore);
            EventManager<ECheckResult, ComboResult>.Instance.Unsubscribe(EEventType.OnCheckerResult, UpdateScore);
        }

        private void Start()
        {
            // Initialize the variables with their saved values

            BestScore = DataManager.Instance.LoadInt("bestScore", 0);
            Currency = DataManager.Instance.LoadInt("currency", 0);
        }

        // Called after each tap result to update the current score with the appropriate value (according to the combo state)
        private void UpdateScore(ECheckResult checkResult, ComboResult comboResult)
        {
            if (checkResult == ECheckResult.Unsuccess) return;

            ComboState comboState = ConfigurationManager.Instance.ComboAsset.ComboStates[comboResult.StateIndex];

            Score += comboState.Score;
        }

        // Called on gameover, checks if the new score is bigger than the best score and updates its value
        //  then resets the score to 0 to ready it for the next run
        private void UpdateBestScore(EGameState oldState, EGameState newState)
        {
            if (oldState != EGameState.Playing && newState != EGameState.GameOver) return;

            if (_score > _bestScore)
                BestScore = _score;

            Score = 0;
        }
    }
}

