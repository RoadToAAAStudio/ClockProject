using System.Collections.Generic;
using UnityEngine;

namespace RoadToAAA.ProjectClock.Scriptables
{
    [CreateAssetMenu(fileName = "PaletteAsset", menuName = "ConfigurationAssets/PaletteAsset")]
    public class PaletteAsset : ValidableScriptableObject
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

        public Color GetRandomHandColor()
        {
            Debug.Assert(CheckValidation().IsValid, "Palette asset is not valid!");

            return HandColors[Random.Range(0, HandColors.Count)];
        }

        public Color GetRandomHandColor(Color currentColor)
        {
            Debug.Assert(CheckValidation().IsValid, "Palette asset is not valid!");
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

        public override ScriptableObjectValidateResult CheckValidation()
        {
            ScriptableObjectValidateResult result = new ScriptableObjectValidateResult();
            result.IsValid = true;

            if (ClockNumberOfSegments <= 2)
            {
                result.IsValid = false;
                result.Message += "Clock must have at least 2 segments!\n";
            }
            if (ClockWidth <= 0.0f)
            {
                result.IsValid = false;
                result.Message += "Clock width can't be negative!\n";
            }
            if (StartHandWidth <= 0.0f)
            {
                result.IsValid = false;
                result.Message += "Hand width can't be negative!\n";
            }
            if (EndHandWidth <= 0.0f)
            {
                result.IsValid = false;
                result.Message += "Hand width can't be negative!\n";
            }
            if (HandLengthClockRadiusRatio < 0.0f || HandLengthClockRadiusRatio > 1.0f)
            {
                result.IsValid = false;
                result.Message += "HandLengthClockRadiusRatio must be a percentage!\n";
            }
            if (HandBackOffsetClockRadiusRatio < 0.0f || HandBackOffsetClockRadiusRatio > 1.0f)
            {
                result.IsValid = false;
                result.Message += "HandBackOffsetClockRadiusRatio!\n";
            }

            if (result.IsValid)
            {
                result.Message += "Successful!";
            }

            return result;
        }
    }
}
