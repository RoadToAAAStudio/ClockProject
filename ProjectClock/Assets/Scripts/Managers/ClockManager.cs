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
        private List<GameObject> _clocks;
        private int _currentClockIndex;

        #region UnityMessages
        private void Awake()
        {
            _clockSpawner = new ClockSpawner(SpawnerAsset, ClockRendererAsset, PaletteAsset, DifficultyAsset, ClockPrefab);
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

            // Spawn a certain amount of Clock
            for (int i = 0; i < SpawnerAsset.ClockPoolSize; i++)
            {
                GameObject previousClockGameObject = i > 0 ? _clocks[i - 1] : null;
                GameObject generatedClockGameObject = _clockSpawner.GenerateClock(previousClockGameObject);
                Clock generatedClock = generatedClockGameObject.GetComponent<Clock>();

                generatedClock.DrawClock();
                generatedClock.DrawHand(generatedClock.GetHandAngle());

                _clocks.Add(generatedClockGameObject);
            }

            _clockSpawner.Initialize();
        }
    }
}
