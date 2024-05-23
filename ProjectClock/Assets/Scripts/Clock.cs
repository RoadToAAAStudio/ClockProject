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

    private void OnEnable()
    {
        //DrawClock();

        //_handTransform.rotation = Quaternion.Euler(0.0f, 0.0f, 0.0f);
        //DrawHand(_handTransform.eulerAngles.z);
    }

    private void OnDisable()
    {
        //HandGO.SetActive(false);
        //_renderer.color = new Color(0.25f, 0.25f, 0.25f);
    }

    void Update()
    {
        DrawHand(_handTransform.rotation.eulerAngles.z + HandAngularVelocity * Time.deltaTime);
    }

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

    public void ChangeHandColor(Color color)
    {
        //_renderer.color = color;
    }

    public Transform GetClockHandTransform()
    {
        return _handTransform;
    }
}
