using System;
using UnityEngine;

namespace RoadToAAA.ProjectClock.Scriptables
{
    [CreateAssetMenu(fileName = "DifficultyAsset", menuName = "ConfigurationAssets/DifficultyAsset")]
    public class DifficultyAsset : ValidableScriptableObject
    {
        public float SuccessArcLength = 0.5f;
        public float PerfectSuccessRatio = 0.5f;
        public DifficultyCurvePoint[] DifficultyCurvePoints;
        public CurrencyCurvePoint[] CurrencyCurvePoints;

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
            if (DifficultyCurvePoints.Length == 0)
            {
                result.IsValid = false;
                result.Message += "Define at least one point of the difficulty curve!\n";
            }
            if (DifficultyCurvePoints.Length == 1 && DifficultyCurvePoints[0].NumberOfSpawnedClocks != 0)
            {
                result.IsValid = false;
                result.Message += "First point of the difficulty curve must refer to cleared clocks equal to 0!\n";
            }

            for (int i = 1; i < DifficultyCurvePoints.Length; i++)
            {
                DifficultyCurvePoint prePoint = DifficultyCurvePoints[i - 1];
                DifficultyCurvePoint currentPoint = DifficultyCurvePoints[i];

                if (prePoint.NumberOfSpawnedClocks >= currentPoint.NumberOfSpawnedClocks)
                {
                    result.IsValid = false;
                    result.Message += "Number of cleared clocks of one difficulty curve point must be higher than the previous one's!\n";
                }
            }

            if (CurrencyCurvePoints.Length == 0)
            {
                result.IsValid = false;
                result.Message += "Define at least one point of the currency curve!\n";
            }
            if (CurrencyCurvePoints.Length == 1 && CurrencyCurvePoints[0].NumberOfSpawnedClocks != 0)
            {
                result.IsValid = false;
                result.Message += "First point of the currency curve must refer to cleared clocks equal to 0!\n";
            }

            for (int i = 1; i < CurrencyCurvePoints.Length; i++)
            {
                CurrencyCurvePoint prePoint = CurrencyCurvePoints[i - 1];
                CurrencyCurvePoint currentPoint = CurrencyCurvePoints[i];

                if (prePoint.NumberOfSpawnedClocks >= currentPoint.NumberOfSpawnedClocks)
                {
                    result.IsValid = false;
                    result.Message += "Number of cleared clocks of one currency curve point must be higher than the previous one's!\n";
                }
                if (prePoint.CurrencyObtained <= 0 || currentPoint.CurrencyObtained <= 0)
                {
                    result.IsValid = false;
                    result.Message += "Currency obtained must be positive!\n";
                }
            }

            if (result.IsValid)
            {
                result.Message += "Successful!";
            }

            return result;
        }

        public float GetLerpedHandAbsoluteSpeed(int currentNumberOfSpawnedClock)
        {
            Debug.Assert(currentNumberOfSpawnedClock >= 0 , "Curve Point must have positive Number of Cleared Clocks!");
            Debug.Assert(CheckValidation().IsValid, "Difficulty asset is not valid!");

            if (currentNumberOfSpawnedClock == 0)
            {
                return DifficultyCurvePoints[0].HandAbsoluteSpeed;
            }

            // Saturate to the last speed
            if (currentNumberOfSpawnedClock > DifficultyCurvePoints[DifficultyCurvePoints.Length - 1].NumberOfSpawnedClocks)
            {
                return DifficultyCurvePoints[DifficultyCurvePoints.Length - 1].HandAbsoluteSpeed;
            }

            for (int i = 1; i < DifficultyCurvePoints.Length; i++)
            {
                DifficultyCurvePoint point = DifficultyCurvePoints[i];
                if (point.NumberOfSpawnedClocks == currentNumberOfSpawnedClock)
                {
                    return point.HandAbsoluteSpeed;
                }
                else if (point.NumberOfSpawnedClocks > currentNumberOfSpawnedClock)
                {
                    DifficultyCurvePoint pre = DifficultyCurvePoints[i - 1];
                    float percentage = (currentNumberOfSpawnedClock - pre.NumberOfSpawnedClocks) / (float)(point.NumberOfSpawnedClocks - pre.NumberOfSpawnedClocks);

                    return Mathf.Lerp(pre.HandAbsoluteSpeed, point.HandAbsoluteSpeed, percentage);
                }
            }

            // Should be unreachable
            return DifficultyCurvePoints[DifficultyCurvePoints.Length - 1].HandAbsoluteSpeed;
        }

        public int GetLerpedCurrencyObtained(int currentNumberOfSpawnedClock)
        {
            Debug.Assert(currentNumberOfSpawnedClock >= 0, "Curve Point must have positive Number of Cleared Clocks!");
            Debug.Assert(CheckValidation().IsValid, "Difficulty asset is not valid!");

            if (currentNumberOfSpawnedClock == 0)
            {
                return CurrencyCurvePoints[0].CurrencyObtained;
            }

            // Saturate to the last speed
            if (currentNumberOfSpawnedClock > CurrencyCurvePoints[CurrencyCurvePoints.Length - 1].NumberOfSpawnedClocks)
            {
                return CurrencyCurvePoints[CurrencyCurvePoints.Length - 1].CurrencyObtained;
            }

            for (int i = 1; i < CurrencyCurvePoints.Length; i++)
            {
                CurrencyCurvePoint point = CurrencyCurvePoints[i];
                if (point.NumberOfSpawnedClocks == currentNumberOfSpawnedClock)
                {
                    return point.CurrencyObtained;
                }
                else if (point.NumberOfSpawnedClocks > currentNumberOfSpawnedClock)
                {
                    CurrencyCurvePoint pre = CurrencyCurvePoints[i - 1];
                    float percentage = (currentNumberOfSpawnedClock - pre.NumberOfSpawnedClocks) / (float)(point.NumberOfSpawnedClocks - pre.NumberOfSpawnedClocks);

                    return (int)Mathf.Lerp(pre.CurrencyObtained, point.CurrencyObtained, percentage);
                }
            }

            // Should be unreachable
            return CurrencyCurvePoints[CurrencyCurvePoints.Length - 1].CurrencyObtained;
        }
    }

    [Serializable]
    public class DifficultyCurvePoint
    {
        public int NumberOfSpawnedClocks = 0;

        [Range(0.0f, 10.0f)]
        public float HandAbsoluteSpeed = 0.0f;
    }

    [Serializable]
    public class CurrencyCurvePoint
    {
        public int NumberOfSpawnedClocks = 0;

        public int CurrencyObtained = 0;
    }
}
