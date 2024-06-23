using System;
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
        public SpecialClockCurvePoint[] Points;


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
            if (Points.Length == 0)
            {
                result.IsValid = false;
                result.Message += "Define at least one point of the curve!\n";
            }
            if (Points.Length == 1 && Points[0].NumberOfSpawnedClocks != 0)
            {
                result.IsValid = false;
                result.Message += "First point of the curve must refer to cleared clocks equal to 0!\n";
            }

            for (int i = 1; i < Points.Length; i++)
            {
                SpecialClockCurvePoint prePoint = Points[i - 1];
                SpecialClockCurvePoint currentPoint = Points[i];

                if (prePoint.NumberOfSpawnedClocks >= currentPoint.NumberOfSpawnedClocks)
                {
                    result.IsValid = false;
                    result.Message += "Number of spawned clocks of one curve point must be higher than the previous one's!\n";
                }
            }

            if (result.IsValid)
            {
                result.Message += "Successful!";
            }

            return result;
        }

        public SpecialClockCurvePointValue GetLerpedSpecialClockCurveValue(int currentNumberOfSpawnedClocks)
        {
            Debug.Assert(currentNumberOfSpawnedClocks >= 0, "Curve Point must have positive Number of Spawned Clocks!");
            Debug.Assert(CheckValidation().IsValid, "Spawner asset is not valid!");

            if (currentNumberOfSpawnedClocks == 0)
            {
                return Points[0].CurveValue;
            }

            // Saturate to the last speed
            if (currentNumberOfSpawnedClocks > Points[Points.Length - 1].NumberOfSpawnedClocks)
            {
                return Points[Points.Length - 1].CurveValue;
            }

            for (int i = 1; i < Points.Length; i++)
            {
                SpecialClockCurvePoint point = Points[i];
                if (point.NumberOfSpawnedClocks == currentNumberOfSpawnedClocks)
                {
                    return point.CurveValue;
                }
                else if (point.NumberOfSpawnedClocks > currentNumberOfSpawnedClocks)
                {
                    SpecialClockCurvePoint pre = Points[i - 1];
                    float percentage = (currentNumberOfSpawnedClocks - pre.NumberOfSpawnedClocks) / (float)(point.NumberOfSpawnedClocks - pre.NumberOfSpawnedClocks);

                    SpecialClockCurvePointValue result = new SpecialClockCurvePointValue();
                    result.SpecialClockSpawnChance = Mathf.Lerp(pre.CurveValue.SpecialClockSpawnChance, point.CurveValue.SpecialClockSpawnChance, percentage);
                    result.MinSpaceBetweenSpecialClocks = pre.CurveValue.MinSpaceBetweenSpecialClocks;
                    result.MaxSpaceBetweenSpecialClocks = pre.CurveValue.MaxSpaceBetweenSpecialClocks;
                    return result;
                }
            }

            // Should be unreachable
            return Points[Points.Length - 1].CurveValue;
        }
    }

    [Serializable]
    public class SpecialClockCurvePoint
    {
        public int NumberOfSpawnedClocks = 0;

        public SpecialClockCurvePointValue CurveValue;
    }

    [Serializable]
    public class SpecialClockCurvePointValue
    {
        [Range(0.0f, 1.0f)]
        public float SpecialClockSpawnChance = 0.0f;

        [Range(0, 100)]
        public int MinSpaceBetweenSpecialClocks = 0;

        [Range(0, 100)]
        public int MaxSpaceBetweenSpecialClocks = 0;
    }
}
