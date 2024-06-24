using RoadToAAA.ProjectClock.Core;
using RoadToAAA.ProjectClock.Managers;
using RoadToAAA.ProjectClock.Scriptables;
using RoadToAAA.ProjectClock.Utilities;
using TMPro;
using UnityEngine;

namespace RoadToAAA.ProjectClock.UI
{
    public class GameplayPanel : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI ScoreText;

        // TODO Remember to move it in PlayerData
        //private int _score = 0;

        private void Awake()
        {
            ScoreText.text = PlayerData.Instance.Score.ToString();
        }

        private void OnEnable()
        {
            //EventManager<ECheckResult, ComboResult>.Instance.Subscribe(EEventType.OnCheckerResult, UpdateScore);
            EventManager<int>.Instance.Subscribe(EEventType.OnScoreChanged, UpdateScoreText);
        }

        private void OnDisable()
        {
            //EventManager<ECheckResult, ComboResult>.Instance.Unsubscribe(EEventType.OnCheckerResult, UpdateScore);
            EventManager<int>.Instance.Unsubscribe(EEventType.OnScoreChanged, UpdateScoreText);
        }

        private void UpdateScoreText(int newScore)
        {
            ScoreText.text = newScore.ToString();
        }

        //private void UpdateScore(ECheckResult checkResult, ComboResult comboResult)
        //{
        //    if (checkResult == ECheckResult.Unsuccess)
        //    {
        //        return;
        //    }
        //    else
        //    {
        //        switch (comboResult.Type)
        //        {
        //            case EComboResult.None:
        //            {
        //                break;
        //            }
        //            default:
        //            {
        //                ComboAsset comboAsset = ConfigurationManager.Instance.ComboAsset;
        //                ComboState comboState = comboAsset.ComboStates[comboResult.StateIndex];
        //                _score += comboState.Score;
        //                ScoreText.text = _score.ToString();
        //                return;
        //            }
        //        }           
        //    }
        //}
    }
}
