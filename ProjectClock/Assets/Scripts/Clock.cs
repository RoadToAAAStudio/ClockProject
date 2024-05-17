using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Clock : MonoBehaviour
{
    [SerializeField] private GameObject HandGO;
    public float AngularVelocity = 1.0f;

    private Transform _hand;
    private SpriteRenderer _renderer;
    private void Awake()
    {
        _hand = GetComponent<Transform>();
        _renderer = HandGO.transform.GetChild(0).GetComponent<SpriteRenderer>();
        _renderer.color = new Color(0.25f, 0.25f, 0.25f);
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
}
