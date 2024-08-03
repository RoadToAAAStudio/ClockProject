using UnityEngine;
using UnityEditor;
using RoadToAAA.ProjectClock.Core;
using UnityEngine.PlayerLoop;

namespace RoadToAAA.ProjectClock.Editors
{
    [CustomEditor(typeof(PlayerDataManager))]
    public class PlayerDataManagerInspector : Editor
    {
        private string _savedDataMessage = string.Empty;
        private string _runtimeDataMessage = string.Empty;

        private void OnEnable()
        {
            PlayerDataManager playerDataManager = (PlayerDataManager)target;

            _savedDataMessage = string.Empty;
            _savedDataMessage += string.Format("\tBestScore: {0}", DataManager.Instance.GetInt("bestScore", 0));
            _savedDataMessage += string.Format("\n\tCurrency: {0}", DataManager.Instance.GetInt("currency", 0));
            _savedDataMessage += string.Format("\n\tIsApplicationStartedForTheFirstTime: {0}", DataManager.Instance.GetInt("isFirstTimeApplicationIsStarted", 1) != 0 ? "true" : "false");

            _runtimeDataMessage = string.Empty;
            _runtimeDataMessage += string.Format("\tScore: {0}", playerDataManager.Score);
            _runtimeDataMessage += string.Format("\n\tBestScore: {0}", playerDataManager.BestScore);
            _runtimeDataMessage += string.Format("\n\tCurrency: {0}", playerDataManager.Currency);
            _runtimeDataMessage += string.Format("\n\tCurrentPaletteIndex: {0}", playerDataManager.CurrentPaletteIndex);
            _runtimeDataMessage += string.Format("\n\tSelectedPaletteIndex: {0}", playerDataManager.SelectedPaletteIndex);
            _runtimeDataMessage += string.Format("\n\tPreviewPaletteIndex: {0}", playerDataManager.PreviewPaletteIndex);
        }

        public override void OnInspectorGUI()
        {      
            DrawDefaultInspector();

            PlayerDataManager playerDataManager = (PlayerDataManager)target;
            
            GUILayout.Space(10.0f);
            GUILayout.BeginVertical("HelpBox");
            GUILayout.Space(4.0f);
            GUILayout.BeginHorizontal();
            if (GUILayout.Button("Clear saved data", GUILayout.MaxWidth(200.0f), GUILayout.MinWidth(0.0f), GUILayout.ExpandWidth(false)))
            {
                ClearSavedData();
            }
            if (GUILayout.Button("Update data", GUILayout.MaxWidth(200.0f), GUILayout.MinWidth(0.0f), GUILayout.ExpandWidth(false)))
            {
                UpdateDataView();
            }
            GUILayout.EndHorizontal();
            GUILayout.Space(10.0f);
            GUILayout.BeginVertical("HelpBox");

            GUILayout.Label("SavedData:");
            GUILayout.Label(_savedDataMessage);
            
            GUILayout.Label("RuntimeData:");
            GUILayout.Label(_runtimeDataMessage);

            GUILayout.EndVertical();
            GUILayout.EndVertical();
        }

        private void ClearSavedData()
        {
            PlayerDataManager playerDataManager = (PlayerDataManager)target;
            playerDataManager.ClearSavedData();
            UpdateDataView();
        }

        private void UpdateDataView()
        {
            PlayerDataManager playerDataManager = (PlayerDataManager)target;

            _savedDataMessage = string.Empty;
            _savedDataMessage += string.Format("\tBestScore: {0}", DataManager.Instance.GetInt("bestScore", 0));
            _savedDataMessage += string.Format("\n\tCurrency: {0}", DataManager.Instance.GetInt("currency", 0));
            _savedDataMessage += string.Format("\n\tIsApplicationStartedForTheFirstTime: {0}", DataManager.Instance.GetInt("isFirstTimeApplicationIsStarted", 1) != 0 ? "true" : "false");

            _runtimeDataMessage = string.Empty;
            _runtimeDataMessage += string.Format("\tScore: {0}", playerDataManager.Score);
            _runtimeDataMessage += string.Format("\n\tBestScore: {0}", playerDataManager.BestScore);
            _runtimeDataMessage += string.Format("\n\tCurrency: {0}", playerDataManager.Currency);
            _runtimeDataMessage += string.Format("\n\tCurrentPaletteIndex: {0}", playerDataManager.CurrentPaletteIndex);
            _runtimeDataMessage += string.Format("\n\tSelectedPaletteIndex: {0}", playerDataManager.SelectedPaletteIndex);
            _runtimeDataMessage += string.Format("\n\tPreviewPaletteIndex: {0}", playerDataManager.PreviewPaletteIndex);
        }
    }
}
