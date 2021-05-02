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

        public RegisterData Register { get; set; }
        public LeaderBoardData Leaderboards { get; set; }

        private float _elapsedTime;
        public float ElapsedTime
        {
            get => _elapsedTime;
            set
            {
                _elapsedTime = value;
                OnDataChanged?.Invoke();
            }
        }

        private int _lives;
        public int Lives
        {
            get => _lives;
            set
            {
                _lives = value;
                OnDataChanged?.Invoke();
            }
        }
        
        private int _score;
        public int Score
        {
            get => _score;
            set
            {
                _score = value;
                OnDataChanged?.Invoke();
            }
        }

        private int _startLives;
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