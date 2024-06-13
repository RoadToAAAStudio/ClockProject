using System;
using System.Collections.Generic;
using UnityEngine;

namespace RoadToAAA.ProjectClock.Utilities
{
    /*
     * This manager handles all timers in the game
     */
    public class TimersManager : PersistentSingleton<TimersManager>
    {
        private List<Timer> _timers = new List<Timer>();

        #region Unity_Messages

        private void Update()
        {
            for (int i = 0; i < _timers.Count; i++) 
            { 
                Timer timer = _timers[i];

                if (timer.State != ETimerState.Running)
                {
                    continue;
                }

                timer.Tick(Time.deltaTime);
            }
        }

        #endregion

        public void Subscribe(Timer timer)
        {
            _timers.Add(timer);
        }

        public void Unsubscribe(Timer timer) 
        { 
            _timers.Remove(timer);
        }
    }

    /*
     * Class of a timer
     * Please when the holder of the instance is destroyed the Timer should be unsubscribed (if it is) from the manager (calling Stop) 
     * otherwise it will leak in the manager
     */
    public class Timer
    {
        public float MaxTimeToWait { get; private set; } = -1f;
        public float TimeLeft { get; private set; } = -1f;
        public ETimerState State { get; private set; } = ETimerState.Stopped;
        private Action _callBack = null;

        public void Start(float amount, Action callback)
        {
            if (State == ETimerState.Stopped || State == ETimerState.Finished)
            {
                MaxTimeToWait = amount;
                TimeLeft = amount;
                State = ETimerState.Running;
                _callBack = callback;

                TimersManager.Instance.Subscribe(this);
            }
        }

        public void Stop()
        {
            if (State == ETimerState.Running || State == ETimerState.Paused) 
            {
                MaxTimeToWait = -1f;
                TimeLeft = -1f;
                State = ETimerState.Stopped;
                _callBack = null;

                TimersManager.Instance.Unsubscribe(this);
            }
        }

        public void Restart()
        {
            if (State == ETimerState.Running || State == ETimerState.Paused)
            {
                TimeLeft = MaxTimeToWait;

                if (State == ETimerState.Paused)
                {
                    State = ETimerState.Running;
                    TimersManager.Instance.Subscribe(this);
                }
            }
        }

        public void Pause()
        {
            if (State == ETimerState.Running)
            {
                State = ETimerState.Paused;
            }
        }

        public void Unpause()
        {
            if (State == ETimerState.Paused)
            {
                State = ETimerState.Running;
            }
        }

        public void Tick(float delta)
        {
            TimeLeft -= delta;

            if (TimeLeft <= 0)
            {
                MaxTimeToWait = -1f;
                TimeLeft = -1f;
                State = ETimerState.Finished;
                TimersManager.Instance.Unsubscribe(this);
                _callBack?.Invoke();
            }
        }

        public override string ToString()
        {
            string text = string.Empty;

            text += string.Format("MaxTime: {0}, TimeLeft: {1}, State: {2}", MaxTimeToWait, TimeLeft, State); 

            return text;
        }
    }

    // Stopped  -> (Start)          -> Running
    // Running  -> (Stop)           -> Stopped
    //          -> (Restart)        -> Running
    //          -> (Pause)          -> Paused
    //          -> (time elapsed)   -> Finished
    // Paused   -> (Unpause)        -> Running
    //          -> (Restart)        -> Running
    //          -> (Stop)           -> Stopped
    // Finished -> (Start)          -> Running

    public enum ETimerState
    {
        Running,
        Paused,
        Stopped,
        Finished
    }
}
