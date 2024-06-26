using RoadToAAA.ProjectClock.Core;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RoadToAAA.ProjectClock.UI
{
    public class LeaderboardPanel : MonoBehaviour
    {
        public void ReturnButtonClicked()
        {
            EventManager.Instance.Publish(EEventType.OnReturnButtonPressed);
        }
    }
}
