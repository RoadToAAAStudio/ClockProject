﻿using RoadToAAA.ProjectClock.Scriptables;
using UnityEngine;

namespace RoadToAAA.ProjectClock.Core
{
    /*
     *  This class has reference to all configuration assets of the game
     */
    public class ConfigurationManager : Singleton<ConfigurationManager>
    {
        [Header("Configurations")]
        public SpawnerAsset SpawnerAsset;
        public PaletteAsset[] PaletteAssets;
        public DifficultyAsset DifficultyAsset;
        public ComboAsset ComboAsset;

        [Header("Prefabs")]
        public GameObject ClockPrefab;



        #region UnityMessages
        protected override void Awake()
        {
            base.Awake();

            Debug.Assert(ValidateConfigurations().IsValid, "ConfigurationManager is not valid!");
        }

        #endregion

        public ScriptableObjectValidateResult ValidateConfigurations()
        {
            Debug.Assert(SpawnerAsset != null, "Spawner asset is null!");
            Debug.Assert(PaletteAssets != null && PaletteAssets.Length > 0, "Palette asset list is null or has 0 palettes!");
            Debug.Assert(DifficultyAsset != null, "Difficulty asset is null!");
            Debug.Assert(ComboAsset != null, "Combo asset is null!");
            Debug.Assert(ClockPrefab != null, "Clock prefab is null!");

            ScriptableObjectValidateResult result = new ScriptableObjectValidateResult();
            result.IsValid = true;

            result.Message += "Combo asset:\n";
            ScriptableObjectValidateResult comboResult = ComboAsset.CheckValidation();
            result.IsValid &= comboResult.IsValid;
            result.Message += "\t" + comboResult.Message + "\n";

            result.Message += "Difficulty asset:\n";
            ScriptableObjectValidateResult difficultyResult = DifficultyAsset.CheckValidation();
            result.IsValid &= difficultyResult.IsValid;
            result.Message += "\t" + difficultyResult.Message + "\n";

            for (int i = 0; i < PaletteAssets.Length; i++)
            {
                result.Message += string.Format("Palette asset {0}:\n", i);
                ScriptableObjectValidateResult paletteResult = PaletteAssets[i].CheckValidation();
                result.IsValid &= paletteResult.IsValid;
                result.Message += "\t" + paletteResult.Message + "\n";
            }

            result.Message += "Spawner asset:\n";
            ScriptableObjectValidateResult spawnerResult = SpawnerAsset.CheckValidation();
            result.IsValid &= spawnerResult.IsValid;
            result.Message += "\t" + spawnerResult.Message + "\n";

            return result;
        }
    }
}