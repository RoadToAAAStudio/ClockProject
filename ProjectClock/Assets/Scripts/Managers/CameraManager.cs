﻿using RoadToAAA.ProjectClock.Utilities;
using System;
using UnityEngine;

namespace RoadToAAA.ProjectClock.Managers
{
    public class CameraManager : MonoBehaviour
    {
        public float CameraLerpVelocity = 10.0f;
        private Camera _camera;
        private Vector3 _cameraStartPosition;
        private Vector3 _targetPosition;

        #region UnityMessages
        private void Awake()
        {
            _camera = Camera.main;
            _cameraStartPosition = _camera.transform.position;
            _targetPosition = _camera.transform.position;
        }

        private void OnEnable()
        {
            EventManager<EGameState, EGameState>.Instance.Subscribe(EEventType.OnGameStateChanged, GameStateChanged);
            EventManager<Clock, Clock>.Instance.Subscribe(EEventType.OnNewClockSelected, NewClockSelected); 
        }

        private void OnDisable()
        {
            EventManager<EGameState, EGameState>.Instance.Unsubscribe(EEventType.OnGameStateChanged, GameStateChanged);
            EventManager<Clock, Clock>.Instance.Unsubscribe(EEventType.OnNewClockSelected, NewClockSelected);
        }

        private void Update()
        {
            Vector2 lerpedPosition = Vector2.Lerp(_camera.transform.position, _targetPosition, CameraLerpVelocity * Time.deltaTime);
            _camera.transform.position = new Vector3(lerpedPosition.x, lerpedPosition.y, _camera.transform.position.z);
        }
        #endregion

        private void Initialize()
        {
            _camera.transform.position = _cameraStartPosition;
            _targetPosition = _camera.transform.position;
        }

        private void GameStateChanged(EGameState oldState, EGameState newState)
        {
            switch (newState)
            {
                case EGameState.MainMenu:
                    Initialize();
                    break;
            }
        }

        private void NewClockSelected(Clock newClock, Clock oldClock)
        {
            _targetPosition = newClock.transform.position;
        }
    }
}
