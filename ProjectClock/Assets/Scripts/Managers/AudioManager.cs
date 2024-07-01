using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RoadToAAA.ProjectClock.Core;
using RoadToAAA.ProjectClock.Scriptables;
using UnityEngine.Audio;

namespace RoadToAAA.ProjectClock.Managers
{

    public class AudioManager : MonoBehaviour
    {
        [SerializeField] private List<AudioClip> _musicClips;
        [SerializeField] private List<AudioClip> _sfxClips;
        [SerializeField] private AudioMixer _mixer;

        private AudioSource _musicSource;
        private AudioSource _sfxSource;

        private Dictionary<AudioSource, List<IEnumerator>> _audioSourceEnvelops;
        private List<IEnumerator> _musicEnvelops;

        private const string MIXER_MASTER = "MasterVolume";

        private const float UNISON_PITCH = 1.0f;
        private const float SEMITONE_PITCH = 1.059f;
        private const float WHOLE_TONE_PITCH = 1.122f;
        private const float MINOR_THIRD_PITCH = 1.189f;
        private const float MAJOR_THIRD_PITCH = 1.260f;
        private const float PERFECT_FOURTH_PITCH = 1.335f;
        private const float TRITONE_PITCH = 1.414f;
        private const float PERFECT_FIFTH_PITCH = 1.498f;
        private const float MINOR_SIXTH_PITCH = 1.587f;
        private const float MAJOR_SIXTH_PITCH = 1.682f;
        private const float MINOR_SEVENTH_PITCH = 1.782f;
        private const float MAJOR_SEVENTH_PITCH = 1.888f;
        private const float OCTAVE_PITCH = 2.000f;

        private float[] PITCHES = 
        {
            UNISON_PITCH,
            SEMITONE_PITCH,
            WHOLE_TONE_PITCH,
            MINOR_THIRD_PITCH,
            MAJOR_THIRD_PITCH,
            PERFECT_FOURTH_PITCH,
            TRITONE_PITCH,
            PERFECT_FIFTH_PITCH,
            MINOR_SIXTH_PITCH,
            MAJOR_SIXTH_PITCH,
            MINOR_SEVENTH_PITCH,
            MAJOR_SEVENTH_PITCH,
            OCTAVE_PITCH
        };

        #region UnityMessages
        private void Awake()
        {
            _musicSource = GetComponents<AudioSource>()[0];
            _sfxSource = GetComponents<AudioSource>()[1];

            _audioSourceEnvelops = new Dictionary<AudioSource, List<IEnumerator>>();
            _audioSourceEnvelops[_musicSource] = new List<IEnumerator>();
            _audioSourceEnvelops[_sfxSource] = new List<IEnumerator>();
        }

        private void OnEnable()
        {
            EventManager<EGameState, EGameState>.Instance.Subscribe(EEventType.OnGameStateChanged, GameStateChanged);
            EventManager<ECheckResult, ComboResult>.Instance.Subscribe(EEventType.OnCheckerResult, CheckPeformed);
            EventManager<int>.Instance.Subscribe(EEventType.OnAudioButtonPressed, ToggleAudio);
        }

        private void OnDisable()
        {
            EventManager<EGameState, EGameState>.Instance.Unsubscribe(EEventType.OnGameStateChanged, GameStateChanged);
            EventManager<ECheckResult, ComboResult>.Instance.Unsubscribe(EEventType.OnCheckerResult, CheckPeformed);
            EventManager<int>.Instance.Unsubscribe(EEventType.OnAudioButtonPressed, ToggleAudio);
        }
        #endregion

        #region Controls
        public void Play(AudioSource audioSource, AudioClip clip, float volume, float pitch)
        {
            audioSource.clip = clip;
            audioSource.volume = volume;
            audioSource.pitch = pitch;
            audioSource.Play();
        }

        public void Pause(AudioSource source)
        {
            source.Pause();
        }

        public void Resume(AudioSource source)
        {
            source.UnPause();
        }

