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
        //EventManagerTwoParams<GameObject, GameObject>.Instance.StartListening("onNewClock", NewClockSelected);    
    }

    private void OnDisable()
    {
        //EventManagerTwoParams<GameObject, GameObject>.Instance.StopListening("onNewClock", NewClockSelected);
    }

    private void Update()
    {
        Vector2 lerpedPosition = Vector2.Lerp(_camera.transform.position, _targetPosition, CameraLerpVelocity * Time.deltaTime);
        _camera.transform.position = new Vector3(lerpedPosition.x, lerpedPosition.y, _camera.transform.position.z);
        //_camera.transform.position = new Vector3(_camera.transform.position.x, lerpedPosition.y, _camera.transform.position.z);
    }

    private void NewClockSelected(GameObject newClockGO, GameObject oldClockGO)
    {
        _targetPosition = newClockGO.transform.position;
    }
}
