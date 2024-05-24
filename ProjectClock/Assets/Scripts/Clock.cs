using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Clock : MonoBehaviour
{
    public float ClockRadius = 1.0f;
    public int ClockSegments = 120;
    public float ClockWidth = 0.02f;
    public Color ClockColor = new Color(0.04f, 0.5f, 0.9f);

    public float HandAngularVelocity = 1.0f;
    public float HandWidth = 0.04f;
    public float HandLengthClockRadiusRatio = 0.95f;
    public Color HandColor = new Color(0.25f, 0.25f, 0.25f);
    public Color HandDeactivatedColor = new Color(0.25f, 0.25f, 0.25f);

    public float SuccessArcLength = 0.5f;
    public float PerfectSuccessRatio = 0.5f;
    public float PerfectSuccessAngle = 0.0f;

    private GameObject _handGO;
    private Transform _handTransform;
    private LineRenderer _circleRenderer;
    private LineRenderer _handRenderer;

    private void Awake()
    {
        _handGO = transform.GetChild(0).gameObject;
        _handTransform = _handGO.transform;
        _circleRenderer = GetComponent<LineRenderer>();
        _handRenderer = transform.GetChild(0).GetComponent<LineRenderer>();
    }

    void Update()
    {
        DrawHand(_handTransform.rotation.eulerAngles.z + HandAngularVelocity * Time.deltaTime);
    }

#if UNITY_EDITOR
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.green;
        Vector3 successArcStart = new Vector3(Mathf.Cos((PerfectSuccessAngle - SuccessRelativeAngleRange() / 2) * Mathf.Deg2Rad), Mathf.Sin((PerfectSuccessAngle - SuccessRelativeAngleRange() / 2) * Mathf.Deg2Rad), 0.0f);
        Vector3 successArcEnd = new Vector3(Mathf.Cos((PerfectSuccessAngle + SuccessRelativeAngleRange() / 2) * Mathf.Deg2Rad), Mathf.Sin((PerfectSuccessAngle + SuccessRelativeAngleRange() / 2) * Mathf.Deg2Rad), 0.0f);
        Gizmos.DrawLine(transform.position, transform.position + successArcStart * ClockRadius);
        Gizmos.DrawLine(transform.position, transform.position + successArcEnd * ClockRadius);

        Gizmos.color = Color.yellow;
        successArcStart = new Vector3(Mathf.Cos((PerfectSuccessAngle - SuccessRelativeAngleRange() * PerfectSuccessRatio / 2) * Mathf.Deg2Rad), Mathf.Sin((PerfectSuccessAngle - SuccessRelativeAngleRange() * PerfectSuccessRatio / 2) * Mathf.Deg2Rad), 0.0f);
        successArcEnd = new Vector3(Mathf.Cos((PerfectSuccessAngle + SuccessRelativeAngleRange() * PerfectSuccessRatio / 2) * Mathf.Deg2Rad), Mathf.Sin((PerfectSuccessAngle + SuccessRelativeAngleRange() * PerfectSuccessRatio / 2) * Mathf.Deg2Rad), 0.0f);
        Gizmos.DrawLine(transform.position, transform.position + successArcStart * ClockRadius);
        Gizmos.DrawLine(transform.position, transform.position + successArcEnd * ClockRadius);
    }
#endif

    public void DrawClock()
    {
        _circleRenderer.positionCount = ClockSegments;
        _circleRenderer.startWidth = ClockWidth;
        _circleRenderer.endWidth = ClockWidth;
        _circleRenderer.loop = true;
        _circleRenderer.startColor = ClockColor;
        _circleRenderer.endColor = ClockColor;

        for (int i = 0; i < ClockSegments; i++)
        {
            float circumferenceProgress = (float)i / ClockSegments;
            float currentRadian = circumferenceProgress * 2 * Mathf.PI;

            float xScaled = Mathf.Cos(currentRadian);
            float yScaled = Mathf.Sin(currentRadian);
            float x = xScaled * ClockRadius;
            float y = yScaled * ClockRadius;

            Vector3 currentPosition = new Vector3(x, y, 0) + transform.position;
            _circleRenderer.SetPosition(i, currentPosition);
        }
    }

    public void DrawHand(float angle)
    {
        _handRenderer.positionCount = 2;
        _handRenderer.startWidth = HandWidth;
        _handRenderer.endWidth = HandWidth;
        _handRenderer.loop = false;
        _handRenderer.startColor = HandColor;
        _handRenderer.endColor = HandColor;

        _handRenderer.SetPosition(0, transform.position);
        _handRenderer.SetPosition(1, transform.position + new Vector3(Mathf.Cos(angle * Mathf.Deg2Rad) * ClockRadius * HandLengthClockRadiusRatio, Mathf.Sin(angle * Mathf.Deg2Rad) * ClockRadius * HandLengthClockRadiusRatio));

        _handTransform.rotation = Quaternion.Euler(0.0f, 0.0f, angle);
    }

    public float SuccessRelativeAngleRange()
    {
        return (SuccessArcLength * 360.0f) / (2 * Mathf.PI * ClockRadius);
    }

    public float MaxDotProductAllowed() => Mathf.Cos((SuccessRelativeAngleRange() / 2) * Mathf.Deg2Rad + Mathf.PI);

    public float PerfectMaxDotProduct() => Mathf.Cos((SuccessRelativeAngleRange() * PerfectSuccessRatio / 2) * Mathf.Deg2Rad + Mathf.PI);

    public float CurrentAngle() => _handTransform.rotation.eulerAngles.z;

    public void ChangeHandColor(Color color)
    {
        HandColor = color;
    }

    public Transform GetClockHandTransform()
    {
        return _handTransform;
    }
}
