using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClockController : MonoBehaviour
{
    public List<Color> Colors = new List<Color>();
    private Color _currentColor = new Color(1.0f, 1.0f, 1.0f);

    private void Awake()
    {
        _currentColor = Colors[Random.Range(0, Colors.Count)];
        Colors.Remove(_currentColor);
    }

    private void OnEnable()
    {
        EventManagerTwoParams<GameObject, GameObject>.Instance.StartListening("onNewClock", NewClockSelected);
    }

    private void OnDisable()
    {
        EventManagerTwoParams<GameObject, GameObject>.Instance.StopListening("onNewClock", NewClockSelected);
    }

    private void NewClockSelected(GameObject newClockGO, GameObject oldClockGO)
    {
        Color newColor = Colors[Random.Range(0, Colors.Count)];
        Colors.Remove(newColor);
        Colors.Add(_currentColor);
        _currentColor = newColor;
        newClockGO.GetComponent<Clock>().ChangeHandColor(newColor);
    }
}
