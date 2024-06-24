using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RoadToAAA.ProjectClock.Core
{
    /*
     * Holds and handles all the data relative to the player
     */

    public class PlayerData : Singleton<PlayerData>
    {
        private int _score;
        public int Score
        {
            get { return _score; }
            set { _score = value; }
        }

        private int _bestScore;
        public int BestScore
        {
            get { return _bestScore; }
            set { _bestScore = value; }
        }

        private int _currency;
        public int Currency
        {
            get { return _currency; }
            set { _currency = value; }
        }

        protected override void Awake()
        {
            base.Awake();
            _score = 0;
        }

        private void Start()
        {
            
        }
    }
}

