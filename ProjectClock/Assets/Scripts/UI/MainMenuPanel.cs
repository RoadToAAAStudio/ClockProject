using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using RoadToAAA.ProjectClock.Core;

namespace RoadToAAA.ProjectClock.UI
{
    public class MainMenuPanel : MonoBehaviour
    {

        public void PlayButton()
        {
            Debug.Log("Play Pressed");
            EventManager.Instance.Publish(EEventType.OnPlayButtonPressed);
        }

        public void ShopButton()
        {
            EventManager.Instance.Publish(EEventType.OnShopButtonPressed); 
        }

        public void LeaderboardButton()
        {
            EventManager.Instance.Publish(EEventType.OnLeaderboardButtonPressed);
        }

        public void SoundButton()
        {
            EventManager.Instance.Publish(EEventType.OnAudioButtonPressed);
        }

    }
}
