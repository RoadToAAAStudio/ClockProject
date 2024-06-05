using System;
using UnityEngine;

namespace RoadToAAA.ProjectClock.Scriptables
{
    [CreateAssetMenu(fileName = "CurveContainer", menuName = "ScriptableObjects/CurveContainer")]
    public class Difficulty : ScriptableObject
    {
        public CurvePoint[] Points;

        public static float GetLerpedHandAbsoluteSpeed(CurvePoint pre, CurvePoint post, int currentNumberOfClearedClock)
        {
            Debug.Assert( pre.NumberOfClearedClock > 0 && post.NumberOfClearedClock > 0 , "Curve Point mus have positive Number of Cleared Clocks!");
            Debug.Assert( pre.NumberOfClearedClock < post.NumberOfClearedClock, "Pre Curve Point must be precedent the post Curve Point!");

            float percentage = (currentNumberOfClearedClock - pre.NumberOfClearedClock) / (float)(post.NumberOfClearedClock - pre.NumberOfClearedClock);

            return Mathf.Lerp(pre.HandAbsoluteSpeed, post.HandAbsoluteSpeed, percentage);
        }
    }

    [Serializable]
    public class CurvePoint
    {
        [Range(0, int.MaxValue)]
        public int NumberOfClearedClock = 0;

        [Range(0.0f, 10.0f)]
        public float HandAbsoluteSpeed = 0.0f;
    }

}
