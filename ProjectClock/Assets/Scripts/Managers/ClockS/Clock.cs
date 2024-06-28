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
        private PaletteAsset _paletteAsset;

        #region UnityMessages
        private void Awake()
        {
            ClockGameObject = gameObject;

            // Configs
            _difficultyAsset = ConfigurationManager.Instance.DifficultyAsset;
            _paletteAsset = ConfigurationManager.Instance.PaletteAssets[0];

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

            float successRelativeAngleRange = (_difficultyAsset.SuccessArcLength) / (ClockParameters.Radius);
            float successAngle = Mathf.Atan2(ClockParameters.SuccessDirection.y, ClockParameters.SuccessDirection.x);
            float perfectSuccessRatio = _difficultyAsset.PerfectSuccessRatio;
            
            Gizmos.color = Color.green;
            Vector3 successArcStart = new Vector3(Mathf.Cos((successAngle - successRelativeAngleRange / 2)), Mathf.Sin((successAngle - successRelativeAngleRange / 2)), 0.0f);
            Vector3 successArcEnd = new Vector3(Mathf.Cos((successAngle + successRelativeAngleRange / 2)), Mathf.Sin((successAngle + successRelativeAngleRange / 2)), 0.0f);

            Gizmos.DrawLine(transform.position, transform.position + successArcStart * ClockParameters.Radius);
            Gizmos.DrawLine(transform.position, transform.position + successArcEnd * ClockParameters.Radius);

            Gizmos.color = Color.yellow;
            successArcStart = new Vector3(Mathf.Cos((successAngle - successRelativeAngleRange * perfectSuccessRatio / 2)), Mathf.Sin((successAngle - successRelativeAngleRange * perfectSuccessRatio / 2)), 0.0f);
            successArcEnd = new Vector3(Mathf.Cos((successAngle + successRelativeAngleRange * perfectSuccessRatio / 2)), Mathf.Sin((successAngle + successRelativeAngleRange * perfectSuccessRatio / 2)), 0.0f);
            
            Gizmos.DrawLine(transform.position, transform.position + successArcStart * ClockParameters.Radius);
            Gizmos.DrawLine(transform.position, transform.position + successArcEnd * ClockParameters.Radius);
        }
#endif
        #endregion

        public void DrawClock()
        {
            Debug.Assert(IsValid(), "Clock is not valid!");

            ClockRenderer.positionCount = _paletteAsset.ClockNumberOfSegments;
            ClockRenderer.startWidth = _paletteAsset.ClockWidth;
            ClockRenderer.endWidth = _paletteAsset.ClockWidth;
            ClockRenderer.loop = true;
            
            ClockRenderer.startColor = State == EClockState.ShutDown ? _paletteAsset.DeactivatedClockColor : ClockParameters.HandColor;
            ClockRenderer.endColor = State == EClockState.ShutDown ? _paletteAsset.DeactivatedClockColor : ClockParameters.HandColor;

            for (int i = 0; i < _paletteAsset.ClockNumberOfSegments; i++)
            {
                float circumferenceProgress = (float)i / _paletteAsset.ClockNumberOfSegments;
                float currentRadian = circumferenceProgress * 2 * Mathf.PI;

                float xScaled = Mathf.Cos(currentRadian);
                float yScaled = Mathf.Sin(currentRadian);
                float x = xScaled * ClockParameters.Radius;
                float y = yScaled * ClockParameters.Radius;

                Vector3 currentPosition = new Vector3(x, y, 0) + ClockTransform.position;
                ClockRenderer.SetPosition(i, currentPosition);
            }
        }

        public void DrawHand(float angle)
        {
            Debug.Assert(IsValid(), "Clock is not valid!");

            HandRenderer.positionCount = 2;
            HandRenderer.startWidth = _paletteAsset.StartHandWidth;
            HandRenderer.endWidth = _paletteAsset.EndHandWidth;
            HandRenderer.loop = false;
          
            HandRenderer.startColor = State == EClockState.Activated ? ClockParameters.HandColor : _paletteAsset.DeactivatedClockColor;
            HandRenderer.endColor = State == EClockState.Activated ? ClockParameters.HandColor : _paletteAsset.DeactivatedClockColor;
            
            float angleRad = angle * Mathf.Deg2Rad;

            Vector3 handBackOffset = new Vector3(Mathf.Cos(angleRad), Mathf.Sin(angleRad)) * ClockParameters.Radius * _paletteAsset.HandBackOffsetClockRadiusRatio;
            HandRenderer.SetPosition(0, HandTransform.position - handBackOffset);
            Vector3 handLength = new Vector3(Mathf.Cos(angleRad), Mathf.Sin(angleRad)) * ClockParameters.Radius * _paletteAsset.HandLengthClockRadiusRatio;
            HandRenderer.SetPosition(1, HandTransform.position + handLength);

            HandTransform.rotation = Quaternion.Euler(0.0f, 0.0f, angle);
        }

        // Update input and derived data according to new one
        public void UpdateData(ClockParameters newParameters)
        {
            // Input parameters
            ClockParameters = newParameters;

            // Derived parameters
            Circumference = 2 * Mathf.PI * ClockParameters.Radius;
            AngularSpeed = ClockParameters.HandSpeedOnCircumference / Circumference * 360.0f;
            ClockTransform.position = ClockParameters.Position;
            
            if (ClockParameters.SpawnDirection != Vector3.zero) 
            { 
                HandTransform.rotation = Quaternion.Euler(0.0f, 0.0f, Mathf.Atan2(-ClockParameters.SpawnDirection.y, -ClockParameters.SpawnDirection.x) * Mathf.Rad2Deg);
            }
            else
            {
                // Set default for the first clock
                HandTransform.rotation = Quaternion.Euler(0.0f, 0.0f, 270.0f); 
            }

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
            if (_paletteAsset == null) return false;

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
        public Vector3 Position;
        public Color ClockColor;
        public Color HandColor;
    }
}
