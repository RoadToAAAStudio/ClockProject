using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using RoadToAAA.ProjectClock.Core;

namespace RoadToAAA.ProjectClock.Scriptables
{
    [CustomEditor(typeof(ConfigurationManager))]
    public class ConfigurationManagerInspector : Editor
    {
        private GUIStyle _validationResultSytle;

        private ScriptableObjectValidateResult _validationResult = new ScriptableObjectValidateResult();

        private void Awake()
        {
            _validationResultSytle = new GUIStyle();
            _validationResultSytle.normal.textColor = Color.white;
        }

        public override void OnInspectorGUI()
        {
            DrawDefaultInspector();

            ConfigurationManager myScript = (ConfigurationManager)target;
            GUILayout.Space(10.0f);
            GUILayout.BeginVertical("HelpBox");
            GUILayout.BeginVertical("Label");
            GUILayout.Space(4.0f);
            if (GUILayout.Button("Validate configurations", GUILayout.MaxWidth(200.0f), GUILayout.MinWidth(0.0f), GUILayout.ExpandWidth(false)))
            {
                _validationResult = myScript.ValidateConfigurations();
            }
            GUILayout.Space(10.0f);
            GUILayout.BeginVertical("HelpBox");
            GUILayout.Label("Result:", _validationResultSytle);
            GUI.color = _validationResult.IsValid ? Color.green : Color.red;
            GUILayout.Label(_validationResult.Message, _validationResultSytle);
            GUILayout.EndVertical();
            GUILayout.EndVertical();
            GUILayout.EndVertical();
        }
    }
}

