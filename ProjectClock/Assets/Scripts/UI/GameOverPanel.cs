using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RoadToAAA.ProjectClock.Utilities;
using TMPro;

namespace RoadToAAA.ProjectClock.UI
{
    public class GameOverPanel : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI scoreText;
        [SerializeField] private TextMeshProUGUI highScoreText;


        private void OnEnable()
        {
            
        }


        public void Initialize(int score, int bestScore)
        {
            scoreText.text = "Score: " + score.ToString();
            highScoreText.text = "Best score: " + bestScore.ToString();
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
