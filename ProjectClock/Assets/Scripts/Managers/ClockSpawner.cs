using RoadToAAA.ProjectClock.Scriptables;
using RoadToAAA.ProjectClock.Utility;
using UnityEngine;

namespace RoadToAAA.ProjectClock.Managers
{
    /*
     * It implements pcg for clock spawning 
     */
    public class ClockSpawner
    {
        private SpawnerAsset _spawnerAsset;
        private ClockRendererAsset _clockRendererAsset;
        private PaletteAsset _paletteAsset;
        private DifficultyAsset _difficultyAsset;

        private StaticPool _clockPool;
        private int _currentNumberOfClocksSpawned = 0;

        public ClockSpawner(SpawnerAsset spawnerAsset, ClockRendererAsset clockRendererAsset, PaletteAsset paletteAsset, DifficultyAsset difficultyAsset, GameObject clockGameObject)
        {
            _spawnerAsset = spawnerAsset;
            _clockRendererAsset = clockRendererAsset;
            _paletteAsset = paletteAsset;
            _difficultyAsset = difficultyAsset;

            _clockPool = new StaticPool(clockGameObject, _spawnerAsset.ClockPoolSize);
        }

        public void Initialize()
        {
            _currentNumberOfClocksSpawned = 0;
        }

        public GameObject GenerateClock(GameObject previousClockGameObject)
        {
            Debug.Assert(_spawnerAsset != null, "Spawner Asset is null!");
            Debug.Assert(_spawnerAsset.IsValid(), "Spawner Asset is not valid!");

            GameObject clockGameObject;

            // Decide parameters based on pcg
            float handSpeed = _difficultyAsset.GetLerpedHandAbsoluteSpeed(_currentNumberOfClocksSpawned);
            handSpeed *= _currentNumberOfClocksSpawned % 2 == 0 ? 1 : -1;

            if (previousClockGameObject == null)
            {
                // Default
                float clockRadius = _spawnerAsset.MaxClockRadius;
                Color handColor = _paletteAsset.GetRandomHandColor();

                clockGameObject = _clockPool.Get(true, Vector3.zero);
                clockGameObject.GetComponent<Clock>().UpdateParameters(clockRadius, handSpeed, Vector3.zero, handColor);
            }
            else
            {
                // Spawn based on previous
                Clock previousClock = previousClockGameObject.GetComponent<Clock>();
                float clockRadius = Random.Range(_spawnerAsset.MinClockRadius, _spawnerAsset.MaxClockRadius);
                float randomAngle = Random.Range(_spawnerAsset.MinSpawnAngle, _spawnerAsset.MaxSpawnAngle);
                float randomRadAngle = randomAngle * Mathf.Deg2Rad;
                Vector3 previousClockPosition = previousClock.ClockTransform.position;
                Vector3 spawnDirection = new Vector3(Mathf.Cos(randomRadAngle), Mathf.Sin(randomRadAngle), 0.0f);
                Vector3 spawnPoint = previousClockPosition + spawnDirection * (previousClock.Radius + clockRadius + _clockRendererAsset.ClockWidth);
                Color handColor = _paletteAsset.GetRandomHandColor(previousClock.HandColor);

                clockGameObject = _clockPool.Get(true, spawnPoint);
                clockGameObject.GetComponent<Clock>().UpdateParameters(clockRadius, handSpeed, spawnDirection, handColor);
            }

            _currentNumberOfClocksSpawned++;

            return clockGameObject;
        }
    }
}