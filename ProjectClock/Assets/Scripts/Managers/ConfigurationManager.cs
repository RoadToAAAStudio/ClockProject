using RoadToAAA.ProjectClock.Scriptables;
using RoadToAAA.ProjectClock.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace RoadToAAA.ProjectClock.Managers
{
    /*
     *  This class has reference to all configuration assets of the game
     */
    public class ConfigurationManager : Singleton<ConfigurationManager>
    {
        [Header("Configurations")]
        public SpawnerAsset SpawnerAsset;
        public ClockRendererAsset ClockRendererAsset;
        public PaletteAsset[] PaletteAssets;
        public DifficultyAsset DifficultyAsset;
        public ComboAsset ComboAsset;

        [Header("Prefabs")]
        public GameObject ClockPrefab;

        #region UnityMessages
        private void OnValidate()
        {
            Debug.Assert(IsValid(), "ConfigurationManager is not valid!");
        }
        #endregion

        public bool IsValid()
        {
            if (SpawnerAsset == null) return false;
            if (ClockRendererAsset == null) return false;
            if (PaletteAssets == null || PaletteAssets.Length <= 0) return false;
            if (DifficultyAsset == null) return false;
            if (ComboAsset == null) return false;
            if (ClockPrefab == null || !ClockPrefab.GetComponent<Clock>()) return false;

            return true;
        }
    }
}
