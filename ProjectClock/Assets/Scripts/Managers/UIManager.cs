using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class UIManager : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI scoreText;
    [SerializeField] private FadingText scoreFadingText;
    [SerializeField] private Canvas canvas;
    int score = 0;

    private void Start()
    {
        score--;
    }

    private void OnEnable()
    {
        EventManagerTwoParams<GameObject, GameObject>.Instance.StartListening("onNewClock", AddScore);
    }

    private void OnDisable()
    {
        EventManagerTwoParams<GameObject, GameObject>.Instance.StopListening("onNewClock", AddScore);
    }

    private void AddScore(GameObject clock, GameObject oldClock)
    {
        score++;
        scoreText.text = score.ToString();
        SpawnText(Camera.main.transform.position);
    }

    private void SpawnText(Vector2 position)
    {
        Instantiate(scoreFadingText, Camera.main.WorldToScreenPoint(position), Quaternion.identity, canvas.transform).Init("+1");
    }
}
