using RoadToAAA.ProjectClock.Core;
using RoadToAAA.ProjectClock.Scriptables;
using System.Collections.Generic;
using TMPro;
using UnityEngine.UI;
using UnityEngine;

namespace RoadToAAA.ProjectClock.UI
{
    public class TutorialPanel : MonoBehaviour
    {
        [SerializeField] private Button _gotItButton;

        private void OnEnable()
        {
            _gotItButton.onClick.AddListener(GotItButtonPressed);
        }

        private void OnDisable()
        {
            _gotItButton.onClick.RemoveListener(GotItButtonPressed);
        }

        public void GotItButtonPressed()
        {
            DataManager.Instance.SaveInt("isFirstTimeApplicationIsStarted", 0);
            EventManager.Instance.Publish(EEventType.OnTutorialGotItButtonPressed);
        }
    }

}