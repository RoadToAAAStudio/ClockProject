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

    public float GetTimeToDespawn() => _timeToDespawn;
    public float GetTimeOnScreen() => _timeOnScreen;

    public void Initialize(string message, Color color)
    {
        _textComponent = GetComponent<TextMeshProUGUI>();
        _textComponent.text = message;
        _textComponent.color = color;
        StartCoroutine(FadeCO());
    }

    private IEnumerator FadeCO()
    {
        yield return new WaitForSeconds(_timeOnScreen);

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
