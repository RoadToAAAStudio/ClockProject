using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

namespace RoadToAAA.ProjectClock.Manager
{
    public class UIManager : Singleton<UIManager>
    {
        [SerializeField] private TextMeshProUGUI scoreText;
        [SerializeField] private FadingText scoreFadingText;
        [SerializeField] private Vector2 fadingTextPosition;
        [SerializeField] private Canvas canvas;

        [SerializeField] private GameObject gameoverPanel;
        [SerializeField] private GameObject mainMenuPanel;

        [SerializeField] private string successString = "+1";
        [SerializeField] private string perfectString = "PERFECT +3";

        public int score = 0;
        public int perfectScore = 3;

        private void OnEnable()
        {
            Core.EventManager.Instance.Subscribe(Core.EventType.OnMainMenu, MainMenu);

            EventManagerOneParam<CheckState>.Instance.StartListening("onNewClock", AddScore);
            EventManager.Instance.StartListening("onGameover", Gameover);
        }

        private void OnDisable()
        {
            EventManagerOneParam<CheckState>.Instance.StopListening("onNewClock", AddScore);
            EventManager.Instance.StopListening("onGameover", Gameover);
        }

        private void AddScore(CheckState state)
        {
            switch (state)
            {
                case CheckState.SUCCESS:
                    score++;
                    SpawnText(Camera.main.transform.position, successString, Color.white);
                    break;

                case CheckState.PERFECT:
                    score += perfectScore;
                    SpawnText(Camera.main.transform.position, perfectString, Color.yellow);
                    break;

                case CheckState.COMBO:

                    score += ComboManager.Instance.CurrentCombo.score;
                    SpawnText(Camera.main.transform.position, ComboManager.Instance.CurrentCombo.message, ComboManager.Instance.CurrentCombo.messageColor);
                    break;
            }

            scoreText.text = score.ToString();
        }

        private void SpawnText(Vector2 position, string message, Color color)
        {
            FadingText text = Instantiate(scoreFadingText);
            text.transform.localPosition = position;
            text.transform.localRotation = Quaternion.identity;
            text.transform.localScale = Vector2.one;
            text.transform.SetParent(canvas.transform, false);
            text.Init(message, color);
        }

        private void Gameover()
        {
            int bestScore = DataManager.Instance.GetInt("bestScore");
            if (score > bestScore)
            {
                bestScore = score;
                DataManager.Instance.SaveInt("bestScore", bestScore);
            }
            gameoverPanel.GetComponent<GameoverPanel>().Initialize(score, bestScore);
            gameoverPanel.SetActive(true);
        }

        private void MainMenu()
        {
            mainMenuPanel.SetActive(true);
        }
    }
}
