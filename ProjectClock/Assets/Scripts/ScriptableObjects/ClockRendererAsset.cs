using UnityEngine;

namespace RoadToAAA.ProjectClock.Scriptables
{
    [CreateAssetMenu(fileName = "ClockRendererAsset", menuName = "ConfigurationAssets/ClockRendererAsset")]
    public class ClockRendererAsset : ScriptableObject
    {
        public int ClockNumberOfSegments = 120;
        public float ClockWidth = 0.02f;

        public float HandWidth = 0.04f;
        public float HandLengthClockRadiusRatio = 0.95f;

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
            if (HandWidth <= 0.0f) return false;
            if (HandLengthClockRadiusRatio < 0.0f || HandLengthClockRadiusRatio > 1.0f) return false;

            return true;
        }
    }
}
