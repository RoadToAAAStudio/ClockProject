using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RoadToAAA.ProjectClock.Core;
using TMPro;
using UnityEngine.UI;
using System;
using RoadToAAA.ProjectClock.Utilities;

namespace RoadToAAA.ProjectClock.UI
{
    public class GameOverPanel : MonoBehaviour
    {
        [SerializeField] private Button _mainMenuButton;
        [SerializeField] private Button _adsButton;
        [SerializeField] private TextMeshProUGUI _scoreText;
        [SerializeField] private TextMeshProUGUI _currencyText;
        [SerializeField] private GameObject _currencyGO;

        private Vector3 _initialScaleAdsButton;
        private IEnumerator _wobbleAnimation;
            
        private void OnEnable()
        {
            Initialize();

            _mainMenuButton.onClick.AddListener(MainMenuButtonClicked);
            _adsButton.onClick.AddListener(AdsButtonClicked);
            _initialScaleAdsButton = _adsButton.transform.localScale;

            _mainMenuButton.interactable = true;
            _adsButton.interactable = PlayerDataManager.Instance.RunCurrency > 0;

            InitialAnimation();

            EventManager.Instance.Subscribe(EEventType.OnAdLoaded, AdLoaded);
            EventManager<int>.Instance.Subscribe(EEventType.OnAdRewardApplied, UpdateCurrencyForAd);
            EventManager<int>.Instance.Subscribe(EEventType.OnRunCurrencyChanged, UpdateCurrency);
        }

        private void InitialAnimation()
        {
            StartCoroutine(EntranceAnimation(_scoreText.transform, 0.0f, true, 2.0f));
            StartCoroutine(EntranceAnimation(_currencyGO.transform, 0.4f, true, 2.0f));
            StartCoroutine(EntranceAnimation(_mainMenuButton.transform, 1.2f, false, 2.0f));
            StartCoroutine(EntranceAnimation(_adsButton.transform, 1.2f, false, 2.0f));

            if (PlayerDataManager.Instance.RunCurrency > 0)
            {
                _wobbleAnimation = WobbleAnimation(_adsButton.transform, 0.1f, 6.0f);
                StartCoroutine(_wobbleAnimation);
            }
        }

        private void OnDisable()
        {
            _mainMenuButton.onClick.RemoveAllListeners();
            _adsButton.onClick.RemoveAllListeners();

            StopAllCoroutines();
            _scoreText.text = "0";
            _currencyText.text = "0";
            _adsButton.transform.localScale = _initialScaleAdsButton;
            EventManager.Instance.Unsubscribe(EEventType.OnAdLoaded, AdLoaded);
            EventManager<int>.Instance.Unsubscribe(EEventType.OnAdRewardApplied, UpdateCurrencyForAd);
            EventManager<int>.Instance.Unsubscribe(EEventType.OnRunCurrencyChanged, UpdateCurrency);
        }

        private void Initialize()
        {
            _scoreText.text = PlayerDataManager.Instance.Score.ToString();
            //_highScoreText.text = PlayerDataManager.Instance.BestScore.ToString();
            _adsButton.interactable = false;
            UpdateCurrency(PlayerDataManager.Instance.RunCurrency);
        }

        private void MainMenuButtonClicked()
        {
            EventManager.Instance.Publish(EEventType.OnMainMenuButtonClicked);
        }

        private void AdsButtonClicked()
        {
            if (_wobbleAnimation != null)
            {
                StopCoroutine(_wobbleAnimation);
            }
            _adsButton.transform.localScale = _initialScaleAdsButton;
            _adsButton.interactable = false;
            EventManager.Instance.Publish(EEventType.OnAdsButtonClicked);
        }

        private void AdLoaded()
        {
            _adsButton.interactable = true;
        }

        private void UpdateCurrency(int currency)
        {
            //_currencyText.text = currency.ToString();
            
            StartCoroutine(CurrencyAnimation(_currencyText, 0, currency, 0.05f, 0.9f));
        }

        private void UpdateCurrencyForAd(int currency)
        {
            StartCoroutine(CurrencyAnimation(_currencyText, int.Parse(_currencyText.text), currency, 0.05f, 0.3f));
        }

        private IEnumerator CurrencyAnimation(TextMeshProUGUI text, int oldCurrency, int newCurrency, float deltaTime, float delay)
        {
            if (delay > 0)
            {
                yield return new WaitForSeconds(delay);
            }

            while (oldCurrency < newCurrency)
            {
                oldCurrency++;
                text.text = oldCurrency.ToString();
                LayoutRebuilder.ForceRebuildLayoutImmediate((RectTransform)_currencyText.transform.parent);
                yield return new WaitForSeconds(deltaTime);
            }
        }

        private IEnumerator EntranceAnimation(Transform transform, float delay, bool controlPos, float speed = 1.0f)
        {
            Graphic[] graphics = transform.GetComponentsInChildren<Graphic>();
            foreach (Graphic g in graphics)
            {
                g.color = new Color(g.color.r, g.color.g, g.color.b, Mathf.Lerp(0.0f, 1.0f, 0.0f));
            }
            Button[] buttons = transform.GetComponentsInChildren<Button>();
            bool[] areButtonsInteractable = new bool[buttons.Length];
            for (int i = 0; i < buttons.Length; i++)
            {
                areButtonsInteractable[i] = buttons[i].interactable;
                Debug.Log(string.Format("GO {0} isInteractable {1}", buttons[i].gameObject.name, areButtonsInteractable[i]));
            }

            foreach (Button button in buttons)
            {
                button.interactable = false;
            }


            yield return null;
            yield return null;

            if (delay > 0)
            {
                yield return new WaitForSeconds(delay);
            }

            Vector3 endPos = transform.position;
            Vector3 startPos = transform.position + Vector3.up * 300.0f;

            float t = 0.0f;
            while(t < 1.0f)
            {
                t += speed * Time.deltaTime;
                t = Mathf.Clamp01(t);
                // Position
                if (controlPos)
                {
                    float transformedT = Utility.EaseOutSine(t);
                    transformedT = Mathf.Clamp01(transformedT);
                    transform.position = Vector3.Lerp(startPos, endPos, transformedT);
                }

                // Alpha
                foreach (Graphic g in graphics)
                {
                    g.color = new Color(g.color.r, g.color.g, g.color.b, Mathf.Lerp(0.0f, 1.0f, t));
                }
                yield return null;
            }

            for (int i = 0; i < buttons.Length; i++)
            {
                buttons[i].interactable = areButtonsInteractable[i];
            }
        }

        private IEnumerator WobbleAnimation(Transform transform, float intensity, float frequency)
        {
            float t = 0.0f;
            while(true)
            {
                t += Time.deltaTime;
                transform.localScale = Vector3.one * (_initialScaleAdsButton.x + intensity * Mathf.Sin(t * frequency));
                yield return null;
            }
        }
    }
}
