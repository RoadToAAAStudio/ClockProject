using RoadToAAA.ProjectClock.Scriptables;
using UnityEngine;

namespace RoadToAAA.ProjectClock.Managers
{
    public class ClockChecker
    {
        private DifficultyAsset _difficultyAsset;

        public ClockChecker(DifficultyAsset difficultyAsset)
        {
            _difficultyAsset = difficultyAsset;
        }

        public ECheckResult Check(Clock clock)
        {
            Debug.Assert(clock != null, "Clock is null!");

            float dotProduct = Vector3.Dot(clock.HandTransform.right, clock.SuccessDirection);
            float successRelativeAngleRange = (_difficultyAsset.SuccessArcLength * 360.0f) / (2 * Mathf.PI * clock.Radius);
            float maxAllowedDotProduct = Mathf.Cos((successRelativeAngleRange / 2) * Mathf.Deg2Rad + Mathf.PI);
            float maxAllowedPerfectDotProduct = Mathf.Cos((successRelativeAngleRange * _difficultyAsset.PerfectSuccessRatio / 2) * Mathf.Deg2Rad + Mathf.PI);

            if (dotProduct <= maxAllowedPerfectDotProduct)
            {
                return ECheckResult.Perfect;
            }
            else if (dotProduct <= maxAllowedDotProduct)
            {
                return ECheckResult.Success;
            }

            return ECheckResult.Unsuccess;
        }
    }

    public enum ECheckResult
    {
        Unsuccess,
        Success,
        Perfect
    }
}
