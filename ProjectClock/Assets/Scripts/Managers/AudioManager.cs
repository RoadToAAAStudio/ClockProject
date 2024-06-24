using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RoadToAAA.ProjectClock.Core;
using RoadToAAA.ProjectClock.Scriptables;

namespace RoadToAAA.ProjectClock.Managers
{

    public class AudioManager : MonoBehaviour
    {
        public List<AudioClip> musicList;
        public List<AudioClip> sfxList;
        private AudioSource musicSource;
        private AudioSource sfxSource;

        private void OnEnable()
        {
            EventManager<ECheckResult, ComboResult>.Instance.Subscribe(EEventType.OnCheckerResult, GoodTap);
        }

        private void OnDisable()
        {
            EventManager<ECheckResult, ComboResult>.Instance.Unsubscribe(EEventType.OnCheckerResult, GoodTap);
        }

        private void Awake()
        {
            musicSource = GetComponents<AudioSource>()[0];
            sfxSource = GetComponents<AudioSource>()[1];
        }

        private void Start()
        {
            musicSource.loop = true;
            PlayMusic(musicList[0]);
        }

        public void PlayMusic(AudioClip clip)
        {
            musicSource.clip = clip;
            musicSource.Play();
        }

        public void PlaySfx(AudioClip clip)
        {

            sfxSource.clip = clip;
            sfxSource.pitch = Random.Range(0.95f, 1.05f);
            sfxSource.Play();
        }

        public void Pause(AudioSource source)
        {
            source.Pause();
        }

        public void Resume(AudioSource source)
        {
            source.UnPause();
        }

        public void Stop(AudioSource source, AudioClip _clip)
        {
            source.clip = _clip;
            source.Stop();
        }

        public void ToggleLoop(AudioSource source,bool _toggle)
        {
            if (_toggle == true)
            {
                source.loop = true;
            }
            else
            {
                source.loop = false;
            }
        }


        private void GoodTap(ECheckResult checkResult, ComboResult comboResult)
        {
            if (checkResult != ECheckResult.Unsuccess)
            {
                PlaySfx(sfxList[0]);
            }
        }
    }
}
