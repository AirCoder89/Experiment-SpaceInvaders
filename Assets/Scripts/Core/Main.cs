
using System;
using System.Collections.Generic;
using Systems;
using AirCoder.NaughtyAttributes.Scripts.Core.DrawerAttributes;
using AirCoder.NaughtyAttributes.Scripts.Core.DrawerAttributes_SpecialCase;
using AirCoder.NaughtyAttributes.Scripts.Core.MetaAttributes;
using AirCoder.NaughtyAttributes.Scripts.Core.ValidatorAttributes;
using Interfaces;
using UI.Core;
using UnityEngine;
using Utils.Array2D;
using Views;
using Vector2Int = Utils.Array2D.Vector2Int;

namespace Core
{
    public class Main : MonoBehaviour
    {
        public static event Action OnLevelWin;
        
        [Required] [SerializeField] private UIManager uiManager;
        
        public static GameSettings Settings => _instance.settings;
        [BoxGroup("Settings")][Required][SerializeField][Expandable] private GameSettings settings;

        [BoxGroup("System Config")][Required][SerializeField] [Expandable] private SystemConfig levelConfig;
        [BoxGroup("System Config")][Required][SerializeField] [Expandable] private SystemConfig playerConfig;
        [BoxGroup("System Config")][Required][SerializeField] [Expandable] private SystemConfig gridConfig;
        [BoxGroup("System Config")][Required][SerializeField] [Expandable] private SystemConfig shieldsConfig;
        [BoxGroup("System Config")][Required][SerializeField] [Expandable] private SystemConfig invadersConfig;
        [BoxGroup("System Config")][Required][SerializeField] [Expandable] private SystemConfig shootingConfig;
        [BoxGroup("System Config")][Required][SerializeField] [Expandable] private SystemConfig animationConfig;
        [BoxGroup("System Config")][Required][SerializeField] [Expandable] private SystemConfig audioConfig;

        private Dictionary<GameStates, IGameState> _gameStates;
        private GameController   _controller;
        private IGameState       _activeState;
        private static Main      _instance;
        private bool             _isRun;
        private void Awake()
        {
            if (_instance != null)  return;
            _instance = this;
        }
        
        private void Start()
        {
            // --------------------------------------------------------------------------------
            // Game Entry Point
            // --------------------------------------------------------------------------------
            ApplyGameSettings();
            Initialize();

            //- we make a check if level is completed or not after invader getting destroyed!
            InvaderView.OnDestroyed += view =>
            {
                if (!GetSystem<GridSystem>().IsLevelWin()) return;
                LevelWin();
            };

            ShowGameState(Settings.startState);
            StartGame();
        }

        private void ApplyGameSettings()
        {
            // --------------------------------------------------------------------------------
            // Setup game settings
            // --------------------------------------------------------------------------------
            Screen.sleepTimeout = SleepTimeout.NeverSleep;
            UnityEngine.Application.targetFrameRate = Settings.targetFrameRate;
            Cursor.visible = Settings.cursorVisibility;
            
            //keep frame rate & monitor refresh rate in sync
            if(!Settings.vSync) return;
            if (Screen.currentResolution.refreshRate > 65) {
                QualitySettings.vSyncCount = Mathf.FloorToInt(Screen.currentResolution.refreshRate/60f);
            }
        }
        
        private void Initialize()
        {
            uiManager.Initialize();
            // --------------------------------------------------------------------------------
            // Create the GameController then add the necessary systems (the order is matter !)
            // --------------------------------------------------------------------------------
            _controller = new GameController();
            _controller
                .AddSystem(new TimingSystem())
                .AddSystem(new LevelSystem(levelConfig))
                .AddSystem(new AudioSystem(audioConfig))
                .AddSystem(new PlayerSystem(playerConfig))
                .AddSystem(new GridSystem(gridConfig))
                .AddSystem(new ShieldsSystem(shieldsConfig))
                .AddSystem(new InvadersSystem(invadersConfig))
                .AddSystem(new AnimationSystem(animationConfig))
                .AddSystem(new ShootingSystem(shootingConfig));
        }

        private void LevelWin()
        {
            Debug.Log($"LEVEL WIN !");
        }

        public static void RegisterGameState(IGameState inGameSate)
        {
            if (_instance._gameStates == null) _instance._gameStates = new Dictionary<GameStates, IGameState>();
            if (_instance._gameStates.ContainsKey(inGameSate.Label))
            {
                Debug.LogWarning($"Cannot register duplicated Game State!");
                return;
            }
            _instance._gameStates.Add(inGameSate.Label, inGameSate);
        }

        public GameStates targetState;

        [Button]
        private void SetState()
        {
            ShowGameState(targetState);
        }
        
        public void ShowGameState(GameStates inState)
        {
            if (!_gameStates.ContainsKey(inState))
                throw new Exception($"Game State [{inState}] not found !");

            if (_activeState != null)
            {
                if (_activeState == _gameStates[inState]) return;
                _activeState.Visibility = false;
            }

            _activeState = _gameStates[inState];
            _activeState.Visibility = true;
        }
        
        
        public static T GetSystem<T>() where T : GameSystem
        => _instance._controller.GetSystem<T>();

        [Button("Start Game")]
        private void StartGame()
        {
            _isRun = true;
            _controller.Start();
        }
        
        [Button("Pause App")]
        private void PauseGame()
        {
            _isRun = false;
            _controller.Pause();
        }
        
        [Button("Resume App")]
        private void ResumeGame()
        {
            _isRun = true;
            _controller.Resume();
        }
        
        private void Update()
        {
            if(!_isRun) return;
            _controller.Tick(Time.deltaTime);
        }

        private void FixedUpdate()
        {
            if(!_isRun) return;
            _controller.FixedTick(Time.fixedDeltaTime);
        }
    }
}