using RoadToAAA.ProjectClock.Scriptables;
using RoadToAAA.ProjectClock.Utilities;
using RoadToAAA.ProjectClock.Core;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace RoadToAAA.ProjectClock.Managers
{
    /*
     * It manages all clock in the scene 
     */
    public class ClockManager : MonoBehaviour 
    {
        // Configs
        private SpawnerAsset SpawnerAsset;

        // Combonents
        private ClockSpawner _clockSpawner;
        private ClockChecker _clockChecker;
        private ClockComboHandler _clockComboHandler;
        
        // Runtime variables
        private List<Clock> _clocks;
        private int _currentClockIndex;

        private Clock _firstClock => _clocks.Count > 0 ? _clocks[0] : null;
        private Clock _currentClock => _clocks.Count > 0 ? _clocks[_currentClockIndex] : null;
        private Clock _lastClock => _clocks.Count > 0 ? _clocks[_clocks.Count - 1] : null;

        #region UnityMessages
        private void Awake()
        {
            // Configs
            SpawnerAsset = ConfigurationManager.Instance.SpawnerAsset;

            // Components
            _clockSpawner = new ClockSpawner();
            _clockChecker = new ClockChecker();
            _clockComboHandler = new ClockComboHandler();

            _clocks = new List<Clock>();
        }

        private void OnEnable()
        {
            EventManager<EGameState, EGameState>.Instance.Subscribe(EEventType.OnGameStateChanged, GameStateChanged);
            EventManager.Instance.Subscribe(EEventType.OnPlayTap, PlayTapPerformed);
        }

        private void OnDisable()
        {
            EventManager<EGameState, EGameState>.Instance.Unsubscribe(EEventType.OnGameStateChanged, GameStateChanged);
            EventManager.Instance.Unsubscribe(EEventType.OnPlayTap, PlayTapPerformed);
        }

        private void Update()
        {
            Clock currentClock = _currentClock;
            if (currentClock == null) return;
            if (currentClock.State != EClockState.Activated) return;
            float handAngle = currentClock.HandTransform.rotation.eulerAngles.z + currentClock.AngularSpeed * Time.deltaTime;
            currentClock.DrawHand(handAngle);

        }
        #endregion

        private void GameStateChanged(EGameState oldState, EGameState newState)
        {
            switch (newState)
            {
                case EGameState.MainMenu:
                    Initialize();
                    break;
                case EGameState.GameOver:
                    DeactivateClocks();
                    break;
            }
        }

        private void PlayTapPerformed()
        {
            Debug.Assert(_currentClock != null, "Current clock object is null!");

            // Take Check result
            ECheckResult checkResult = _clockChecker.Check(_currentClock);         
            switch (checkResult) 
            { 
                case ECheckResult.Success:
                case ECheckResult.Perfect:
                    SelectNewClock();
                    EventManager<Clock, Clock>.Instance.Publish(EEventType.OnNewClockSelected, _currentClock, _clocks[_currentClockIndex - 1]);
                    break;

                case ECheckResult.Unsuccess:
                    break;
            }

            // Take Combo Result
            ComboResult comboResult = _clockComboHandler.HandleCheckResult(checkResult);
            EventManager<ECheckResult, ComboResult>.Instance.Publish(EEventType.OnCheckerResult, checkResult, comboResult);
        }

        private void Initialize()
        {
            // Initialize components
            _clockSpawner.Initialize();
            _clockChecker.Initialize();
            _clockComboHandler.Initialize();

            // Initialize Manager
            // Despawn all clocks
            while (_clocks.Count > 0)
            {
                DespawnClockInFirstPosition();
            }

            _currentClockIndex = 0;

            // Spawn a certain amount of Clock
            for (int i = 0; i < SpawnerAsset.ClockPoolSize; i++)
            {
                SpawnClockInLastPosition();
            }

            // Rendering
            _currentClock.ActivateHand();
            _currentClock.DrawHand(_currentClock.GetHandAngle());
        }

        private void DeactivateClocks()
        {
            for (int i = 0; i < _clocks.Count; i++) 
            { 
                Clock clock = _clocks[i];
                clock.DeactivateClock();
                clock.DrawClock();
                clock.DrawHand(clock.GetHandAngle());
            }
        }

        private void SelectNewClock()
        {
            // Rendering
            _currentClock.DeactivateHand();
            _currentClock.DrawHand(_currentClock.GetHandAngle());

            if (_currentClockIndex < SpawnerAsset.RenderingDistance)
            {
                _currentClockIndex++;
            }
            else
            {
                DespawnClockInFirstPosition();
                SpawnClockInLastPosition();
            }

            // Rendering
            _currentClock.ActivateHand();
            _currentClock.DrawHand(_currentClock.GetHandAngle());
        }

        private void SpawnClockInLastPosition()
        {
            Clock generatedClock = _clockSpawner.SpawnClock(_lastClock);

            Debug.Assert(generatedClock != null, "Generated clock is null!");

            _clocks.Add(generatedClock);
        }

        private void DespawnClockInFirstPosition()
        {
            Debug.Assert(_firstClock != null, "First clock object is null!");

            Clock firstClock = _firstClock;
            _clocks.Remove(firstClock);
            _clockSpawner.DespawnClock(firstClock);
        }
    }
}
