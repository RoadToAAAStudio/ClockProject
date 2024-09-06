using RoadToAAA.ProjectClock.Core;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class DebugAds : MonoBehaviour
{
    [SerializeField] private TMP_Text _eventsDebug;

    private void OnEnable()
    {
        EventManager.Instance.Subscribe(EEventType.OnAdLoaded, AdLoaded);
        EventManager.Instance.Subscribe(EEventType.OnAdCompleted, AdCompleted);
        EventManager.Instance.Subscribe(EEventType.OnAdRewardApplied, AdRewardApplied);
    }

    private void AdLoaded()
    {
        _eventsDebug.text = "AdLoaded!";
    }

    private void AdCompleted()
    {
        _eventsDebug.text = "AdCompleted!";
    }

    private void AdRewardApplied()
    {
        _eventsDebug.text = "AdRewardApplied!";
    }
}
