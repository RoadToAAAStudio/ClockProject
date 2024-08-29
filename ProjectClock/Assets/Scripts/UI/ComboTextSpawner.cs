using RoadToAAA.ProjectClock.Managers;
using RoadToAAA.ProjectClock.Scriptables;
using RoadToAAA.ProjectClock.Core;
using UnityEngine;
using RoadToAAA.ProjectClock.Utilities;
using static UnityEngine.ParticleSystem;
using System.Collections.Generic;
using TMPro;

namespace RoadToAAA.ProjectClock.UI
{
    public class ComboTextSpawner : MonoBehaviour
    {
        [SerializeField] private FadingText ComboText;
        private StaticPool _comboTextPool;
        private void Awake()
        {
            _comboTextPool = new StaticPool(ComboText.gameObject, transform, 4);
        }

        private void OnEnable()
        {
            EventManager<ECheckResult, ComboResult>.Instance.Subscribe(EEventType.OnCheckerResult, SpawnComboText);
        }

        private void OnDisable()
        {
            EventManager<ECheckResult, ComboResult>.Instance.Unsubscribe(EEventType.OnCheckerResult, SpawnComboText);
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
                        {
                            break;
                        }
                    default:
                        {
                            ComboAsset comboAsset = ConfigurationManager.Instance.ComboAsset;
                            PaletteAsset paletteAsset = ConfigurationManager.Instance.PaletteAssets[PlayerDataManager.Instance.CurrentPaletteIndex];

                            ComboState comboState = comboAsset.ComboStates[comboResult.StateIndex];

                            ShowComboText(comboState.Message, paletteAsset.ComboColors[comboResult.StateIndex]);
                            return;
                        }
                }
            }
        }

        private void ShowComboText(string message, Color color)
        {
            GameObject fadingTextGameObject = _comboTextPool.Get(true);
            fadingTextGameObject.transform.position = new Vector2(Camera.main.transform.position.x, Camera.main.transform.position.y);
            FadingText fadingText = fadingTextGameObject.GetComponent<FadingText>();
            TextMeshProUGUI textMeshProUGUI = fadingTextGameObject.GetComponent<TextMeshProUGUI>();
            textMeshProUGUI.text = message;
            textMeshProUGUI.color = color;
            fadingText.SetPool(_comboTextPool);

            //FadingText text = Instantiate(ComboText, new Vector2(Camera.main.transform.position.x, Camera.main.transform.position.y), Quaternion.identity, transform);
            //text.Initialize(message, color);
        }
    }
}
