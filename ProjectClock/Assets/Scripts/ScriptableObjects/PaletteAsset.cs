﻿using System.Collections.Generic;
using UnityEngine;

namespace RoadToAAA.ProjectClock.Scriptables
{
    [CreateAssetMenu(fileName = "PaletteAsset", menuName = "ConfigurationAssets/PaletteAsset")]
    public class PaletteAsset : ScriptableObject
    {
        [Header("ClockColor")]
        public Color ClockColor = new Color(0.04f, 0.5f, 0.9f);
        public List<Color> HandColors = new List<Color>();
        public Color DeactivatedHandColor = new Color(0.25f, 0.25f, 0.25f);

        [Header("ClockRendering")]
        public int ClockNumberOfSegments = 120;
        public float ClockWidth = 0.02f;

        public float StartHandWidth = 0.04f;
        public float EndHandWidth = 0.04f;
        public float HandLengthClockRadiusRatio = 0.95f;
        public float HandBackOffsetClockRadiusRatio = 0.05f;

#if UNITY_EDITOR
        private void OnValidate()
        {
            Debug.Assert(IsValid(), "Palette asset is not valid!");
        }
#endif

        public Color GetRandomHandColor()
        {
            Debug.Assert(IsValid(), "Palette asset is not valid!");

            return HandColors[Random.Range(0, HandColors.Count)];
        }

        public Color GetRandomHandColor(Color currentColor)
        {
            Debug.Assert(IsValid(), "Palette asset is not valid!");
            Color color = HandColors[Random.Range(0, HandColors.Count)];

            // 15 tries to get a different color from the list
            for (int i = 0; i < 15; i++) 
            {
                if (color == currentColor)
                {
                    color = HandColors[Random.Range(0, HandColors.Count)];
                    continue;
                }
                else break;
            }
            return color;
        }

        public bool IsValid()
        {
            if (HandColors.Count == 0) return false;
            if (ClockNumberOfSegments <= 2) return false;
            if (ClockWidth <= 0.0f) return false;
            if (StartHandWidth <= 0.0f) return false;
            if (EndHandWidth <= 0.0f) return false;
            if (HandLengthClockRadiusRatio < 0.0f || HandLengthClockRadiusRatio > 1.0f) return false;
            if (HandBackOffsetClockRadiusRatio < 0.0f || HandBackOffsetClockRadiusRatio > 1.0f) return false;

            return true;
        }
    }
}