        public void Stop(AudioSource source)
        {
            source.Stop();
        }

        public void ToggleLoop(AudioSource source, bool toggle)
        {
            if (toggle == true)
            {
                source.loop = true;
            }
            else
            {
                source.loop = false;
            }
        }
        #endregion

        #region Envelops

        private IEnumerator FadeOut(AudioSource source, float startVolume, float duration)
        {
            Debug.Assert(source != null, "AudioSource is null!");
            Debug.Assert(startVolume > 0, "Start volume must be positive!");
            Debug.Assert(duration > 0, "Duration must be positive!");

            float startDuration = duration;
            while (duration > 0.0f)
            {
                duration -= Time.deltaTime;
                duration = Mathf.Clamp(duration, 0.0f, float.PositiveInfinity);

                source.volume = Mathf.Lerp(startVolume, 0.0f, 1 - (duration / startDuration));
                yield return null;
            }

            ClearEnvelops(source);
            source.volume = 0.0f;
            source.Stop();
        }

        private IEnumerator PitchLinearEnvelop(AudioSource source, float startPitch, float endPitch, float duration)
        {
            Debug.Assert(source != null, "AudioSource is null!");
            Debug.Assert(startPitch > 0, "Start pitch must be positive!");
            Debug.Assert(endPitch > 0, "End pitch must be positive!");
            Debug.Assert(duration > 0, "Duration must be positive!");

            float startDuration = duration;
            while (duration > 0.0f)
            {
                duration -= Time.deltaTime;
                duration = Mathf.Clamp(duration, 0.0f, float.PositiveInfinity);

                source.pitch = Mathf.Lerp(startPitch, endPitch, 1 - (duration / startDuration));
                yield return null;
            }
        }

        #endregion

        #region EventListeners
        private void GameStateChanged(EGameState oldState, EGameState newState)
        {
            switch (oldState)
            {
                case EGameState.Playing:
                    StartEnvelop(_musicSource, FadeOut(_musicSource, _musicSource.volume, 2.0f));
                    StartEnvelop(_musicSource, PitchLinearEnvelop(_musicSource, _musicSource.pitch, _musicSource.pitch / 3, 2.0f));
                    break;
            }

            switch (newState) 
            { 
                case EGameState.Playing:
                    ClearEnvelops(_musicSource);
                    Play(_musicSource, _musicClips[0], 1.0f, 1.0f); 
                    break;
            }
        }

        private void CheckPeformed(ECheckResult checkResult, ComboResult comboResult)
        {
            if (checkResult != ECheckResult.Unsuccess)
            {
                float[] pitches = { UNISON_PITCH, WHOLE_TONE_PITCH, MINOR_THIRD_PITCH, PERFECT_FIFTH_PITCH };
                int pitchIndex = comboResult.StateIndex % ConfigurationManager.Instance.ComboAsset.ComboStates.Length;

                Play(_sfxSource, _sfxClips[0], 1.0f, pitches[pitchIndex] + Random.Range(-0.02f, 0.02f));
            }
        }
        #endregion

        private void StartEnvelop(AudioSource audioSource, IEnumerator envelop)
        {
            _audioSourceEnvelops[audioSource].Add(envelop);
            StartCoroutine(envelop);
        }

        private void ClearEnvelops(AudioSource audioSource) 
        {
            List<IEnumerator> envelops = _audioSourceEnvelops[audioSource];

            for (int i = 0; i < envelops.Count; i++)
            {
                IEnumerator envelop = envelops[i];
                StopCoroutine(envelop);
            }

            envelops.Clear();
        }

        private void ToggleAudio(int toggleState)
        {
            if (toggleState == 1)
            {
                _mixer.SetFloat(MIXER_MASTER, Mathf.Log10(toggleState) * 20);
            }
            else
            {
                _mixer.SetFloat(MIXER_MASTER, Mathf.Log10(0.0001f) * 20);
            }
        }
    }
}
