using RoadToAAA.ProjectClock.Scriptables;
using RoadToAAA.ProjectClock.Utilities;
using RoadToAAA.ProjectClock.Core;
using UnityEngine;

namespace RoadToAAA.ProjectClock.Managers
{
    /*
     * It implements pcg for clock spawning and despawning
     */
    public class ClockSpawner
    {
        private SpawnerAsset _spawnerAsset;
        private PaletteAsset _paletteAsset;
        private DifficultyAsset _difficultyAsset;
        private GameObject _clockPrefab;

        private StaticPool _clocksPool;
        private int _currentNumberOfClocksSpawned = 0;
        private int _distanceFromLastSpecialClockSpawned = 0;

        public ClockSpawner()
        {
            _spawnerAsset = ConfigurationManager.Instance.SpawnerAsset;
            _paletteAsset = ConfigurationManager.Instance.PaletteAssets[0];
            _difficultyAsset = ConfigurationManager.Instance.DifficultyAsset;
            _clockPrefab = ConfigurationManager.Instance.ClockPrefab;

            _clocksPool = new StaticPool(_clockPrefab, _spawnerAsset.ClockPoolSize);
        }

        public void Initialize()
        {
            _currentNumberOfClocksSpawned = 0;
        }

        // Return a new ClockGameObject if it was possible to generate one
        public Clock SpawnClock(Clock previousClock)
        {
            Debug.Assert(_spawnerAsset != null, "Spawner Asset is null!");
            Debug.Assert(_spawnerAsset.CheckValidation().IsValid, "Spawner Asset is not valid!");

            if (!_clocksPool.HasItems()) return null;

            Clock newClock = _clocksPool.Get(true).GetComponent<Clock>();
            ClockParameters parameters;

            // Decide parameters based on pcg
            if (previousClock == null)
            {
                // Default
                parameters = GenerateFirstClock(); 
            }
            else
            {
                // Spawn based on previous
                parameters = GenerateClock(previousClock.ClockParameters);
            }

            newClock.UpdateData(parameters);            

            // Rendering
            newClock.DeactivateHand();
            newClock.DrawClock();
            newClock.DrawHand(newClock.GetHandAngle());

            // Handle spawner state
            _currentNumberOfClocksSpawned++;
            if (parameters.IsSpecial)
            {
                _distanceFromLastSpecialClockSpawned = 0;
            }
            else
            {
                _distanceFromLastSpecialClockSpawned++;
            }

            return newClock;
        }

        public void DespawnClock(Clock clock)
        {
            _clocksPool.Release(clock.gameObject);
        }

        private ClockParameters GenerateFirstClock()
        {
            ClockParameters newClockParameters = new ClockParameters();

            float randomAngle = Random.Range(_spawnerAsset.MinSpawnAngle, _spawnerAsset.MaxSpawnAngle);
            float randomRadAngle = randomAngle * Mathf.Deg2Rad;

            newClockParameters.IsSpecial = false;
            newClockParameters.Radius = _spawnerAsset.MaxClockRadius;
            newClockParameters.HandSpeedOnCircumference = _difficultyAsset.GetLerpedHandAbsoluteSpeed(_currentNumberOfClocksSpawned) * (_currentNumberOfClocksSpawned % 2 == 0 ? 1 : -1);
            newClockParameters.SuccessDirection = new Vector3(Mathf.Cos(randomRadAngle), Mathf.Sin(randomRadAngle), 0.0f);
            newClockParameters.SpawnDirection = Vector3.zero;
            newClockParameters.ClockColor = _paletteAsset.ClockColor;
            newClockParameters.HandColor = _paletteAsset.GetRandomHandColor();
            newClockParameters.Position = Vector3.zero;

            return newClockParameters;
        }

        private ClockParameters GenerateClock(ClockParameters previousClockParameters)
        {
            ClockParameters newClockParameters = new ClockParameters();

            float randomAngle = Random.Range(_spawnerAsset.MinSpawnAngle, _spawnerAsset.MaxSpawnAngle);
            float randomRadAngle = randomAngle * Mathf.Deg2Rad;
            Vector3 previousClockPosition = previousClockParameters.Position;
            Vector3 spawnDirection = previousClockParameters.SuccessDirection;

            newClockParameters.IsSpecial = ShouldGenerateASpecialClock();
            newClockParameters.Radius = Random.Range(_spawnerAsset.MinClockRadius, _spawnerAsset.MaxClockRadius);
            newClockParameters.HandSpeedOnCircumference = _difficultyAsset.GetLerpedHandAbsoluteSpeed(_currentNumberOfClocksSpawned) * (_currentNumberOfClocksSpawned % 2 == 0 ? 1 : -1);
            newClockParameters.SuccessDirection = new Vector3(Mathf.Cos(randomRadAngle), Mathf.Sin(randomRadAngle), 0.0f);
            newClockParameters.SpawnDirection = spawnDirection;
            newClockParameters.ClockColor = newClockParameters.IsSpecial ? _paletteAsset.SpecialClockColor : _paletteAsset.ClockColor;
            newClockParameters.HandColor = _paletteAsset.GetRandomHandColor(previousClockParameters.HandColor);
            newClockParameters.Position = previousClockPosition + spawnDirection * (previousClockParameters.Radius + newClockParameters.Radius + _paletteAsset.ClockWidth);

            return newClockParameters;
        }

        private bool ShouldGenerateASpecialClock()
        {
            if (_distanceFromLastSpecialClockSpawned < _spawnerAsset.GetLerpedSpecialClockCurveValue(_currentNumberOfClocksSpawned).MinSpaceBetweenSpecialClocks)
            {
                return false;
            }
            else if (_distanceFromLastSpecialClockSpawned > _spawnerAsset.GetLerpedSpecialClockCurveValue(_currentNumberOfClocksSpawned).MaxSpaceBetweenSpecialClocks)
            {
                return true;
            }

           int spawnChance = (int)(_spawnerAsset.GetLerpedSpecialClockCurveValue(_currentNumberOfClocksSpawned).SpecialClockSpawnChance * 100);

            return Random.Range(0, 100) < spawnChance;
        }
    }
}