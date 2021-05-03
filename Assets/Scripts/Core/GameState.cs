using System.Collections.Generic;
using System;
using Systems;
using Interfaces;
using Models;
using UI.HUD;
using UnityEngine;
using Views;
using Object = UnityEngine.Object;

namespace Core
{
    public sealed class GameState : IGameState
    {
        public static event Action<int>  OnLevelWin;
        public static event Action       OnGameOver;
        public static event Action       OnResetGame;
        
        public static Transform GameHolder { get; private set; }
        public States           Label { get; }
        
        public bool Visibility
        {
            get => _isVisible;
            private set
            {
                _isVisible = value;
                if(_isVisible) Resume();
                else Pause();
                if(StateManager.NextState == States.Pause) return;
                    GameHolder.gameObject.SetActive(_isVisible);
                    HUDCanvas.instance.RootCanvas.enabled = _isVisible;
            }
        }
        
        private readonly Dictionary<Type, GameSystem> _systems;
        private HashSet<ITick>                        _tickSystems = null;
        private GameData                              _gameData;
        private bool                                  _isRun;
        private bool                                  _isVisible;
        
        public GameState(States inLabel, GameData inData)
        {
            _systems = new Dictionary<Type, GameSystem>();
            _tickSystems = null;
            Label = inLabel;
            _gameData = inData;
            
            //- create the root game parent
            CreateRootGameHolder();
            
            //- check level is completed after each invader getting destroyed!
            InvaderView.OnDestroyed += view =>
            {
                if (!Main.GetSystem<GridSystem>().IsLevelWin()) return;
                OnLevelWin?.Invoke(Main.Data.Score);
            };

           // PlayerView.OnDestroyed += OnPlayerDestroyed;
            PlayerView.OnReviveRequest += RevivePlayer;
            InvadersSystem.OnCollectScore += UpdateScore;
            
            //- register state
            StateManager.RegisterGameState(this);
            Visibility = false;
        }

        private void RevivePlayer()
        {
            if (_gameData.Lives > 0)
            {
                _gameData.Lives--;
                GetSystem<PlayerSystem>().Reset();
            }
            else OnGameOver?.Invoke();
        }

        private void CreateRootGameHolder()
        {
            GameHolder = new GameObject("GameHolder").transform;
            GameHolder.localScale = Vector3.one;
            GameHolder.SetPositionAndRotation(Vector3.zero, Quaternion.identity);
        }
        
        private void UpdateScore(int inScore)
        {
            _gameData.Score += inScore;
        }

        public void Enter()
        {
            Visibility = true;
        }

        public void Exit()
        {
            Visibility = false;
        }
        
        public GameState AddSystem(GameSystem inSystem)
        {
            if(_systems.ContainsKey(inSystem.GetType())) throw new Exception($"Cannot duplicate game systems");
            _systems.Add(inSystem.GetType(), inSystem);
            if (inSystem is ITick systemTick) RegisterTickSystem(systemTick);
            return this;
        }
        
        public T GetSystem<T>() where T : GameSystem
        {
            if (!_systems.ContainsKey(typeof(T))) return null;
            return (T) _systems[typeof(T)];
        }
    
        public void Tick(float inDeltaTime)
        {
            if(!_isRun || _tickSystems == null) return;
            foreach (var system in _tickSystems)
                system.Tick(inDeltaTime);
        }

        public void NewGame()
        {
            _gameData.ResetData();
            
            OnResetGame?.Invoke();
            GetSystem<GridSystem>().NotReady(); //to stop invaders
            var invadersCallback = GetSystem<InvadersSystem>().Reset();
            GetSystem<GridSystem>().Reset();
            GetSystem<ShieldsSystem>().Reset();
            GetSystem<PlayerSystem>().Reset();
            invadersCallback?.Invoke();
        }

        public void Start()
        {
            _isRun = true;
            foreach (var system in _systems.Values)
                system.Start();
        }
    
        public void Pause()
        {
            foreach (var system in _systems.Values)
                system.Pause();
        }
        
        public void Resume()
        {
            foreach (var system in _systems.Values)
                system.Resume();
        }
        
        private void RegisterTickSystem(ITick inSystem)
        {
            if(_tickSystems == null) _tickSystems = new HashSet<ITick>();
            _tickSystems.Add(inSystem);
        }
    }
}