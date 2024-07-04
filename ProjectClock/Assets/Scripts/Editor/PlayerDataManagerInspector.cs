using UnityEngine;
using UnityEditor;
using RoadToAAA.ProjectClock.Core;
using UnityEditor.TerrainTools;
using Codice.Utils;

namespace RoadToAAA.ProjectClock.Editors
{
    [CustomEditor(typeof(PlayerDataManager))]
    public class PlayerDataManagerInspector : Editor
    {
        public override void OnInspectorGUI()
        {
            DrawDefaultInspector();

            PlayerDataManager playerDataManager = (PlayerDataManager)target;
            GUILayout.Space(10.0f);
            GUILayout.BeginVertical("HelpBox");
            GUILayout.Space(4.0f);
            if (GUILayout.Button("Clear saved data", GUILayout.MaxWidth(200.0f), GUILayout.MinWidth(0.0f), GUILayout.ExpandWidth(false)))
            {
                playerDataManager.ClearSavedData();
            }
            GUILayout.Space(10.0f);
            GUILayout.BeginVertical("HelpBox");

            string savedDataMessage = string.Empty;
            savedDataMessage += string.Format("\tBestScore: {0}", DataManager.Instance.GetInt("bestScore", 0));
            savedDataMessage += string.Format("\n\tCurrency: {0}", DataManager.Instance.GetInt("currency", 0));
            GUILayout.Label("SavedData:");
            GUILayout.Label(savedDataMessage);
            GUILayout.Label("RuntimeData:");

            string runtimeDataMessage = string.Empty;
            runtimeDataMessage += string.Format("\tScore: {0}", playerDataManager.Score);
            runtimeDataMessage += string.Format("\n\tScore: {0}", playerDataManager.Score);
            GUILayout.Label(runtimeDataMessage);
            GUILayout.EndVertical();
            GUILayout.EndVertical();
        }
    }
}
