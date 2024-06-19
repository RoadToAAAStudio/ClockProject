using System;
using System.Collections.Generic;
using UnityEngine;

namespace RoadToAAA.ProjectClock.Utilities
{
    /*
     * This class implements the Pub/Sub design pattern
     * An event (enum) can be published or a listener (with void <name-method>(EventPayload) signature) can be subscribed or unsubscribed
     * If an event takes no parameter EventNullPayload should be used
     */
    public class EventManager
    {
        private static EventManager _instance;
        public static EventManager Instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = new EventManager();
                }
                return _instance;
            }
        }

        private Dictionary<EEventType, Action> _listeners = new Dictionary<EEventType, Action>();

        private EventManager() { }

        public void Subscribe(EEventType eventName, Action listener)
        {
            Debug.Assert(listener != null, "An empty delegate can't be subscribed");

            if (_listeners.ContainsKey(eventName))
            {
                _listeners[eventName] += listener;
            }
            else
            {
                _listeners[eventName] = listener;
            }
        }

        public void Unsubscribe(EEventType eventName, Action listener)
        {
            Debug.Assert(listener != null, "An empty delegate can't be unsubscribed");

            if (!_listeners.ContainsKey(eventName)) return;

            _listeners[eventName] -= listener;

            if (_listeners[eventName] != null) return;
            
            _listeners.Remove(eventName);
        }

        public void Publish(EEventType eventName)
        {
#if UNITY_EDITOR
            Debug.Log(string.Format("<color=silver>EventManager: {0}</color>", eventName));
#endif
            if (!_listeners.ContainsKey(eventName)) return;
                
            _listeners[eventName]?.Invoke();
        }

        public override string ToString()
        {
            string text = string.Empty;

            foreach (EEventType e in _listeners.Keys)
            {
                text += "Event registred: " + e + "\tListeners: " + _listeners[e]?.GetInvocationList().Length + Environment.NewLine;
            }

            return text;
        }
    }

    public class EventManager<T>
    {
        private static EventManager<T> _instance;
        public static EventManager<T> Instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = new EventManager<T>();
                }
                return _instance;
            }
        }

        private Dictionary<EEventType, Action<T>> _listeners = new Dictionary<EEventType, Action<T>>();

        private EventManager() { }

        public void Subscribe(EEventType eventName, Action<T> listener)
        {
            Debug.Assert(listener != null, "An empty delegate can't be subscribed");

            if (_listeners.ContainsKey(eventName))
            {
                _listeners[eventName] += listener;
            }
            else
            {
                _listeners[eventName] = listener;
            }
        }

        public void Unsubscribe(EEventType eventName, Action<T> listener)
        {
            Debug.Assert(listener != null, "An empty delegate can't be unsubscribed");

            if (!_listeners.ContainsKey(eventName)) return;

            _listeners[eventName] -= listener;

            if (_listeners[eventName] != null) return;

            _listeners.Remove(eventName);
        }

        public void Publish(EEventType eventName, T param)
        {
#if UNITY_EDITOR
            Debug.Log(string.Format("<color=silver>EventManager: {0} param({1})</color>", eventName, param));
#endif
            if (!_listeners.ContainsKey(eventName)) return;

            _listeners[eventName]?.Invoke(param);
        }

        public override string ToString()
        {
            string text = string.Empty;

            foreach (EEventType e in _listeners.Keys)
            {
                text += "Event registred: " + e + "\tListeners: " + _listeners[e]?.GetInvocationList().Length + Environment.NewLine;
            }

            return text;
        }
    }

    public class EventManager<T1, T2>
    {
        private static EventManager<T1, T2> _instance;
        public static EventManager<T1, T2> Instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = new EventManager<T1, T2>();
                }
                return _instance;
            }
        }

        private Dictionary<EEventType, Action<T1, T2>> _listeners = new Dictionary<EEventType, Action<T1, T2>>();

        private EventManager() { }

        public void Subscribe(EEventType eventName, Action<T1, T2> listener)
        {
            Debug.Assert(listener != null, "An empty delegate can't be subscribed");

            if (_listeners.ContainsKey(eventName))
            {
                _listeners[eventName] += listener;
            }
            else
            {
                _listeners[eventName] = listener;
            }
        }

        public void Unsubscribe(EEventType eventName, Action<T1, T2> listener)
        {
            Debug.Assert(listener != null, "An empty delegate can't be unsubscribed");

            if (!_listeners.ContainsKey(eventName)) return;

            _listeners[eventName] -= listener;

            if (_listeners[eventName] != null) return;

            _listeners.Remove(eventName);
        }

        public void Publish(EEventType eventName, T1 param1, T2 param2)
        {
#if UNITY_EDITOR
            Debug.Log(string.Format("<color=silver>EventManager: {0} param1({1}) param2 ({2})</color>", eventName, param1, param2));
#endif
            if (!_listeners.ContainsKey(eventName)) return;

            _listeners[eventName]?.Invoke(param1, param2);
        }

        public override string ToString()
        {
            string text = string.Empty;

            foreach (EEventType e in _listeners.Keys)
            {
                text += "Event registred: " + e + "\tListeners: " + _listeners[e]?.GetInvocationList().Length + Environment.NewLine;
            }

            return text;
        }
    }
    public enum EEventType
    {
        #region CoreSystemEvents
        OnBootSystemsLoaded,
        #endregion

        #region GameloopEvents
        OnGameStateChanged,
        #endregion

        #region InputEvents
        OnPlayTap,
        #endregion

        #region GameplayEvents
        OnCheckerResult,
        OnNewClockSelected,
        #endregion

        #region UIEvents
        OnPlayButtonPressed,
        OnShopButtonPressed,
        OnLeaderboardButtonPressed,
        OnAudioButtonPressed,
        OnReturnButtonPressed,
        OnLeaderboardReturnButtonPressed,
        OnRetryButtonPressed
        #endregion
    }
}