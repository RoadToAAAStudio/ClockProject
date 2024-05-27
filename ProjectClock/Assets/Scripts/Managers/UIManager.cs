using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class UIManager : Singleton<UIManager>
{
    [SerializeField] private TextMeshProUGUI scoreText;
    [SerializeField] private FadingText scoreFadingText;
    [SerializeField] private Canvas canvas;

    [SerializeField] private GameObject gameoverPanel;

    [SerializeField] private string successString = "+1";
    [SerializeField] private string perfectString = "PEFFOZZA +3";
    [SerializeField] private string comboString = "COMBO +5";

    public int score = 0;

    private void OnEnable()
    {
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
                score += 3;
                SpawnText(Camera.main.transform.position, perfectString, Color.yellow);
                break;

            case CheckState.COMBO:

                score += ComboManager.Instance.CurrentCombo.score;
                SpawnText(Camera.main.transform.position, ComboManager.Instance.CurrentCombo.message, Color.green);
                break;
        }

        scoreText.text = score.ToString();
    }

    private void SpawnText(Vector2 position, string message, Color color)
    {
        Instantiate(scoreFadingText, Camera.main.WorldToScreenPoint(position), Quaternion.identity, canvas.transform).Init(message, color);
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
}
