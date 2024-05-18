using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VFXManager : MonoBehaviour
{
    [SerializeField] private GameObject ParticleSystemPrefab;


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
        if (oldClockGO == null) return;

        Clock clock = oldClockGO.GetComponent<Clock>();
        Transform handTransform = clock.GetClockHandTransform();

        GameObject ParticleSystemGO = Instantiate(ParticleSystemPrefab);
        //ParticleSystem particleSystem = ParticleSystemGO.GetComponent<ParticleSystem>();
        //ParticleSystem.ShapeModule particleShape = particleSystem.shape;
        ParticleSystemGO.transform.position = handTransform.position;
        ParticleSystemGO.transform.rotation = handTransform.rotation;
    }
}
