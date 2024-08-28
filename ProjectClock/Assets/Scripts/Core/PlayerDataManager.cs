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

    public class PlayerDataManager : Singleton<PlayerDataManager>
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

        private int _runCurrency;
        public int RunCurrency
        {
            get { return _runCurrency; }
            set
            {
                _runCurrency = value;
                EventManager<int>.Instance.Publish(EEventType.OnRunCurrencyChanged, _runCurrency);
            }
        }

        private int _currentPaletteIndex;
        public int CurrentPaletteIndex
        {
            get { return _currentPaletteIndex; }
            set
            {
                _currentPaletteIndex = value;
                DataManager.Instance.SaveInt("currentPalette", _currentPaletteIndex);
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

        private List<int> _unlockedPalettes;
        private int _unlockedPalettesNumber = 0;

        private bool _isFirstTimeApplicationIsStarted = true;
        public bool IsFirstTimeApplicationIsStarted
        {
            get { return _isFirstTimeApplicationIsStarted; }
            set { _isFirstTimeApplicationIsStarted = value; }
        }

        #region Initialization
        protected override void Awake()
        {
            base.Awake();
            Score = 0;
            RunCurrency = 0;
        }

        private void OnEnable()
        {
            EventManager<EGameState, EGameState>.Instance.Subscribe(EEventType.OnGameStateChanged, UpdateBestScore);
            EventManager<EGameState, EGameState>.Instance.Subscribe(EEventType.OnGameStateChanged, UpdateCurrency);
            EventManager<EGameState, EGameState>.Instance.Subscribe(EEventType.OnGameStateChanged, ResetRunData);
            EventManager<ECheckResult, ComboResult>.Instance.Subscribe(EEventType.OnCheckerResult, UpdateScore);
            EventManager<int>.Instance.Subscribe(EEventType.OnSpecialClockCleared, UpdateCurrency);
            EventManager.Instance.Subscribe(EEventType.OnAdCompleted, ApplyAdReward);
            EventManager.Instance.Subscribe(EEventType.OnReturnButtonPressed, UpdateCurrentPalette);
            EventManager.Instance.Subscribe(EEventType.OnShopButtonPressed, UpdatePreviewPalette);
        }

        private void OnDisable()
        {
            EventManager<EGameState, EGameState>.Instance.Unsubscribe(EEventType.OnGameStateChanged, UpdateBestScore);
            EventManager<EGameState, EGameState>.Instance.Unsubscribe(EEventType.OnGameStateChanged, UpdateCurrency);
            EventManager<EGameState, EGameState>.Instance.Unsubscribe(EEventType.OnGameStateChanged, ResetRunData);
            EventManager<ECheckResult, ComboResult>.Instance.Unsubscribe(EEventType.OnCheckerResult, UpdateScore);
            EventManager<int>.Instance.Unsubscribe(EEventType.OnSpecialClockCleared, UpdateCurrency);
            EventManager.Instance.Unsubscribe(EEventType.OnAdCompleted, ApplyAdReward);
            EventManager.Instance.Unsubscribe(EEventType.OnReturnButtonPressed, UpdateCurrentPalette);
            EventManager.Instance.Unsubscribe(EEventType.OnShopButtonPressed, UpdatePreviewPalette);
        }

        private void Start()
        {
            // Initialize the variables with their saved values

            BestScore = DataManager.Instance.LoadInt("bestScore", 0);
            Currency = DataManager.Instance.LoadInt("currency", 0);
            CurrentPaletteIndex = DataManager.Instance.LoadInt("currentPalette", 0);
            SelectedPaletteIndex = CurrentPaletteIndex;
            _previewPaletteIndex = CurrentPaletteIndex;

            InitializeUnlockedPalettesList();

            IsFirstTimeApplicationIsStarted = DataManager.Instance.LoadInt("isFirstTimeApplicationIsStarted", 1) != 0;
        }
        #endregion

        public void ClearSavedData()
        {
            DataManager.Instance.SaveInt("bestScore", 0);
            DataManager.Instance.SaveInt("currency", 0);
            for (int i = 0; i <= _unlockedPalettesNumber; i++)
            {
                DataManager.Instance.ClearData("palette" + i);
            }
            DataManager.Instance.ClearData("unlockedPalettesNumber");
            DataManager.Instance.ClearData("isFirstTimeApplicationIsStarted");
        }

        private void ResetRunData(EGameState oldState, EGameState newState)
        {
            if (oldState != EGameState.MainMenu || newState != EGameState.Playing) return;

            Score = 0;
            RunCurrency = 0;
        }

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
            if (oldState != EGameState.Playing || newState != EGameState.GameOver) return;

            if (_score > _bestScore)
                BestScore = _score;
        }
        #endregion

        #region Currency
        // Called on gameover, save currency
        private void UpdateCurrency(EGameState oldState, EGameState newState)
        {
            if (oldState != EGameState.Playing || newState != EGameState.GameOver) return;

            Currency += RunCurrency;

            DataManager.Instance.SaveInt("currency", _currency);
        }

        private void UpdateCurrency(int currencyObtained)
        {
            //Currency += currencyObtained;
            RunCurrency += currencyObtained;
        }

        private void ApplyAdReward()
        {
            AdRewardAsset adRewardAsset = ConfigurationManager.Instance.AdRewardAsset;

            switch(adRewardAsset.RewardType)
            {
                case ERewardType.SUM:
                    Currency += (int)adRewardAsset.Amount;
                    break;
                case ERewardType.MULTIPLY:
                    Currency += (int)(RunCurrency * (adRewardAsset.Amount - 1));
                    break;
            }
        }
        #endregion

        #region Palette
        public void SetSelectedPalette()
        {
            if (IsPaletteUnlocked(_previewPaletteIndex))
            {
                if (_previewPaletteIndex != SelectedPaletteIndex)
                {
                    SelectedPaletteIndex = _previewPaletteIndex;
                }
            }
            else
            {
                BuyPalette();
            }
        }

        private void BuyPalette()
        {
            // Update the currency
            Currency -= ConfigurationManager.Instance.PaletteAssets[_previewPaletteIndex].Cost;
            DataManager.Instance.SaveInt("currency", _currency);

            // Add the index of the newly bought palette into the list of unlocked palettes and save it in its correct position
            _unlockedPalettes.Add(_previewPaletteIndex);
            _unlockedPalettesNumber++;
            DataManager.Instance.SaveInt("unlockedPalettesNumber", _unlockedPalettesNumber);
            DataManager.Instance.SaveInt("palette" + _unlockedPalettesNumber, _previewPaletteIndex);

            // Select the new palette
            SelectedPaletteIndex = _previewPaletteIndex;
            EventManager<int>.Instance.Publish(EEventType.OnNewPaletteBought, _previewPaletteIndex);
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

        public bool IsPaletteUnlocked(int index)
        {
            for (int i = 0; i < _unlockedPalettes.Count; i++)
            {
                if (_unlockedPalettes[i] == index)
                {
                    return true;
                }
            }
            return false;
        }

        private void InitializeUnlockedPalettesList()
        {
            _unlockedPalettes = new();
            _unlockedPalettesNumber = DataManager.Instance.LoadInt("unlockedPalettesNumber", 0);
            for (int i = 0; i <= _unlockedPalettesNumber; i++)
            {
                _unlockedPalettes.Add(DataManager.Instance.LoadInt("palette" + i, 0));
            }
        }
        #endregion
    }
}