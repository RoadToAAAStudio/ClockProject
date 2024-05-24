using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClockController : MonoBehaviour
{
    public List<Color> Colors = new List<Color>();
    public float CurrentVelocity = 0.0f;
    public int CurrentClock = 0;
    public float MaxAngularVelocity = 280f;

    private Color _currentColor = new Color(1.0f, 1.0f, 1.0f);
    private Color _oldColor = new Color(1.0f, 1.0f, 1.0f);

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

    public Color GetOldColor()
    {
        return _oldColor;
    }

    private void NewClockSelected(GameObject newClockGO, GameObject oldClockGO)
    {
        // Set Color
        Color newColor = Colors[Random.Range(0, Colors.Count)];
        _oldColor = _currentColor;
        _currentColor = newColor;

        Colors.Remove(newColor);
        Colors.Add(_oldColor);
        newClockGO.GetComponent<Clock>().ChangeHandColor(newColor);

        // Add Angular Velocity
        Clock newClock = newClockGO.GetComponent<Clock>();
        if (oldClockGO != null) 
        {         
            Clock oldClock = oldClockGO.GetComponent<Clock>();
            newClock.HandAngularVelocity = newClock.HandAngularVelocity > 0 ? Mathf.Abs(oldClock.HandAngularVelocity) + 1 : -(Mathf.Abs(oldClock.HandAngularVelocity) + 1);
            newClock.HandAngularVelocity = Mathf.Clamp(newClock.HandAngularVelocity, -MaxAngularVelocity, MaxAngularVelocity);
            oldClock.ChangeHandColor(oldClock.HandDeactivatedColor);
            oldClock.DrawHand(oldClock.CurrentAngle());
        }

        CurrentVelocity = Mathf.Abs(newClock.HandAngularVelocity);
        CurrentClock++;
    }
}
