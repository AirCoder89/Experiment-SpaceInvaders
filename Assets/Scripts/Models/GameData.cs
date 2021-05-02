using System;
using UnityEngine;

namespace Models
{
    [CreateAssetMenu(menuName = "Game/GameData")]
    public class GameData : ScriptableObject
    {
        public static event Action OnDataChanged;

        private float _elapsedTime;
        public float elapsedTime
        {
            get => _elapsedTime;
            set
            {
                _elapsedTime = value;
                OnDataChanged?.Invoke();
            }
        }

        private int _lives;
        public int lives
        {
            get => _lives;
            set
            {
                _lives = value;
                OnDataChanged?.Invoke();
            }
        }
        
        private int _score;
        public int score
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
            _lives = _startLives;
            _score = 0;
            _elapsedTime = 0f;
            OnDataChanged?.Invoke();
        }
    }
}