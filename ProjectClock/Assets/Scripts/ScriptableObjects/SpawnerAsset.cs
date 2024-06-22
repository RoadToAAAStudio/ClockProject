using UnityEngine;

namespace RoadToAAA.ProjectClock.Scriptables
{
    [CreateAssetMenu(fileName = "SpawnerAsset", menuName = "ConfigurationAssets/SpawnerAsset")]
    public class SpawnerAsset : ValidableScriptableObject
    {
        public GameObject ClockPrefab;
        public float MinClockRadius = 0.5f;
        public float MaxClockRadius = 2.0f;
        public float MinSpawnAngle = 45.0f;
        public float MaxSpawnAngle = 135.0f;
        public int ClockPoolSize = 11;
        public int RenderingDistance = 5;

        public override ScriptableObjectValidateResult CheckValidation()
        {
            ScriptableObjectValidateResult result = new ScriptableObjectValidateResult();
            result.IsValid = true;

            if (MinClockRadius <= 0 || MaxClockRadius <= 0)
            {
                result.IsValid = false;
                result.Message += "Radius must be positive!\n";
            }
            if (MinClockRadius > MaxClockRadius)
            {
                result.IsValid = false;
                result.Message += "Min radius can't be higher then max radius!\n";
            }
            if (MinSpawnAngle > MaxSpawnAngle)
            {
                result.IsValid = false;
                result.Message += "Min spawn angle can't be higher then max spawn angle!\n";
            }
            if (ClockPoolSize <= 0)
            {
                result.IsValid = false;
                result.Message += "Clock pool size must be positive!\n";
            }
            if (RenderingDistance <= 0 || RenderingDistance > ClockPoolSize / 2)
            {
                result.IsValid = false;
                result.Message += "Rendering distance must be positive and less than half the clock pool size!\n";
            }

            if (result.IsValid)
            {
                result.Message += "Successful!";
            }

            return result;

        }
    }
}
