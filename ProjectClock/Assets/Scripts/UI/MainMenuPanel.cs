using UnityEngine;
using RoadToAAA.ProjectClock.Core;
using UnityEngine.UI;

namespace RoadToAAA.ProjectClock.UI
{
    public class MainMenuPanel : MonoBehaviour
    {
        [SerializeField] private Sprite _volumeOnIcon;
        [SerializeField] private Sprite _volumeOffIcon;
        [SerializeField] private Image _audioButtonImage;

        private int _audioButtonState;

        private const string AUDIOBUTTONSTATE = "AudioButtonState";

        private void Start()
        {
            _audioButtonState = DataManager.Instance.GetInt(AUDIOBUTTONSTATE, 1);
            if (_audioButtonState == 1)
            {
                _audioButtonImage.sprite = _volumeOnIcon;
            }
            else
            {
                _audioButtonImage.sprite = _volumeOffIcon;
            }
            EventManager<int>.Instance.Publish(EEventType.OnAudioButtonPressed, _audioButtonState);
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
            if (_audioButtonState == 1)
            {
                _audioButtonState = 0;
                _audioButtonImage.sprite = _volumeOffIcon;
                DataManager.Instance.SaveInt(AUDIOBUTTONSTATE, _audioButtonState);
                EventManager<int>.Instance.Publish(EEventType.OnAudioButtonPressed, _audioButtonState);
            }
            else
            {
                _audioButtonState = 1;
                _audioButtonImage.sprite = _volumeOnIcon;
                DataManager.Instance.SaveInt(AUDIOBUTTONSTATE, _audioButtonState);
                EventManager<int>.Instance.Publish(EEventType.OnAudioButtonPressed, _audioButtonState);
            }
        }
    }
}
