using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RoadToAAA.ProjectClock.Core;
using TMPro;
using UnityEngine.UI;

namespace RoadToAAA.ProjectClock.UI
{
    public class GameOverPanel : MonoBehaviour
    {
        [SerializeField] private Button _mainMenuButton;
        [SerializeField] private TextMeshProUGUI _scoreText;
        [SerializeField] private TextMeshProUGUI _highScoreText;

        private void OnEnable()
        {
            _mainMenuButton.onClick.AddListener(MainMenuButtonClicked);

            Initialize();
        }

        private void OnDisable()
        {
            _mainMenuButton.onClick.RemoveAllListeners();
        }

        public void Initialize()
        {
            _scoreText.text = PlayerDataManager.Instance.Score.ToString();
            _highScoreText.text = PlayerDataManager.Instance.BestScore.ToString();
        }

        public void MainMenuButtonClicked()
        {
            EventManager.Instance.Publish(EEventType.OnMainMenuButtonClicked);
        }
    }
}
