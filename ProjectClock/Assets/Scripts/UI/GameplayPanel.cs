using RoadToAAA.ProjectClock.Managers;
using RoadToAAA.ProjectClock.Scriptables;
using RoadToAAA.ProjectClock.Utilities;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UIElements;

namespace RoadToAAA.ProjectClock.UI
{
    public class GameplayPanel : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI ScoreText;
        [SerializeField] private FadingText ComboText;

        private GameObject _canvas;

        // TODO Remember to move it in PlayerData
        private int _score = 0;

        private void OnEnable()
        {
            EventManager<ECheckResult, ComboResult>.Instance.Subscribe(EEventType.OnCheckerResult, UpdateScore);
        }

        private void OnDisable()
        {
            EventManager<ECheckResult, ComboResult>.Instance.Unsubscribe(EEventType.OnCheckerResult, UpdateScore);
        }

        private void Start()
        {
            _canvas = transform.parent.gameObject;
        }

        private void UpdateScore(ECheckResult checkResult, ComboResult comboResult)
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
                        ComboState comboState = comboAsset.ComboStates[comboResult.StateIndex];

                        _score += comboState.Score;
                        ScoreText.text = _score.ToString();

                        ShowComboText(comboState.Message, comboState.MessageColor);
                        return;
                    }
                }           
            }
        }

        private void ShowComboText(string message, Color color)
        {
            FadingText text = Instantiate(ComboText);
            text.transform.localPosition = Camera.main.transform.position;
            text.transform.localRotation = Quaternion.identity;
            text.transform.localScale = Vector2.one;
            text.transform.SetParent(_canvas.transform, false);
            text.Initialize(message, color);
        }
    }
}
