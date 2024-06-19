using RoadToAAA.ProjectClock.Scriptables;
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

            Debug.Assert(IsValid(), "ConfigurationManager is not valid!");
        }

        private void OnValidate()
        {
            Debug.Assert(IsValid(), "ConfigurationManager is not valid!");
        }
        #endregion

        public bool IsValid()
        {
            if (SpawnerAsset == null && !SpawnerAsset.IsValid()) return false;

            if (PaletteAssets == null || PaletteAssets.Length <= 0) return false;
            for (int i = 0; i < PaletteAssets.Length; i++)
            {
                if (!PaletteAssets[i].IsValid()) return false;
            }

            if (DifficultyAsset == null && !DifficultyAsset.IsValid()) return false;
            if (ComboAsset == null && !ComboAsset.IsValid()) return false;
            if (ClockPrefab == null || !ClockPrefab.GetComponent<Clock>()) return false;

            return true;
        }
    }
}
