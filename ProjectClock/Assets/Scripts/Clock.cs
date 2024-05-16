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
        _renderer.color = Color.gray;
    }

    private void OnEnable()
    {
        //HandGO.SetActive(true);
        _renderer.color = Color.red;
    }

    private void OnDisable()
    {
        //HandGO.SetActive(false);
        _renderer.color = Color.gray;
    }

    void Update()
    {
        _hand.eulerAngles = new Vector3(0.0f, 0.0f, _hand.eulerAngles.z + AngularVelocity * Time.deltaTime);
    }
}
