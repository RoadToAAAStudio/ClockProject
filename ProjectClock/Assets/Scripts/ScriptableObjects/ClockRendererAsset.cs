using UnityEngine;

namespace RoadToAAA.ProjectClock.Scriptables
{
    [CreateAssetMenu(fileName = "ClockRendererAsset", menuName = "ConfigurationAssets/ClockRendererAsset")]
    public class ClockRendererAsset : ScriptableObject
    {
        public int ClockNumberOfSegments = 120;
        public float ClockWidth = 0.02f;

        public float StartHandWidth = 0.04f;
        public float EndHandWidth = 0.04f;
        public float HandLengthClockRadiusRatio = 0.95f;
        public float HandBackOffsetClockRadiusRatio = 0.05f;

#if UNITY_EDITOR
        private void OnValidate()
        {
            Debug.Assert(IsValid(), "ClockRenderer asset is not valid!");
        }
#endif

        public bool IsValid()
        {
            if (ClockNumberOfSegments <= 2) return false;
            if (ClockWidth <= 0.0f) return false;
            if (StartHandWidth <= 0.0f) return false;
            if (EndHandWidth <= 0.0f) return false;
            if (HandLengthClockRadiusRatio < 0.0f || HandLengthClockRadiusRatio > 1.0f) return false;
            if (HandBackOffsetClockRadiusRatio < 0.0f || HandBackOffsetClockRadiusRatio > 1.0f) return false;

            return true;
        }
    }
}
