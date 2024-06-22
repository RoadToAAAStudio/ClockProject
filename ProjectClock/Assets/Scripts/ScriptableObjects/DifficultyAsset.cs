using System;
using UnityEngine;

namespace RoadToAAA.ProjectClock.Scriptables
{
    [CreateAssetMenu(fileName = "DifficultyAsset", menuName = "ConfigurationAssets/DifficultyAsset")]
    public class DifficultyAsset : ValidableScriptableObject
    {
        public float SuccessArcLength = 0.5f;
        public float PerfectSuccessRatio = 0.5f;
        public DifficultyCurvePoint[] Points;

        public override ScriptableObjectValidateResult CheckValidation()
        {
            ScriptableObjectValidateResult result = new ScriptableObjectValidateResult();
            result.IsValid = true;

            if (SuccessArcLength <= 0.0f)
            {
                result.IsValid = false;
                result.Message += "Success arc length must be positive!\n";
            }
            if (PerfectSuccessRatio < 0.0f || PerfectSuccessRatio > 1.0f)
            {
                result.IsValid = false;
                result.Message += "Perfect success ratio must be a percentage!\n";
            }
            if (Points.Length == 0)
            {
                result.IsValid = false;
                result.Message += "Define at least one point of the curve!\n";
            }
            if (Points.Length == 1 && Points[0].NumberOfClearedClocks != 0)
            {
                result.IsValid = false;
                result.Message += "First point of the curve must refer to cleared clocks equal to 0!\n";
            }

            for (int i = 1; i < Points.Length; i++)
            {
                DifficultyCurvePoint prePoint = Points[i - 1];
                DifficultyCurvePoint currentPoint = Points[i];

                if (prePoint.NumberOfClearedClocks >= currentPoint.NumberOfClearedClocks)
                {
                    result.IsValid = false;
                    result.Message += "Number of cleared clocks of one curve point must be higher than the previous one's!\n";
                }
            }

            if (result.IsValid)
            {
                result.Message += "Successful!";
            }

            return result;
        }

        public float GetLerpedHandAbsoluteSpeed(int currentNumberOfClearedClock)
        {
            Debug.Assert(currentNumberOfClearedClock >= 0 , "Curve Point must have positive Number of Cleared Clocks!");
            Debug.Assert(CheckValidation().IsValid, "Difficulty asset is not valid!");

            if (currentNumberOfClearedClock == 0)
            {
                return Points[0].HandAbsoluteSpeed;
            }

            // Saturate to the last speed
            if (currentNumberOfClearedClock > Points[Points.Length - 1].NumberOfClearedClocks)
            {
                return Points[Points.Length - 1].HandAbsoluteSpeed;
            }

            for (int i = 1; i < Points.Length; i++)
            {
                DifficultyCurvePoint point = Points[i];
                if (point.NumberOfClearedClocks == currentNumberOfClearedClock)
                {
                    return point.HandAbsoluteSpeed;
                }
                else if (point.NumberOfClearedClocks > currentNumberOfClearedClock)
                {
                    DifficultyCurvePoint pre = Points[i - 1];
                    float percentage = (currentNumberOfClearedClock - pre.NumberOfClearedClocks) / (float)(point.NumberOfClearedClocks - pre.NumberOfClearedClocks);

                    return Mathf.Lerp(pre.HandAbsoluteSpeed, point.HandAbsoluteSpeed, percentage);
                }
            }

            // Should be unreachable
            return Points[Points.Length - 1].HandAbsoluteSpeed;
        }
    }

    [Serializable]
    public class DifficultyCurvePoint
    {
        public int NumberOfClearedClocks = 0;

        [Range(0.0f, 10.0f)]
        public float HandAbsoluteSpeed = 0.0f;
    }
}
