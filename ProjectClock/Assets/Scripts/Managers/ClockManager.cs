using RoadToAAA.ProjectClock.Scriptables;
using RoadToAAA.ProjectClock.Utilities;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
        private ClocksMover _clocksMover;
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
            // Components
            _clockSpawner = new ClockSpawner();
            _clockChecker = new ClockChecker();
            _clocksMover = new ClocksMover();
            _clockComboHandler = new ClockComboHandler();

            _clocks = new List<Clock>();
        }

        private void Start()
        {
            // Configs
            SpawnerAsset = ConfigurationManager.Instance.SpawnerAsset;

            // Initialize self and components
            Initialize();
        }

        private void OnEnable()
        {
            EventManager.Instance.Subscribe(EEventType.OnPlayTap, PlayTapPerformed);
        }

        private void OnDisable()
        {
            EventManager.Instance.Unsubscribe(EEventType.OnPlayTap, PlayTapPerformed);
        }

        private void Update()
        {
            Debug.Assert(_currentClock != null, "Current clock object is null!");

            Clock currentClock = _currentClock.GetComponent<Clock>();
            currentClock.DrawHand(currentClock.HandTransform.rotation.eulerAngles.z + currentClock.AngularSpeed * Time.deltaTime);

        }
        #endregion

        private void Initialize()
        {
            // Initialize components
            _clockSpawner.Initialize();
            _clockChecker.Initialize();
            _clockComboHandler.Initialize();

            // Initialize Manager
            _clocks.Clear();
            _currentClockIndex = 0;

            // Spawn a certain amount of Clock
            for (int i = 0; i < SpawnerAsset.ClockPoolSize; i++)
            {
                SpawnClock();
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
                    if (_currentClockIndex < SpawnerAsset.RenderingDistance)
                    {
                        _currentClockIndex++;
                    }
                    else
                    {
                        DespawnClock();
                        SpawnClock();
                    }
                    _clocksMover.MoveClocks(this, _clocks, _currentClock.transform.position);
                    break;

                case ECheckResult.Perfect:
                    if (_currentClockIndex < SpawnerAsset.RenderingDistance)
                    {
                        _currentClockIndex++;
                    }
                    else
                    {
                        DespawnClock();
                        SpawnClock();
                    }
                    _clocksMover.MoveClocks(this, _clocks, _currentClock.transform.position);
                    break;

                case ECheckResult.Unsuccess:
                    break;
            }

            // Take Combo Result
            ComboResult comboResult = _clockComboHandler.HandleCheckResult(checkResult);

            EventManager<ECheckResult, ComboResult>.Instance.Publish(EEventType.OnCheckerResult, checkResult, comboResult);
        }

        private void SpawnClock()
        {
            Clock generatedClock = _clockSpawner.SpawnClock(_lastClock);

            Debug.Assert(generatedClock != null, "Generated clock is null!");

            generatedClock.DrawClock();
            generatedClock.DrawHand(generatedClock.GetHandAngle());
            _clocks.Add(generatedClock);
        }

        private void DespawnClock()
        {
            Debug.Assert(_firstClock != null, "First clock object is null!");

            Clock firstClock = _firstClock;
            _clocks.Remove(firstClock);
            _clockSpawner.DespawnClock(firstClock);
        }
    }
}
