using UnityEngine;

// Generate a screenshot and save it to disk with the name SomeLevel.png.

namespace RoadToAAA.ProjectClock.Utilities
{
    public class ScreenCaptureUtility : MonoBehaviour
    {
        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.S))
            {
                ScreenCapture.CaptureScreenshot("ScreenShot.png", 1);
            }
        }
    }
}
