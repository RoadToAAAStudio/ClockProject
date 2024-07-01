using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using RoadToAAA.ProjectClock.Core;
using UnityEngine.UI;

namespace RoadToAAA.ProjectClock.UI
{
    public class MainMenuPanel : MonoBehaviour
    {

        [SerializeField] Sprite _volumeOnIcon;
        [SerializeField] Sprite _volumeOffIcon;
        [SerializeField] Image _audioButtonImage;

        private int AudioButtonState;

        private const string AUDIOBUTTONSTATE = "AudioButtonState";

        private void Start()
        {
            AudioButtonState = DataManager.Instance.GetInt(AUDIOBUTTONSTATE, 1);
            if (AudioButtonState == 1)
            {
                _audioButtonImage.sprite = _volumeOnIcon;
            }
            else
            {
                _audioButtonImage.sprite = _volumeOffIcon;
            }
            EventManager<int>.Instance.Publish(EEventType.OnAudioButtonPressed, AudioButtonState);
        }
        public void PlayButton()
        {
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
            if (AudioButtonState == 1)
            {
                AudioButtonState = 0;
                _audioButtonImage.sprite = _volumeOffIcon;
                DataManager.Instance.SaveInt(AUDIOBUTTONSTATE, AudioButtonState);
                EventManager<int>.Instance.Publish(EEventType.OnAudioButtonPressed, AudioButtonState);
            }
            else
            {
                AudioButtonState = 1;
                _audioButtonImage.sprite = _volumeOnIcon;
                DataManager.Instance.SaveInt(AUDIOBUTTONSTATE, AudioButtonState);
                EventManager<int>.Instance.Publish(EEventType.OnAudioButtonPressed, AudioButtonState);
            }
        }
    }
}
