using System;
using System.Collections.Generic;
using Interfaces;

namespace Core
{
    public static class StateManager
    {
        public static States               NextState { get; private set; }
        public static IGameState           PreviousState { get; private set; }
        public static IGameState           ActiveState { get; private set; }
        public static event Action<States> OnGameStateChanged; 
        
        private static Dictionary<States, IGameState> _gameStates;
        
        public static void RegisterGameState(IGameState inGameSate)
        {
            if (_gameStates == null) _gameStates = new Dictionary<States, IGameState>();
            if (_gameStates.ContainsKey(inGameSate.Label))
                throw new Exception($"Cannot register duplicated Game State!");
            
            _gameStates.Add(inGameSate.Label, inGameSate);
        }

        public static void Tick(float inDeltaTime)
            => ActiveState?.Tick(inDeltaTime);
        
        public static void UpdateGameState(States inState)
        {
            if (!_gameStates.ContainsKey(inState))
                throw new Exception($"Game State [{inState}] not found !");

            NextState = inState;
            if (ActiveState != null)
            {
                if (ActiveState.Label == inState) return;
                ActiveState.Exit();
                PreviousState = ActiveState;
            }

            //Ensure that Enter() will be called before the 1st tick!
            _gameStates[inState].Enter();
            ActiveState = _gameStates[inState];
            
            OnGameStateChanged?.Invoke(inState);
        }
    }
}