﻿using System;
using UnityEngine;

namespace RoadToAAA.ProjectClock.Scriptables
{
    [CreateAssetMenu(fileName = "CurveContainer", menuName = "ScriptableObjects/CurveContainer")]
    public class Difficulty : ScriptableObject
    {
        public CurvePoint[] Points;

        public float GetLerpedHandAbsoluteSpeed(int currentNumberOfClearedClock)
        {
            Debug.Assert(currentNumberOfClearedClock >= 0 , "Curve Point mus have positive Number of Cleared Clocks!");
            Debug.Assert(IsValid(), "Difficulty asset is not valid!");

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
                CurvePoint point = Points[i];
                if (point.NumberOfClearedClocks == currentNumberOfClearedClock)
                {
                    return point.HandAbsoluteSpeed;
                }
                else if (point.NumberOfClearedClocks > currentNumberOfClearedClock)
                {
                    CurvePoint pre = Points[i - 1];
                    float percentage = (currentNumberOfClearedClock - pre.NumberOfClearedClocks) / (float)(point.NumberOfClearedClocks - pre.NumberOfClearedClocks);

                    return Mathf.Lerp(pre.HandAbsoluteSpeed, point.HandAbsoluteSpeed, percentage);
                }
            }

            // Should be unreachable
            return Points[Points.Length - 1].HandAbsoluteSpeed;
        }

#if UNITY_EDITOR
        private void OnValidate()
        {
            Debug.Assert(IsValid(), "Difficulty asset is not valid!");      
        }
#endif
        private bool IsValid()
        {
            if (Points.Length == 0) return false;
            if (Points.Length == 1) return Points[0].NumberOfClearedClocks == 0;

            for (int i = 1; i < Points.Length; i++) 
            {
                CurvePoint prePoint = Points[i - 1];
                CurvePoint currentPoint = Points[i];
                
                if (prePoint.NumberOfClearedClocks >= currentPoint.NumberOfClearedClocks)
                {
                    return false;
                }
            }

            return true;
        }
    }

    [Serializable]
    public class CurvePoint
    {
        public int NumberOfClearedClocks = 0;

        [Range(0.0f, 10.0f)]
        public float HandAbsoluteSpeed = 0.0f;
    }

}