using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class FadingText : MonoBehaviour
{
    private TextMeshProUGUI textComponent;
    public float timeToDespawn = 0.75f;
    public float timeOnScreen = 0.5f;
    private float elapsedTime = 0;

    private Coroutine FadeCoroutine;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    public void Init(string message, Color color)
    {
        textComponent = GetComponent<TextMeshProUGUI>();
        textComponent.text = message;
        textComponent.color = color;
        FadeCoroutine = StartCoroutine(FadeCO());
    }

    private IEnumerator FadeCO()
    {
        yield return new WaitForSeconds(timeOnScreen);

        while(elapsedTime < timeToDespawn)
        {
            elapsedTime += Time.deltaTime;
            textComponent.alpha = Mathf.Lerp(1, 0, elapsedTime / timeToDespawn);
            yield return null;
        }
        
        if (textComponent.alpha <= 0) Destroy(gameObject);
    }
}
