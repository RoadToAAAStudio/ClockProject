using RoadToAAA.ProjectClock.Utilities;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class FadingText : MonoBehaviour
{
    private TextMeshProUGUI _textComponent;
    private float _elapsedTime = 0;
    [SerializeField] private float _timeToDespawn = 0.75f;
    [SerializeField] private float _timeOnScreen = 0.5f;
    private WaitForSeconds _fadeDurationWait;
    private IEnumerator _coroutine;
    private StaticPool _pool;

    public float GetTimeToDespawn() => _timeToDespawn;
    public float GetTimeOnScreen() => _timeOnScreen;

    private void Awake()
    {
        _textComponent = GetComponent<TextMeshProUGUI>();
        _fadeDurationWait = new WaitForSeconds(_timeOnScreen);
    }

    private void OnEnable()
    {
        _coroutine = FadeCO();
        StartCoroutine(_coroutine);
    }

    private IEnumerator FadeCO()
    {
        _elapsedTime = 0.0f;
        yield return _fadeDurationWait;

        while (_elapsedTime < _timeToDespawn)
        {
            _elapsedTime += Time.deltaTime;
            _textComponent.color = new Color(_textComponent.color.r, _textComponent.color.g, _textComponent.color.b, Mathf.Lerp(1, 0, _elapsedTime / _timeToDespawn));
            yield return null;
        }

        _pool.Release(gameObject);
    }

    public void SetPool(StaticPool pool)
    {
        _pool = pool;
    }
}
