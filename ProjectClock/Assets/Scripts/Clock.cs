using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Clock : MonoBehaviour
{
    [SerializeField] private GameObject HandGO;
    public float AngularVelocity = 1.0f;
    private Transform _hand;

    private void Awake()
    {
        _hand = GetComponent<Transform>();
    }

    private void OnEnable()
    {
        HandGO.SetActive(true);
    }

    private void OnDisable()
    {
        HandGO.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        _hand.eulerAngles = new Vector3(0, 0, _hand.eulerAngles.z + AngularVelocity * Time.deltaTime);
    }
}
