using RoadToAAA.ProjectClock.Scriptables;
using UnityEngine;

namespace RoadToAAA.ProjectClock.Managers
{
    public class ClockChecker
    {
        private DifficultyAsset _difficultyAsset;

        public void Initialize()
        {
            _difficultyAsset = ConfigurationManager.Instance.DifficultyAsset;
        }

        public ECheckResult Check(Clock clock)
        {
            Debug.Assert(clock != null, "Clock is null!");

            float dotProduct = Vector3.Dot(clock.HandTransform.right, clock.SuccessDirection);
            float successRelativeAngleRange = (_difficultyAsset.SuccessArcLength) / (clock.Radius);
            float maxAllowedDotProduct = Mathf.Cos((successRelativeAngleRange / 2));
            float maxAllowedPerfectDotProduct = Mathf.Cos((successRelativeAngleRange * _difficultyAsset.PerfectSuccessRatio / 2));

            if (dotProduct >= maxAllowedPerfectDotProduct)
            {
                return ECheckResult.Perfect;
            }
            else if (dotProduct >= maxAllowedDotProduct)
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
