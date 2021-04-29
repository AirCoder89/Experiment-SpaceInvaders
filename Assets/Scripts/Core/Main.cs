
using System;
using Systems;
using AirCoder.NaughtyAttributes.Scripts.Core.DrawerAttributes;
using AirCoder.NaughtyAttributes.Scripts.Core.DrawerAttributes_SpecialCase;
using AirCoder.NaughtyAttributes.Scripts.Core.MetaAttributes;
using AirCoder.NaughtyAttributes.Scripts.Core.ValidatorAttributes;
using UnityEngine;
using Utils.Array2D;
using Vector2Int = Utils.Array2D.Vector2Int;

namespace Core
{
    public class Main : MonoBehaviour
    {
        public static GameSettings Settings => _instance.settings;
        [BoxGroup("Settings")][Required][SerializeField][Expandable] private GameSettings settings;
        
        [BoxGroup("System Config")][Required][SerializeField] [Expandable] private SystemConfig levelConfig;
        [BoxGroup("System Config")][Required][SerializeField] [Expandable] private SystemConfig playerConfig;
        [BoxGroup("System Config")][Required][SerializeField] [Expandable] private SystemConfig gridConfig;
        [BoxGroup("System Config")][Required][SerializeField] [Expandable] private SystemConfig shieldsConfig;
        [BoxGroup("System Config")][Required][SerializeField] [Expandable] private SystemConfig invadersConfig;
        [BoxGroup("System Config")][Required][SerializeField] [Expandable] private SystemConfig shootingConfig;

        private GameController _controller;
        private static Main    _instance;
        private GameStates     _currentState;

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
            InitializeHelpers();
            ApplyGameSettings();
            Initialize();
            
            StartGame();
        }
        
        private void InitializeHelpers()
        {
            // --------------------------------------------------------------------------------
            // Create and Initialize Helper classes
            // --------------------------------------------------------------------------------
            //ViewsContainer = new ViewsContainer(this);
            //ObjectMap.Initialize(this);
        }
        
        private void ApplyGameSettings()
        {
            // --------------------------------------------------------------------------------
            // Setup game settings
            // --------------------------------------------------------------------------------
            Screen.sleepTimeout = SleepTimeout.NeverSleep;
            //UnityEngine.Application.targetFrameRate = 60;
            //Cursor.visible = false;
            
            if (Screen.currentResolution.refreshRate > 65) {
                QualitySettings.vSyncCount = Mathf.FloorToInt(Screen.currentResolution.refreshRate/60f);
            }
        }
        
        private void Initialize()
        {
            // --------------------------------------------------------------------------------
            // Create the GameController and add the necessary systems (the order is matter !)
            // --------------------------------------------------------------------------------
            _controller = new GameController();
            _controller
                .AddSystem(new TimingSystem())
                .AddSystem(new LevelSystem(levelConfig))
                .AddSystem(new PlayerSystem(playerConfig))
                .AddSystem(new GridSystem(gridConfig))
                .AddSystem(new ShieldsSystem(shieldsConfig))
                .AddSystem(new InvadersSystem(invadersConfig))
                .AddSystem(new ShootingSystem(shootingConfig));
        }
        
        public static T GetSystem<T>() where T : GameSystem
        => _instance._controller.GetSystem<T>();
        
        [Button("Start Game")]
        private void StartGame()
        {
            _controller.Start();
        }
        
        [Button("Pause App")]
        private void PauseGame()
        {
            _controller.Pause();
        }
        
        [Button("Resume App")]
        private void ResumeGame()
        {
            _controller.Resume();
        }
        
        private void Update()
        {
            _controller.Tick(Time.deltaTime);
        }

        private void FixedUpdate()
        {
            _controller.FixedTick(Time.fixedDeltaTime);
        }
    }
}