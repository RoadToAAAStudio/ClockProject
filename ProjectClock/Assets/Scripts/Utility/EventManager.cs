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

        private Dictionary<EventType, Action> _listeners = new Dictionary<EventType, Action>();

        private EventManager() { }

        public void Subscribe(EventType eventName, Action listener)
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

        public void Unsubscribe(EventType eventName, Action listener)
        {
            Debug.Assert(listener != null, "An empty delegate can't be unsubscribed");

            if (!_listeners.ContainsKey(eventName)) return;

            _listeners[eventName] -= listener;

            if (_listeners[eventName] != null) return;
            
            _listeners.Remove(eventName);
        }

        public void Publish(EventType eventName)
        {
            if (!_listeners.ContainsKey(eventName)) return;
                
            _listeners[eventName]?.Invoke();
        }

        public override string ToString()
        {
            string text = string.Empty;

            foreach (EventType e in _listeners.Keys)
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

        private Dictionary<EventType, Action<T>> _listeners = new Dictionary<EventType, Action<T>>();

        private EventManager() { }

        public void Subscribe(EventType eventName, Action<T> listener)
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

        public void Unsubscribe(EventType eventName, Action<T> listener)
        {
            Debug.Assert(listener != null, "An empty delegate can't be unsubscribed");

            if (!_listeners.ContainsKey(eventName)) return;

            _listeners[eventName] -= listener;

            if (_listeners[eventName] != null) return;

            _listeners.Remove(eventName);
        }

        public void Publish(EventType eventName, T param)
        {
            if (!_listeners.ContainsKey(eventName)) return;

            _listeners[eventName]?.Invoke(param);
        }

        public override string ToString()
        {
            string text = string.Empty;

            foreach (EventType e in _listeners.Keys)
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

        private Dictionary<EventType, Action<T1, T2>> _listeners = new Dictionary<EventType, Action<T1, T2>>();

        private EventManager()
        {
        }

        public void Subscribe(EventType eventName, Action<T1, T2> listener)
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

        public void Unsubscribe(EventType eventName, Action<T1, T2> listener)
        {
            Debug.Assert(listener != null, "An empty delegate can't be unsubscribed");

            if (!_listeners.ContainsKey(eventName)) return;

            _listeners[eventName] -= listener;

            if (_listeners[eventName] != null) return;

            _listeners.Remove(eventName);
        }

        public void Publish(EventType eventName, T1 param1, T2 param2)
        {
            if (!_listeners.ContainsKey(eventName)) return;

            _listeners[eventName]?.Invoke(param1, param2);
        }

        public override string ToString()
        {
            string text = string.Empty;

            foreach (EventType e in _listeners.Keys)
            {
                text += "Event registred: " + e + "\tListeners: " + _listeners[e]?.GetInvocationList().Length + Environment.NewLine;
            }

            return text;
        }
    }
    public enum EventType
    {
        #region GameloopEvents
        OnGameStateChanged,
        #endregion
        #region InputsEvents
        OnPlayTap,
        OnMenuTap
        #endregion
    }
}