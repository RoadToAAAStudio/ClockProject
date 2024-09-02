using RoadToAAA.ProjectClock.Managers;
using RoadToAAA.ProjectClock.Scriptables;
using RoadToAAA.ProjectClock.Core;
using UnityEngine;
using RoadToAAA.ProjectClock.Utilities;
using TMPro;
using System;

namespace RoadToAAA.ProjectClock.UI
{
    public class ComboTextSpawner : MonoBehaviour
    {
        [SerializeField] private FadingText ComboText;
        [SerializeField] private FadingText SpecialClockText;

        private StaticPool _comboTextPool;
        private StaticPool _specialClockTextPool;

        private void Awake()
        {
            _comboTextPool = new StaticPool(ComboText.gameObject, transform, 4);
            _specialClockTextPool = new StaticPool(SpecialClockText.gameObject, transform, 2);
        }

        private void OnEnable()
        {
            EventManager<ECheckResult, ComboResult>.Instance.Subscribe(EEventType.OnCheckerResult, SpawnComboText);
            EventManager<int, Clock>.Instance.Subscribe(EEventType.OnSpecialClockCleared, SpawnSpecialClockText);
        }

        private void OnDisable()
        {
            EventManager<ECheckResult, ComboResult>.Instance.Unsubscribe(EEventType.OnCheckerResult, SpawnComboText);
            EventManager<int, Clock>.Instance.Unsubscribe(EEventType.OnSpecialClockCleared, SpawnSpecialClockText);
        }


        private void SpawnComboText(ECheckResult checkResult, ComboResult comboResult)
        {
            if (checkResult == ECheckResult.Unsuccess)
            {
                return;
            }
            else
            {
                switch (comboResult.Type)
                {
                    case EComboResult.None:
                        break;
                    case EComboResult.Progress:
                    case EComboResult.NewState:
                    case EComboResult.Saturated:
                    case EComboResult.Fail:
                        ComboAsset comboAsset = ConfigurationManager.Instance.ComboAsset;
                        PaletteAsset paletteAsset = ConfigurationManager.Instance.PaletteAssets[PlayerDataManager.Instance.CurrentPaletteIndex];

                        ComboState comboState = comboAsset.ComboStates[comboResult.StateIndex];

                        ShowComboText(comboState.Message, paletteAsset.ComboColors[comboResult.StateIndex]);
                        return;
                }
            }
        }

        private void SpawnSpecialClockText(int currencyAmount, Clock specialClock)
        {
            PaletteAsset paletteAsset = ConfigurationManager.Instance.PaletteAssets[PlayerDataManager.Instance.CurrentPaletteIndex];
            ShowSpecialClockText(string.Format("+{0}", currencyAmount), paletteAsset.SpecialClockColor, specialClock);
        }

        private void ShowComboText(string message, Color color)
        {
            GameObject comboTextGameObject = _comboTextPool.Get(true);
            comboTextGameObject.transform.position = new Vector2(Camera.main.transform.position.x, Camera.main.transform.position.y);
            FadingText fadingText = comboTextGameObject.GetComponent<FadingText>();
            TextMeshProUGUI textMeshProUGUI = comboTextGameObject.GetComponent<TextMeshProUGUI>();
            textMeshProUGUI.text = message;
            textMeshProUGUI.color = color;
            fadingText.SetPool(_comboTextPool);
        }

        private void ShowSpecialClockText(string message, Color color, Clock specialClock)
        {
            GameObject specialClockTextGameObject = _specialClockTextPool.Get(true);
            specialClockTextGameObject.transform.position = new Vector2(Camera.main.transform.position.x - 0.5f, Camera.main.transform.position.y + 0.25f);
            FadingText fadingText = specialClockTextGameObject.GetComponent<FadingText>();
            TextMeshProUGUI textMeshProUGUI = specialClockTextGameObject.GetComponent<TextMeshProUGUI>();
            textMeshProUGUI.text = message;
            textMeshProUGUI.color = color;
            fadingText.SetPool(_specialClockTextPool);

            //GameObject specialClockTextGameObject = _specialClockTextPool.Get(true);
            //specialClockTextGameObject.transform.position = specialClock.ClockTransform.position + new Vector3(-1.0f, 0.0f);
            //specialClockTextGameObject.transform.localScale = new Vector3(2.0f, 2.0f);
            //FadingText fadingText = specialClockTextGameObject.GetComponent<FadingText>();
            //TextMeshProUGUI textMeshProUGUI = specialClockTextGameObject.GetComponent<TextMeshProUGUI>();
            //textMeshProUGUI.text = message;
            //textMeshProUGUI.color = color;
            //fadingText.SetPool(_specialClockTextPool);
        }
    }
}
