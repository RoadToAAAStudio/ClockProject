using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class UIManager : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI scoreText;
    [SerializeField] private FadingText scoreFadingText;
    [SerializeField] private Canvas canvas;

    [SerializeField] private string successString = "+1";
    [SerializeField] private string perfectString = "PEFFOZZA +3";

    int score = 0;

    private void Start()
    {

    }

    private void OnEnable()
    {
        EventManagerOneParam<CheckState>.Instance.StartListening("onNewClock", AddScore);
    }

    private void OnDisable()
    {
        EventManagerOneParam<CheckState>.Instance.StopListening("onNewClock", AddScore);
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
        }

        scoreText.text = score.ToString();
    }

    private void SpawnText(Vector2 position, string message, Color color)
    {
        Instantiate(scoreFadingText, Camera.main.WorldToScreenPoint(position), Quaternion.identity, canvas.transform).Init(message, color);
    }
}
