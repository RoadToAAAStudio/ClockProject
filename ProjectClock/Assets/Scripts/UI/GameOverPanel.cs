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

        private void OnEnable()
        {
            Initialize();

            _mainMenuButton.onClick.AddListener(MainMenuButtonClicked);
            _adsButton.onClick.AddListener(AdsButtonClicked);

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
        }

        private void OnDisable()
        {
            _mainMenuButton.onClick.RemoveAllListeners();
            _adsButton.onClick.RemoveAllListeners();

            StopAllCoroutines();

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

            foreach (Button button in buttons)
            {
                button.interactable = true;
            }
        }
    }
}
