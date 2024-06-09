using UnityEngine;

namespace RoadToAAA.ProjectClock.Scriptables
{
    [CreateAssetMenu(fileName = "SpawnerAsset", menuName = "ConfigurationAssets/SpawnerAsset")]
    public class SpawnerAsset : ScriptableObject
    {
        public GameObject ClockPrefab;
        public float MinClockRadius = 0.5f;
        public float MaxClockRadius = 2.0f;
        public float MinSpawnAngle = 45.0f;
        public float MaxSpawnAngle = 135.0f;
        public int ClockPoolSize = 10;


#if UNITY_EDITOR
        private void OnValidate()
        {
            Debug.Assert(IsValid(), "Spawner asset is not valid!");
        }
#endif

        public bool IsValid()
        {
            if (MinClockRadius <= 0 || MaxClockRadius <= 0) return false;
            if (MinClockRadius > MaxClockRadius) return false;
            if (MinSpawnAngle > MaxSpawnAngle) return false;
            if (ClockPoolSize < 1) return false;

            return true;
        }
    }
}
