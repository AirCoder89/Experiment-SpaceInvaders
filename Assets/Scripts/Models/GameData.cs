using System;
using System.Collections.Generic;
using Models.Database;
using UnityEngine;

namespace Models
{
    [CreateAssetMenu(menuName = "Game/GameData")]
    public class GameData : ScriptableObject
    {
        public static event Action OnDataChanged;

        public RegisterData    Register { get; set; }
        public LeaderBoardData Leaderboards { get; set; }

        public float ElapsedTime
        {
            get => _elapsedTime;
            set
            {
                _elapsedTime = value;
                OnDataChanged?.Invoke();
            }
        }

        public int Lives
        {
            get => _lives;
            set
            {
                _lives = value;
                OnDataChanged?.Invoke();
            }
        }
        
        public int Score
        {
            get => _score;
            set
            {
                _score = value;
                OnDataChanged?.Invoke();
            }
        }

        private float  _elapsedTime;
        private int    _startLives;
        private int    _lives;
        private int    _score;
        
        public void Initialize(int inLives)
        {
            _startLives = _lives = inLives;
        }
        
        public void ResetData()
        {
            Register = null;
            _lives = _startLives;
            _score = 0;
            _elapsedTime = 0f;
            OnDataChanged?.Invoke();
        }
    }
}