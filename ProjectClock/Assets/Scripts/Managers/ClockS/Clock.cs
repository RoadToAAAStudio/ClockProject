using RoadToAAA.ProjectClock.Scriptables;
using RoadToAAA.ProjectClock.Core;
using UnityEngine;

namespace RoadToAAA.ProjectClock.Managers
{
    /*
     * It hold all clock parameters 
     */
    public class Clock : MonoBehaviour
    {
        public GameObject ClockGameObject { get; private set; }
        public Transform ClockTransform { get; private set; }
        public GameObject HandGameObject { get; private set; }
        public Transform HandTransform { get; private set; }

        // Components
        public LineRenderer ClockRenderer { get; private set; }
        public LineRenderer HandRenderer { get; private set; }

        // Input Parameters
        public ClockParameters ClockParameters { get; private set; }

        // Derived Parameters
        public float Circumference { get; private set; }
        public float AngularSpeed { get; private set; }

        // Runtime data
        public EClockState State { get; private set; }
        public float GetHandAngle() => HandTransform.rotation.eulerAngles.z;

        // Configs
        private DifficultyAsset _difficultyAsset;

        #region UnityMessages
        private void Awake()
        {
            ClockGameObject = gameObject;

            // Configs
            _difficultyAsset = ConfigurationManager.Instance.DifficultyAsset;

            // Deduce other data
            ClockTransform = ClockGameObject.transform;
            HandGameObject = ClockTransform.GetChild(0).gameObject;
            HandTransform = HandGameObject.transform;
            ClockRenderer = ClockGameObject.GetComponent<LineRenderer>();
            HandRenderer = HandGameObject.GetComponent<LineRenderer>();
        }

#if UNITY_EDITOR
        private void OnDrawGizmos()
        {
            if (_difficultyAsset == null) return;

            ClockParameters clockParameters = ClockParameters;

            float successRelativeAngleRange = (_difficultyAsset.SuccessArcLength) / (clockParameters.Radius);
            float successAngle = Mathf.Atan2(clockParameters.SuccessDirection.y, clockParameters.SuccessDirection.x);
            float perfectSuccessRatio = _difficultyAsset.PerfectSuccessRatio;
            
            Gizmos.color = Color.green;
            Vector3 successArcStart = new Vector3(Mathf.Cos((successAngle - successRelativeAngleRange / 2)), Mathf.Sin((successAngle - successRelativeAngleRange / 2)), 0.0f);
            Vector3 successArcEnd = new Vector3(Mathf.Cos((successAngle + successRelativeAngleRange / 2)), Mathf.Sin((successAngle + successRelativeAngleRange / 2)), 0.0f);

            Gizmos.DrawLine(clockParameters.Position, clockParameters.Position + successArcStart * clockParameters.Radius);
            Gizmos.DrawLine(clockParameters.Position, clockParameters.Position + successArcEnd * clockParameters.Radius);

            Gizmos.color = Color.yellow;
            successArcStart = new Vector3(Mathf.Cos((successAngle - successRelativeAngleRange * perfectSuccessRatio / 2)), Mathf.Sin((successAngle - successRelativeAngleRange * perfectSuccessRatio / 2)), 0.0f);
            successArcEnd = new Vector3(Mathf.Cos((successAngle + successRelativeAngleRange * perfectSuccessRatio / 2)), Mathf.Sin((successAngle + successRelativeAngleRange * perfectSuccessRatio / 2)), 0.0f);
            
            Gizmos.DrawLine(clockParameters.Position, clockParameters.Position + successArcStart * clockParameters.Radius);
            Gizmos.DrawLine(clockParameters.Position, clockParameters.Position + successArcEnd * clockParameters.Radius);
        }
#endif
        #endregion

