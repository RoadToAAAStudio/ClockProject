using RoadToAAA.ProjectClock.Scriptables;
using RoadToAAA.ProjectClock.Utilities;
using System.Collections.Generic;
using UnityEngine;

namespace RoadToAAA.ProjectClock.Managers
{
    /*
     * It implements pcg for clock spawning and despawning
     */
    public class ClockSpawner
    {
        private SpawnerAsset _spawnerAsset;
        private ClockRendererAsset _clockRendererAsset;
        private PaletteAsset _paletteAsset;
        private DifficultyAsset _difficultyAsset;
        private GameObject _clockPrefab;

        private StaticPool _clockPool;
        private int _currentNumberOfClocksSpawned = 0;

        public void Initialize()
        {
            _currentNumberOfClocksSpawned = 0;

            _spawnerAsset = ConfigurationManager.Instance.SpawnerAsset;
            _clockRendererAsset = ConfigurationManager.Instance.ClockRendererAsset;
            _paletteAsset = ConfigurationManager.Instance.PaletteAssets[0];
            _difficultyAsset = ConfigurationManager.Instance.DifficultyAsset;
            _clockPrefab = ConfigurationManager.Instance.ClockPrefab;

            _clockPool = new StaticPool(_clockPrefab, _spawnerAsset.ClockPoolSize);
        }

        // Return a new ClockGameObject if it was possible to generate one
        public Clock SpawnClock(Clock previousClock)
        {
            Debug.Assert(_spawnerAsset != null, "Spawner Asset is null!");
            Debug.Assert(_spawnerAsset.IsValid(), "Spawner Asset is not valid!");

            if (!_clockPool.HasItems()) return null;

            Clock newClock = null;

            // Decide parameters based on pcg
            float handSpeed = _difficultyAsset.GetLerpedHandAbsoluteSpeed(_currentNumberOfClocksSpawned);
            handSpeed *= _currentNumberOfClocksSpawned % 2 == 0 ? 1 : -1;

            if (previousClock == null)
            {
                // Default
                float clockRadius = _spawnerAsset.MaxClockRadius;
                Color handColor = _paletteAsset.GetRandomHandColor();

                newClock = _clockPool.Get(true, Vector3.zero).GetComponent<Clock>();
                newClock.UpdateParameters(clockRadius, handSpeed, Vector3.zero, handColor, null);
            }
            else
            {
                // Spawn based on previous
                float clockRadius = Random.Range(_spawnerAsset.MinClockRadius, _spawnerAsset.MaxClockRadius);
                float randomAngle = Random.Range(_spawnerAsset.MinSpawnAngle, _spawnerAsset.MaxSpawnAngle);
                float randomRadAngle = randomAngle * Mathf.Deg2Rad;
                Vector3 previousClockPosition = previousClock.ClockTransform.position;
                Vector3 spawnDirection = new Vector3(Mathf.Cos(randomRadAngle), Mathf.Sin(randomRadAngle), 0.0f);
                Vector3 spawnPoint = previousClockPosition + spawnDirection * (previousClock.Radius + clockRadius + _clockRendererAsset.ClockWidth);
                Color handColor = _paletteAsset.GetRandomHandColor(previousClock.HandColor);

                newClock = _clockPool.Get(true, spawnPoint).GetComponent<Clock>();
                newClock.UpdateParameters(clockRadius, handSpeed, spawnDirection, handColor, previousClock);
            }

            _currentNumberOfClocksSpawned++;

            return newClock;
        }

        public void DespawnClock(Clock clock)
        {
            _clockPool.Release(clock.gameObject);
        }
    }
}