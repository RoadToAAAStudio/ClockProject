using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SfxController : MonoBehaviour
{

    public List<AudioClip> clipList;
    private AudioSource source;

    private void OnEnable()
    {
        EventManagerOneParam<GameObject>.Instance.StartListening("onNewClock", GoodTap);
    }

    private void OnDisable()
    {
        EventManagerOneParam<GameObject>.Instance.StopListening("onNewClock", GoodTap);
    }

    private void Awake()
    {
        source = GetComponent<AudioSource>();
    }

    public void Play(AudioClip _clip)
    {
        source.clip = _clip;
        source.Play();
    }

    public void Pause()
    {
        source.Pause();
    }

    public void Resume()
    {
        source.UnPause();
    }

    public void Stop(AudioClip _clip)
    {
        source.clip = _clip;
        source.Stop();
    }

    public void ToggleLoop(bool _toggle)
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


    private void GoodTap(GameObject gameObject)
    {
        Play(clipList[0]);
    }
}
