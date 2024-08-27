using UnityEngine;
using RoadToAAA.ProjectClock.Core;
using UnityEngine.UI;
using TMPro;
using System.Collections;

namespace RoadToAAA.ProjectClock.UI
{
    public class MainMenuPanel : MonoBehaviour
    {
        [SerializeField] private Sprite _volumeOnIcon;
        [SerializeField] private Sprite _volumeOffIcon;
        [SerializeField] private Image _audioButtonImage;
        [SerializeField] private Button _tutorialButton;
        [SerializeField] private TMP_Text _tapToPlayText;
        public float FadeSpeed = 1.0f;

        private int _audioButtonState;
        private IEnumerator _fadeInfadeOutCoroutine;

        private const string AUDIOBUTTONSTATE = "AudioButtonState";

        private void OnEnable()
        {
            _tutorialButton.onClick.AddListener(TutorialButtonPressed);
        }

        private void OnDisable()
        {
            _tutorialButton.onClick.RemoveListener(TutorialButtonPressed);
        }

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
            _fadeInfadeOutCoroutine = FadeInFadeOut();
            StartCoroutine(_fadeInfadeOutCoroutine);
            EventManager<int>.Instance.Publish(EEventType.OnAudioButtonPressed, _audioButtonState);
        }

        public void PlayButton()
        {
            StopCoroutine(_fadeInfadeOutCoroutine);
            EventManager.Instance.Publish(EEventType.OnPlayButtonPressed);
        }

        public void ShopButton()
        {
            StopCoroutine(_fadeInfadeOutCoroutine);
            EventManager.Instance.Publish(EEventType.OnShopButtonPressed); 
        }

        public void LeaderboardButton()
        {
            StopCoroutine(_fadeInfadeOutCoroutine);
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

        private void TutorialButtonPressed()
        {
            StopCoroutine(_fadeInfadeOutCoroutine);
            EventManager.Instance.Publish(EEventType.OnTutorialButtonPressed);
        }

        private IEnumerator FadeInFadeOut()
        {
            Color initialColor = _tapToPlayText.color;

            while(true)
            {
                float t = 0.0f;
                while(t < 1.0f)
                {
                    t += FadeSpeed * Time.deltaTime;
                    t = Mathf.Clamp01(t);
                    _tapToPlayText.color = new Color(initialColor.r, initialColor.g, initialColor.b, Mathf.Lerp(initialColor.a, 0.0f, t));
                    yield return null;
                }

                t = 0.0f;
                while (t < 1.0f)
                {
                    t += FadeSpeed * Time.deltaTime;
                    t = Mathf.Clamp01(t);
                    _tapToPlayText.color = new Color(initialColor.r, initialColor.g, initialColor.b, Mathf.Lerp(0.0f, initialColor.a, t));
                    yield return null;
                }
            }
        }
    }
}
