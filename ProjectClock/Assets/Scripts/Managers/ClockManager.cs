using RoadToAAA.ProjectClock.Scriptables;
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
        [SerializeField] private SpawnerAsset SpawnerAsset;
        [SerializeField] private ClockRendererAsset ClockRendererAsset;
        [SerializeField] private PaletteAsset PaletteAsset;
        [SerializeField] private DifficultyAsset DifficultyAsset;
        [SerializeField] private GameObject ClockPrefab;

        private ClockSpawner _clockSpawner;
        private ClockChecker _clockChecker;
        private List<GameObject> _clocks;
        private int _currentClockIndex;

        #region UnityMessages
        private void Awake()
        {
            _clockSpawner = new ClockSpawner(SpawnerAsset, ClockRendererAsset, PaletteAsset, DifficultyAsset, ClockPrefab);
            _clockChecker = new ClockChecker(DifficultyAsset);

            _clocks = new List<GameObject>();

            Initialize();
        }

        private void Update()
        {
            Clock currentClock = _clocks[_currentClockIndex].GetComponent<Clock>();
            currentClock.DrawHand(currentClock.HandTransform.rotation.eulerAngles.z + currentClock.AngularSpeed * Time.deltaTime);
        }
        #endregion

        private void Initialize()
        {
            _clocks.Clear();
            _currentClockIndex = 0;

            // Spawn frist Clock
            GameObject generatedClockGameObject = _clockSpawner.GenerateClock(null);
            Clock generatedClock = generatedClockGameObject.GetComponent<Clock>();
            generatedClock.DrawClock();
            generatedClock.DrawHand(generatedClock.GetHandAngle());
            _clocks.Add(generatedClockGameObject);

            // Spawn a certain amount of Clock
            for (int i = 1; i < SpawnerAsset.ClockPoolSize - 1; i++)
            {
                GameObject previousClockGameObject = _clocks[i - 1];
                generatedClockGameObject = _clockSpawner.GenerateClock(previousClockGameObject);
                generatedClock.DrawClock();
                generatedClock.DrawHand(generatedClock.GetHandAngle());
                _clocks.Add(generatedClockGameObject);
            }

            _clockSpawner.Initialize();
        }
    }
}
