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
        private DifficultyAsset _difficultyAsset;
        private GameObject _clockPrefab;

        private StaticPool _clocksPool;
        private int _currentNumberOfSpawnedClocks = 0;
        private int _distanceFromLastSpecialClockSpawned = 0;

        public ClockSpawner()
        {
            _spawnerAsset = ConfigurationManager.Instance.SpawnerAsset;
            _difficultyAsset = ConfigurationManager.Instance.DifficultyAsset;
            _clockPrefab = ConfigurationManager.Instance.ClockPrefab;

            _clocksPool = new StaticPool(_clockPrefab, null, _spawnerAsset.ClockPoolSize);
        }

        public void Initialize()
        {
            _currentNumberOfSpawnedClocks = 0;
        }

        // Return a new ClockGameObject if it was possible to generate one
        public Clock SpawnClock(PaletteAsset paletteAsset, Clock previousClock)
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
                parameters = GenerateFirstClock(paletteAsset); 
            }
            else
            {
                // Spawn based on previous
                parameters = GenerateClock(paletteAsset, previousClock.ClockParameters);
            }

            newClock.UpdateData(parameters);            

            // Rendering
            newClock.DeactivateHand();
            newClock.DrawClock(paletteAsset);
            newClock.DrawHand(paletteAsset, newClock.GetHandAngle());

            // Handle spawner state
            _currentNumberOfSpawnedClocks++;
            if (parameters.IsSpecial)
            {
                _distanceFromLastSpecialClockSpawned = 0;
            }
            else
            {
                _distanceFromLastSpecialClockSpawned++;
            }

#if UNITY_EDITOR && LOGGER
            Debug.Log(string.Format("<color=red>ClockSpawned = {0} </color>", _currentNumberOfSpawnedClocks));
#endif
            return newClock;
        }

        public void DespawnClock(Clock clock)
        {
            _clocksPool.Release(clock.gameObject);
        }

        private ClockParameters GenerateFirstClock(PaletteAsset paletteAsset)
        {
            ClockParameters newClockParameters = new ClockParameters();

            float randomAngle = Random.Range(_spawnerAsset.MinSpawnAngle, _spawnerAsset.MaxSpawnAngle);
            float randomAngleRad = randomAngle * Mathf.Deg2Rad;

            newClockParameters.IsSpecial = false;
            newClockParameters.Radius = _spawnerAsset.MaxClockRadius;
            newClockParameters.HandSpeedOnCircumference = _difficultyAsset.GetLerpedHandAbsoluteSpeed(_currentNumberOfSpawnedClocks) * (_currentNumberOfSpawnedClocks % 2 == 0 ? 1 : -1);
            newClockParameters.SuccessDirection = new Vector3(Mathf.Cos(randomAngleRad), Mathf.Sin(randomAngleRad), 0.0f);
            newClockParameters.PerfectSuccessRadAngle = randomAngleRad;
            newClockParameters.PerfectSuccessRadAngleStart = randomAngleRad - ((_difficultyAsset.SuccessArcLength / newClockParameters.Radius) * _difficultyAsset.PerfectSuccessRatio) / 2;
            newClockParameters.PerfectSuccessRadAngleEnd = randomAngleRad + ((_difficultyAsset.SuccessArcLength / newClockParameters.Radius) * _difficultyAsset.PerfectSuccessRatio) / 2;
            newClockParameters.SuccessRadAngleStart = randomAngleRad - (_difficultyAsset.SuccessArcLength / newClockParameters.Radius) / 2;
            newClockParameters.SuccessRadAngleEnd = randomAngleRad + (_difficultyAsset.SuccessArcLength / newClockParameters.Radius) / 2;
            newClockParameters.SpawnDirection = Vector3.zero;
            newClockParameters.StartAngle = 270.0f;
            newClockParameters.ClockColor = paletteAsset.ClockColor;
            newClockParameters.HandColor = paletteAsset.GetRandomHandColor();
            newClockParameters.Position = Vector3.zero;
            newClockParameters.Currency = 0;

            return newClockParameters;
        }

        private ClockParameters GenerateClock(PaletteAsset paletteAsset, ClockParameters previousClockParameters)
        {
            ClockParameters newClockParameters = new ClockParameters();

            float randomAngle = Random.Range(_spawnerAsset.MinSpawnAngle, _spawnerAsset.MaxSpawnAngle);
            float randomAngleRad = randomAngle * Mathf.Deg2Rad;
            Vector3 previousClockPosition = previousClockParameters.Position;
            Vector3 spawnDirection = previousClockParameters.SuccessDirection;

            newClockParameters.IsSpecial = ShouldGenerateASpecialClock();
            newClockParameters.Radius = Random.Range(_spawnerAsset.MinClockRadius, _spawnerAsset.MaxClockRadius);
            newClockParameters.HandSpeedOnCircumference = _difficultyAsset.GetLerpedHandAbsoluteSpeed(_currentNumberOfSpawnedClocks) * (_currentNumberOfSpawnedClocks % 2 == 0 ? 1 : -1);
            newClockParameters.SuccessDirection = new Vector3(Mathf.Cos(randomAngleRad), Mathf.Sin(randomAngleRad), 0.0f);
            newClockParameters.PerfectSuccessRadAngle = randomAngleRad;
            newClockParameters.PerfectSuccessRadAngleStart = randomAngleRad - ((_difficultyAsset.SuccessArcLength / newClockParameters.Radius) * _difficultyAsset.PerfectSuccessRatio) / 2;
            newClockParameters.PerfectSuccessRadAngleEnd = randomAngleRad + ((_difficultyAsset.SuccessArcLength / newClockParameters.Radius) * _difficultyAsset.PerfectSuccessRatio) / 2;
            newClockParameters.SuccessRadAngleStart = (randomAngleRad - (_difficultyAsset.SuccessArcLength) / (newClockParameters.Radius) / 2);
            newClockParameters.SuccessRadAngleEnd = (randomAngleRad + (_difficultyAsset.SuccessArcLength) / (newClockParameters.Radius) / 2);
            newClockParameters.SpawnDirection = spawnDirection;
            newClockParameters.StartAngle = Mathf.Atan2(-spawnDirection.y, -spawnDirection.x) * Mathf.Rad2Deg;
            newClockParameters.ClockColor = newClockParameters.IsSpecial ? paletteAsset.SpecialClockColor : paletteAsset.ClockColor;
            newClockParameters.HandColor = paletteAsset.GetRandomHandColor(previousClockParameters.HandColor);
            newClockParameters.Position = previousClockPosition + spawnDirection * (previousClockParameters.Radius + newClockParameters.Radius + paletteAsset.ClockWidth);
            newClockParameters.Currency = newClockParameters.IsSpecial ? _difficultyAsset.GetLerpedCurrencyObtained(_currentNumberOfSpawnedClocks) : 0;

            return newClockParameters;
        }

        private bool ShouldGenerateASpecialClock()
        {
            if (_distanceFromLastSpecialClockSpawned < _spawnerAsset.GetLerpedSpecialClockCurveValue(_currentNumberOfSpawnedClocks).MinSpaceBetweenSpecialClocks)
            {
                return false;
            }
            else if (_distanceFromLastSpecialClockSpawned > _spawnerAsset.GetLerpedSpecialClockCurveValue(_currentNumberOfSpawnedClocks).MaxSpaceBetweenSpecialClocks)
            {
                return true;
            }

           int spawnChance = (int)(_spawnerAsset.GetLerpedSpecialClockCurveValue(_currentNumberOfSpawnedClocks).SpecialClockSpawnChance * 100);

            return Random.Range(0, 100) < spawnChance;
        }
    }
}