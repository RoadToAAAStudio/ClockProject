using RoadToAAA.ProjectClock.Managers;
using RoadToAAA.ProjectClock.Scriptables;
using System;
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

        private int _currentPaletteIndex;
        public int CurrentPaletteIndex
        {
            get { return _currentPaletteIndex; }
            set
            {
                _currentPaletteIndex = value;
                EventManager<int>.Instance.Publish(EEventType.OnCurrentPaletteChanged, _currentPaletteIndex);
            }
        }

        private int _selectedPaletteIndex;
        public int SelectedPaletteIndex
        {
            get { return _selectedPaletteIndex; }
            set
            {
                _selectedPaletteIndex = value;
            }
        }

        private int _previewPaletteIndex;
        public int PreviewPaletteIndex
        {
            get { return _previewPaletteIndex; }
            set
            {
                float oldPreviewIndex = _previewPaletteIndex;

                _previewPaletteIndex = value;
             
                if (_previewPaletteIndex != oldPreviewIndex)
                {
                    CurrentPaletteIndex = _previewPaletteIndex;
                }
            }
        }

        #region Initialization
        protected override void Awake()
        {
            base.Awake();
            Score = 0;
            SelectedPaletteIndex = 0;
            _previewPaletteIndex = 0;

        }

        private void OnEnable()
        {
            EventManager<EGameState, EGameState>.Instance.Subscribe(EEventType.OnGameStateChanged, UpdateBestScore);
            EventManager<ECheckResult, ComboResult>.Instance.Subscribe(EEventType.OnCheckerResult, UpdateScore);
            EventManager.Instance.Subscribe(EEventType.OnReturnButtonPressed, UpdateCurrentPalette);
            EventManager.Instance.Subscribe(EEventType.OnShopButtonPressed, UpdatePreviewPalette);
        }

        private void OnDisable()
        {
            EventManager<EGameState, EGameState>.Instance.Unsubscribe(EEventType.OnGameStateChanged, UpdateBestScore);
            EventManager<ECheckResult, ComboResult>.Instance.Unsubscribe(EEventType.OnCheckerResult, UpdateScore);
            EventManager.Instance.Unsubscribe(EEventType.OnReturnButtonPressed, UpdateCurrentPalette);
            EventManager.Instance.Unsubscribe(EEventType.OnShopButtonPressed, UpdatePreviewPalette);
        }

        private void Start()
        {
            // Initialize the variables with their saved values

            BestScore = DataManager.Instance.LoadInt("bestScore", 0);
            Currency = DataManager.Instance.LoadInt("currency", 0);
        }
        #endregion

        #region Score
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
        #endregion

        public void SetSelectedPalette()
        {
            if (_previewPaletteIndex != SelectedPaletteIndex)
            {
                SelectedPaletteIndex = _previewPaletteIndex;
            }
        }

        private void UpdateCurrentPalette()
        {
            if (_previewPaletteIndex != _selectedPaletteIndex)
            {
                CurrentPaletteIndex = _selectedPaletteIndex;
            }
        }

        private void UpdatePreviewPalette()
        {
            _previewPaletteIndex = SelectedPaletteIndex;
        }
    }
}

