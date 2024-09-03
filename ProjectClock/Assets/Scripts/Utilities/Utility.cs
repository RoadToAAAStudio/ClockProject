using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RoadToAAA.ProjectClock.Utilities
{
    public static class Utility
    {
        public static float EaseOutSine(float x)
        {
            return Mathf.Sin((x * Mathf.PI) / 2);
        }
    }
}
