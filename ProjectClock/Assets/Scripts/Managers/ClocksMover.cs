using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RoadToAAA.ProjectClock.Managers
{
    // Simulates a Camera movement moving a clock instead
    public class ClocksMover
    {
        private Camera _camera;
        private Vector3 _cameraPositionOnXY;
        private IEnumerator MoveClocksCoroutine;

        public ClocksMover()
        {
            _camera = Camera.main;
            _cameraPositionOnXY = new Vector3(_camera.transform.position.x, _camera.transform.position.y, 0.0f);
        }

        public void MoveClocks(MonoBehaviour owner, List<Clock> clocks, Vector3 targetPosition)
        {
            Debug.Assert(owner != null, "Owner is null!");
            Debug.Assert(clocks != null && clocks.Count > 0, "Clocks is invalid!");

            if (MoveClocksCoroutine != null)
            {
                owner.StopCoroutine(MoveClocksCoroutine);
            }
            MoveClocksCoroutine = MoveClocks(clocks, targetPosition);
            owner.StartCoroutine(MoveClocksCoroutine);
        }

        // Move all clocks as the camera moved to targetPosition
        private IEnumerator MoveClocks(List<Clock> clocks, Vector3 targetPosition)
        {
            while (Vector3.Distance(targetPosition, _cameraPositionOnXY) > 0.005f)
            {
                Vector3 lerpedPosition = Vector3.Lerp(_cameraPositionOnXY, targetPosition, 10.0f * Time.deltaTime);
                Vector3 deltaClockMovement = _cameraPositionOnXY - lerpedPosition;

                targetPosition += deltaClockMovement;    

                for (int i = 0; i < clocks.Count; i++)
                {
                    Clock clock = clocks[i];
                    GameObject clockGameObject = clocks[i].gameObject;
                    Transform clockTransform = clockGameObject.transform;
                    clockTransform.position += (Vector3)deltaClockMovement;
                    clock.DrawClock();
                    clock.DrawHand(clock.GetHandAngle());
                }

                yield return null;
            }
        }
    }
}
