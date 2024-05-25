using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class GameoverPanel : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI scoreText;
    [SerializeField] private TextMeshProUGUI highScoreText;

    public void Initialize(int score, int bestScore)
    {
        scoreText.text = "Score: " + score.ToString();
        highScoreText.text = "Best score: " + bestScore.ToString();
    }
}
