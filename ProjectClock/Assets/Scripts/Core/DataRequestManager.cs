using System;
using System.Collections.Generic;
using UnityEngine;

namespace RoadToAAA.ProjectClock.Core
{
    /*
     * This class decouples the demandeur of a piece of data from the provider
     * There should be only 1 provider per request
     */
    public class DataRequestManager<T>
    {
        private static DataRequestManager<T> _instance;
        public static DataRequestManager<T> Instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = new DataRequestManager<T>();
                }
                return _instance;
            }
        }

        private Dictionary<RequestType, Func<T>> _providers = new Dictionary<RequestType, Func<T>>();

        private DataRequestManager() { }

        public void Subscribe(RequestType requestName, Func<T> provider)
        {
            Debug.Assert(provider != null, "An empty delegate can't be subscribed");

            _providers[requestName] = provider;
        }

        public void Unsubscribe(RequestType requestName, Func<T> provider)
        {
            Debug.Assert(provider != null, "An empty delegate can't be unsubscribed");

            _providers.Remove(requestName);
        }

        public T Request(RequestType requestName, T defaultValue) 
        {
            if (!_providers.ContainsKey(requestName)) return defaultValue;

            if (_providers[requestName] != null)
            {
                return _providers[requestName].Invoke();
            }
            else
            {
                return defaultValue;
            }
        }

        public override string ToString()
        {
            string text = string.Empty;

            foreach (RequestType r in _providers.Keys)
            {
                text += "Request registred: " + r + "\tProvider: " + _providers[r]?.GetInvocationList().Length + Environment.NewLine;
            }

            return text;
        }
    }

    public enum RequestType
    {

    }
}
