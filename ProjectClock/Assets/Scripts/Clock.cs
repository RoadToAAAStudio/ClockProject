using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Clock : MonoBehaviour
{
    [SerializeField] private GameObject HandGO;
    public float AngularVelocity = 1.0f;

    private Transform _hand;
    private SpriteRenderer _renderer;

    public LineRenderer _circle;

    private void Awake()
    {
        _hand = HandGO.GetComponent<Transform>();
        _renderer = HandGO.transform.GetChild(0).GetComponent<SpriteRenderer>();
        _renderer.color = new Color(0.25f, 0.25f, 0.25f);

        //_circle.positionCount = 60;
        //_circle.startWidth = 0.02f;
        //_circle.endWidth = 0.02f;
        //_circle.loop = true;
        //_circle.material.color = new Color(0.04f, 0.5f, 0.9f);
        //for (int i = 0; i < 60; i++) 
        //{
        //    float circumferenceProgress = (float)i / 60;
        //    float currentRadian = circumferenceProgress * 2 * Mathf.PI;
        //    float xScaled = Mathf.Cos(currentRadian);
        //    float yScaled = Mathf.Sin(currentRadian);
        //    float x = xScaled * 1;
        //    float y = yScaled * 1;

        //    Vector3 currentPosition = new Vector3(x, y, 0) + transform.position;
        //    _circle.SetPosition(i, currentPosition);
        //}
    }

    private void OnEnable()
    {
        //HandGO.SetActive(true);
    }

    private void OnDisable()
    {
        //HandGO.SetActive(false);
        _renderer.color = new Color(0.25f, 0.25f, 0.25f);
    }

    void Update()
    {
        _hand.eulerAngles = new Vector3(0.0f, 0.0f, _hand.eulerAngles.z + AngularVelocity * Time.deltaTime);
    }

    public void ChangeHandColor(Color color)
    {
        _renderer.color = color;
    }

    public Transform GetClockHandTransform()
    {
        return _hand;
    }
}
