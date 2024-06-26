using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RoadToAAA.ProjectClock.Core;
using TMPro;

namespace RoadToAAA.ProjectClock.UI
{
    public class GameOverPanel : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI ScoreText;
        [SerializeField] private TextMeshProUGUI HighScoreText;


        private void OnEnable()
        {
            
        }


        public void Initialize(int score, int bestScore)
        {
            ScoreText.text = "Score: " + score.ToString();
            HighScoreText.text = "Best score: " + bestScore.ToString();
        }

        public void RetryButton()
        {
            EventManager.Instance.Publish(EEventType.OnRetryButtonPressed);
        }

        public void ShopButton()
        {
            EventManager.Instance.Publish(EEventType.OnShopButtonPressed);
        }
    }
}
