using RoadToAAA.ProjectClock.Core;
using TMPro;
using UnityEngine;

namespace RoadToAAA.ProjectClock.UI
{
    public class GameplayPanel : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI ScoreText;

        private void Awake()
        {
            ScoreText.text = PlayerData.Instance.Score.ToString();
        }

        private void OnEnable()
        {
            EventManager<int>.Instance.Subscribe(EEventType.OnScoreChanged, UpdateScoreText);
        }

        private void OnDisable()
        {
            EventManager<int>.Instance.Unsubscribe(EEventType.OnScoreChanged, UpdateScoreText);
        }

        private void UpdateScoreText(int newScore)
        {
            ScoreText.text = newScore.ToString();
        }
    }
}
