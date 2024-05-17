using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    public float CameraLerpVelocity = 10.0f;
    private Camera _camera;
    private Vector3 _targetPosition;

    private void Awake()
    {
        _camera = Camera.main;
        _targetPosition = _camera.transform.position;
    }

    private void OnEnable()
    {
        EventManagerOneParam<GameObject>.Instance.StartListening("onNewClock", NewClockSelected);    
    }

    private void OnDisable()
    {
        EventManagerOneParam<GameObject>.Instance.StopListening("onNewClock", NewClockSelected);
    }

    private void Update()
    {
        _camera.transform.position = Vector2.Lerp(_camera.transform.position, _targetPosition, CameraLerpVelocity * Time.deltaTime);
    }

    private void NewClockSelected(GameObject newClockGO)
    {
        _targetPosition = newClockGO.transform.position;
    }
}
