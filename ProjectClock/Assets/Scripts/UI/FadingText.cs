using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class FadingText : MonoBehaviour
{
    private TextMeshProUGUI _textComponent;
    private float _elapsedTime = 0;
    [SerializeField] private float TimeToDespawn = 0.75f;
    [SerializeField] private float TimeOnScreen = 0.5f;

    public void Initialize(string message, Color color)
    {
        _textComponent = GetComponent<TextMeshProUGUI>();
        _textComponent.text = message;
        _textComponent.color = color;
        StartCoroutine(FadeCO());
    }

    private IEnumerator FadeCO()
    {
        yield return new WaitForSeconds(TimeOnScreen);

        while (_elapsedTime < TimeToDespawn)
        {
            _elapsedTime += Time.deltaTime;
            _textComponent.alpha = Mathf.Lerp(1, 0, _elapsedTime / TimeToDespawn);
            yield return null;
        }

        if (_textComponent.alpha <= 0)
        {
            Destroy(gameObject);
        }
    }
}
