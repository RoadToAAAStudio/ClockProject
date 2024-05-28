using System.Collections;
using System.Collections.Generic;
using Unity.PlasticSCM.Editor.WebApi;
using UnityEngine;

public class ClockSpawner : Singleton<ClockSpawner>
{
    [SerializeField] private Clock clockPrefab;
    
    public int clockToPass = 0;

    private Vector2 spawnPos = Vector2.zero;
    private Clock prevClock;
    private Clock currentClock;
    public float minRadius, maxRadius;

    private void OnEnable()
    {
        EventManager.Instance.StartListening("onNewClock", SpawnClock);
    }

    private void OnDisable()
    {
        EventManager.Instance.StopListening("onNewClock", SpawnClock);
    }

    private void Start()
    {
        for (int i = 0; i < 5; i++)
        {
            SpawnClock();
        }
        InputController.Instance.canCheck = true;
    }

    private void SpawnClock()
    {
        if (prevClock == null)
        {
            currentClock = Instantiate(clockPrefab, spawnPos, Quaternion.identity);
            currentClock.DrawClock();
            currentClock.DrawHand(270);
            currentClock.enabled = true;
        }
        else
        {
            float randomAngle = Random.Range(45, 135);
            Vector2 point = new Vector2((prevClock.ClockRadius * Mathf.Cos(randomAngle * Mathf.Deg2Rad) + prevClock.transform.position.x), (prevClock.ClockRadius * Mathf.Sin(randomAngle * Mathf.Deg2Rad) + prevClock.transform.position.y));
            Vector2 direction = (point - (Vector2)prevClock.transform.position).normalized;

            float radius = Random.Range(minRadius, maxRadius);
            Vector2 newPos = direction * radius + point + (direction * radius).normalized * currentClock.ClockWidth; // adding additianal space to spacing clocks
            currentClock = Instantiate(clockPrefab, newPos, Quaternion.identity);
            currentClock.ClockRadius = radius;
            currentClock.HandVelocityOnCircumference = -prevClock.HandVelocityOnCircumference;
            //currentClock.DrawClock();
            currentClock.DrawHand(Mathf.Atan2(-direction.y, -direction.x) * Mathf.Rad2Deg);

            // Success range of a clock is determined only when you spawn the next one
            prevClock.PerfectSuccessAngle = randomAngle;

            if (InputController.Instance.ClockPassed > clockToPass)
            {
                prevClock.SuccessZoneClockColor = prevClock.ClockColor;
            }

            // Drawing only after the subsequent clock has been spawned
            prevClock.DrawClock();
        }

        InputController.Instance.hands.Add(currentClock.gameObject);
        prevClock = currentClock;
    }

    private void Update()
    {
        
    }
}
