using UnityEngine;

public class VFXManager : MonoBehaviour
{
    [SerializeField] private GameObject ParticleSystemPrefab;
    [SerializeField] private ClockController ClockController;

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
        ParticleSystem particleSystem = ParticleSystemGO.GetComponent<ParticleSystem>();

        ParticleSystemGO.transform.position = handTransform.position;
        ParticleSystemGO.transform.rotation = handTransform.rotation;

        ParticleSystem.MainModule particleMain = particleSystem.main;
        particleMain.startColor = ClockController.GetOldColor();
    }
}
