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

    public float GetTimeToDespawn() => _timeToDespawn;
    public float GetTimeOnScreen() => _timeOnScreen;

    private void Awake()
    {
        _textComponent = GetComponent<TextMeshProUGUI>();
        _fadeDurationWait = new WaitForSeconds(_timeOnScreen);
        _coroutine = FadeCO();
    }

    public void Initialize(string message, Color color)
    {
        _textComponent.text = message;
        _textComponent.color = color;
        StartCoroutine(_coroutine);
    }

    private IEnumerator FadeCO()
    {
        yield return _fadeDurationWait;

        while (_elapsedTime < _timeToDespawn)
        {
            _elapsedTime += Time.deltaTime;
            _textComponent.alpha = Mathf.Lerp(1, 0, _elapsedTime / _timeToDespawn);
            yield return null;
        }

        //if (_textComponent.alpha <= 0)
        //{
        //    Destroy(gameObject);
        //}
    }
}
