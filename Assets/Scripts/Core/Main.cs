using Systems;
using AirCoder.NaughtyAttributes.Scripts.Core.DrawerAttributes;
using AirCoder.NaughtyAttributes.Scripts.Core.DrawerAttributes_SpecialCase;
using AirCoder.NaughtyAttributes.Scripts.Core.MetaAttributes;
using AirCoder.NaughtyAttributes.Scripts.Core.ValidatorAttributes;
using UnityEngine;

namespace Core
{
    public class Main : MonoBehaviour
    {
        [BoxGroup("System Config")][Required][SerializeField] [Expandable] private SystemConfig gridConfig;

        private GameController _controller;
        private static Main _instance;
        
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
            UnityEngine.Application.targetFrameRate = 60;
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
                .AddSystem(new GridSystem(gridConfig));
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
    }
}