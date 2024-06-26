using RoadToAAA.ProjectClock.Managers;
using RoadToAAA.ProjectClock.Scriptables;
using RoadToAAA.ProjectClock.Core;
using UnityEngine;
using RoadToAAA.ProjectClock.Utilities;

namespace RoadToAAA.ProjectClock.UI
{
    public class ComboTextSpawner : MonoBehaviour
    {
        [SerializeField] private FadingText ComboText;

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
                            PaletteAsset paletteAsset = ConfigurationManager.Instance.PaletteAssets[0];

                            ComboState comboState = comboAsset.ComboStates[comboResult.StateIndex];

                            ShowComboText(comboState.Message, paletteAsset.ComboColors[comboResult.StateIndex]);
                            return;
                        }
                }
            }
        }

        private void ShowComboText(string message, Color color)
        {
            FadingText text = Instantiate(ComboText, new Vector2(Camera.main.transform.position.x, Camera.main.transform.position.y), Quaternion.identity, transform);
            text.Initialize(message, color);
        }
    }
}