        public void DrawClock(PaletteAsset paletteAsset)
        {
            Debug.Assert(IsValid(), "Clock is not valid!");

            ClockParameters clockParameters = ClockParameters;
            LineRenderer clockRenderer = ClockRenderer;
            Transform clockTransform = ClockTransform;
            Vector3 clockPosition = clockTransform.position;

            clockRenderer.positionCount = paletteAsset.ClockNumberOfSegments;
            clockRenderer.startWidth = paletteAsset.ClockWidth;
            clockRenderer.endWidth = paletteAsset.ClockWidth;
            clockRenderer.loop = true;
            
            clockRenderer.startColor = State == EClockState.ShutDown ? paletteAsset.DeactivatedClockColor : clockParameters.ClockColor;
            clockRenderer.endColor = State == EClockState.ShutDown ? paletteAsset.DeactivatedClockColor : clockParameters.ClockColor;

            for (int i = 0; i < paletteAsset.ClockNumberOfSegments; i++)
            {
                float circumferenceProgress = (float)i / paletteAsset.ClockNumberOfSegments;
                float currentRadian = circumferenceProgress * 2 * Mathf.PI;

                float xScaled = Mathf.Cos(currentRadian);
                float yScaled = Mathf.Sin(currentRadian);
                float x = xScaled * clockParameters.Radius;
                float y = yScaled * clockParameters.Radius;

                Vector3 currentPosition = new Vector3(x, y, 0) + clockPosition;
                clockRenderer.SetPosition(i, currentPosition);
            }
        }

        public void DrawHand(PaletteAsset paletteAsset, float angle)
        {
            Debug.Assert(IsValid(), "Clock is not valid!");

            ClockParameters clockParameters = ClockParameters;
            LineRenderer handeRenderer = HandRenderer;
            Transform handTransform = HandTransform;
            Vector3 handPosition = handTransform.position;

            handeRenderer.positionCount = 2;
            handeRenderer.startWidth = paletteAsset.StartHandWidth;
            handeRenderer.endWidth = paletteAsset.EndHandWidth;
            handeRenderer.loop = false;
          
            handeRenderer.startColor = State == EClockState.Activated ? clockParameters.HandColor : paletteAsset.DeactivatedClockColor;
            handeRenderer.endColor = State == EClockState.Activated ? clockParameters.HandColor : paletteAsset.DeactivatedClockColor;
            
            float angleRad = angle * Mathf.Deg2Rad;

            Vector3 handBackOffset = new Vector3(Mathf.Cos(angleRad), Mathf.Sin(angleRad)) * clockParameters.Radius * paletteAsset.HandBackOffsetClockRadiusRatio;
            handeRenderer.SetPosition(0, handPosition - handBackOffset);
            Vector3 handLength = new Vector3(Mathf.Cos(angleRad), Mathf.Sin(angleRad)) * clockParameters.Radius * paletteAsset.HandLengthClockRadiusRatio;
            handeRenderer.SetPosition(1, handPosition + handLength);

            handTransform.rotation = Quaternion.Euler(0.0f, 0.0f, angle);
        }

        // Update input and derived data according to new one
        public void UpdateData(ClockParameters newParameters)
        {
            // Input parameters
            ClockParameters = newParameters;

            // Derived parameters
            Circumference = 2 * Mathf.PI * newParameters.Radius;
            AngularSpeed = newParameters.HandSpeedOnCircumference / Circumference * 360.0f;
            ClockTransform.position = newParameters.Position;
            HandTransform.rotation = Quaternion.Euler(0.0f, 0.0f, newParameters.StartAngle);

            State = EClockState.Activated;

            Debug.Assert(IsValid(), "Clock is not valid!");
        }

        public void ActivateHand()
        {
            State = EClockState.Activated;
        }

        public void DeactivateHand()
        {
            State = EClockState.Deactivated;
        }

        public void DeactivateClock()
        {
            State = EClockState.ShutDown;
        }

        public bool IsValid()
        {
            if (ClockGameObject == null) return false;
            if (ClockTransform == null) return false;
            if (HandTransform == null) return false;
            if (ClockParameters.Radius <= 0) return false;
            if (ClockRenderer == null) return false;
            if (HandRenderer == null) return false;

            return true;
        }

        public override string ToString()
        {
            return gameObject.name;
        }
    }

    public enum EClockState
    {
        Activated,
        Deactivated,
        ShutDown
    }

    public struct ClockParameters
    {
        public bool IsSpecial;
        public float Radius;
        public float HandSpeedOnCircumference;
        public Vector3 SuccessDirection;
        public Vector3 SpawnDirection;
        public float StartAngle;
        public Vector3 Position;
        public Color ClockColor;
        public Color HandColor;
    }
}
