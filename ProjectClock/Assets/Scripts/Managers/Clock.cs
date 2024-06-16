using RoadToAAA.ProjectClock.Scriptables;
using System;
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
        public float Radius { get; private set; }
        public Vector3 SuccessDirection { get; private set; }
        public Vector3 SpawnDirection { get; private set; }
        public Color HandColor { get; private set; }

        // Derived Parameters
        public float Circumference { get; private set; }
        public float AngularSpeed { get; private set; }

        // Runtime data
        public EClockState ClockState { get; private set; }
        public float GetHandAngle() => HandTransform.rotation.eulerAngles.z;

        // Configs
        private DifficultyAsset _difficultyAsset;
        private ClockRendererAsset _clockRendererAsset;
        private PaletteAsset _paletteAsset;

        #region UnityMessages
        private void Awake()
        {
            ClockGameObject = gameObject;

            // Configs
            _difficultyAsset = ConfigurationManager.Instance.DifficultyAsset;
            _clockRendererAsset = ConfigurationManager.Instance.ClockRendererAsset;
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

            float successRelativeAngleRange = (_difficultyAsset.SuccessArcLength) / (Radius);
            float successAngle = Mathf.Atan2(SuccessDirection.y, SuccessDirection.x);
            float perfectSuccessRatio = _difficultyAsset.PerfectSuccessRatio;
            
            Gizmos.color = Color.green;
            Vector3 successArcStart = new Vector3(Mathf.Cos((successAngle - successRelativeAngleRange / 2)), Mathf.Sin((successAngle - successRelativeAngleRange / 2)), 0.0f);
            Vector3 successArcEnd = new Vector3(Mathf.Cos((successAngle + successRelativeAngleRange / 2)), Mathf.Sin((successAngle + successRelativeAngleRange / 2)), 0.0f);

            Gizmos.DrawLine(transform.position, transform.position + successArcStart * Radius);
            Gizmos.DrawLine(transform.position, transform.position + successArcEnd * Radius);

            Gizmos.color = Color.yellow;
            successArcStart = new Vector3(Mathf.Cos((successAngle - successRelativeAngleRange * perfectSuccessRatio / 2)), Mathf.Sin((successAngle - successRelativeAngleRange * perfectSuccessRatio / 2)), 0.0f);
            successArcEnd = new Vector3(Mathf.Cos((successAngle + successRelativeAngleRange * perfectSuccessRatio / 2)), Mathf.Sin((successAngle + successRelativeAngleRange * perfectSuccessRatio / 2)), 0.0f);
            
            Gizmos.DrawLine(transform.position, transform.position + successArcStart * Radius);
            Gizmos.DrawLine(transform.position, transform.position + successArcEnd * Radius);
        }
#endif
        #endregion

        public void DrawClock()
        {
            Debug.Assert(IsValid(), "Clock is not valid!");

            ClockRenderer.positionCount = _clockRendererAsset.ClockNumberOfSegments;
            ClockRenderer.startWidth = _clockRendererAsset.ClockWidth;
            ClockRenderer.endWidth = _clockRendererAsset.ClockWidth;
            ClockRenderer.loop = true;
            ClockRenderer.startColor = _paletteAsset.ClockColor;
            ClockRenderer.endColor = _paletteAsset.ClockColor;

            for (int i = 0; i < _clockRendererAsset.ClockNumberOfSegments; i++)
            {
                float circumferenceProgress = (float)i / _clockRendererAsset.ClockNumberOfSegments;
                float currentRadian = circumferenceProgress * 2 * Mathf.PI;

                float xScaled = Mathf.Cos(currentRadian);
                float yScaled = Mathf.Sin(currentRadian);
                float x = xScaled * Radius;
                float y = yScaled * Radius;

                Vector3 currentPosition = new Vector3(x, y, 0) + ClockTransform.position;
                ClockRenderer.SetPosition(i, currentPosition);
            }
        }

        public void DrawHand(float angle)
        {
            Debug.Assert(IsValid(), "Clock is not valid!");

            HandRenderer.positionCount = 2;
            HandRenderer.startWidth = _clockRendererAsset.StartHandWidth;
            HandRenderer.endWidth = _clockRendererAsset.EndHandWidth;
            HandRenderer.loop = false;
          
            HandRenderer.startColor = ClockState == EClockState.Activated ? HandColor : _paletteAsset.DeactivatedHandColor;
            HandRenderer.endColor = ClockState == EClockState.Activated ? HandColor : _paletteAsset.DeactivatedHandColor;
            
            float angleRad = angle * Mathf.Deg2Rad;

            Vector3 handBackOffset = new Vector3(Mathf.Cos(angleRad), Mathf.Sin(angleRad)) * Radius * _clockRendererAsset.HandBackOffsetClockRadiusRatio;
            HandRenderer.SetPosition(0, HandTransform.position - handBackOffset);
            Vector3 handLength = new Vector3(Mathf.Cos(angleRad), Mathf.Sin(angleRad)) * Radius * _clockRendererAsset.HandLengthClockRadiusRatio;
            HandRenderer.SetPosition(1, HandTransform.position + handLength);

            HandTransform.rotation = Quaternion.Euler(0.0f, 0.0f, angle);
        }

        // Update derived data according to new one
        public void UpdateParameters(float radius, float handSpeedOnCircumference, Vector3 spawnDirection, Color handColor, Clock previousClock)
        {
            Radius = radius;
            Circumference = 2 * Mathf.PI * Radius;
            AngularSpeed = handSpeedOnCircumference / Circumference * 360.0f;
            SpawnDirection = spawnDirection;
            
            if (spawnDirection != Vector3.zero) 
            { 
                HandTransform.rotation = Quaternion.Euler(0.0f, 0.0f, Mathf.Atan2(-spawnDirection.y, -spawnDirection.x) * Mathf.Rad2Deg);
            }
            else
            {
                // Set default for the first clock
                HandTransform.rotation = Quaternion.Euler(0.0f, 0.0f, 270.0f); 
            }

            HandColor = handColor;

            if (previousClock != null) 
            { 
                previousClock.SuccessDirection = SpawnDirection;
            }

            ClockState = EClockState.Activated;

            Debug.Assert(IsValid(), "Clock is not valid!");
        }

        public void ActivateHand()
        {
            ClockState = EClockState.Activated;
        }

        public void DeactivateHand()
        {
            ClockState = EClockState.Deactivated;
        }

        public bool IsValid()
        {
            if (ClockGameObject == null) return false;
            if (ClockTransform == null) return false;
            if (HandTransform == null) return false;
            if (Radius <= 0) return false;
            if (ClockRenderer == null) return false;
            if (HandRenderer == null) return false;
            if (_clockRendererAsset == null) return false;
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
        Deactivated
    }
}
