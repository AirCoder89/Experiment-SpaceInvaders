using System;
using AirCoder.NaughtyAttributes.Scripts.Core.DrawerAttributes_SpecialCase;
using AirCoder.NaughtyAttributes.Scripts.Core.ValidatorAttributes;
using AirCoder.NaughtyAttributes.Scripts.Core.DrawerAttributes;
using AirCoder.NaughtyAttributes.Scripts.Core.MetaAttributes;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Systems;
using Database;
using LocalLeaderboards;
using Models;
using Models.Database;
using Models.SystemConfigs;
using UI.Core;

namespace Core
{
    public enum GameLoop
    {
        None, Update, Coroutine
    }
    public class Main : MonoBehaviour
    {
        [Required] [SerializeField] private UIManager uiManager;
        
        public static GameSettings Settings => _instance.settings;
        public static GameData Data => _instance.gameData;
        
        [BoxGroup("Setup")][Required][SerializeField][Expandable] private GameSettings settings;
        [BoxGroup("Setup")][Required][SerializeField] private GameData                 gameData;

        [BoxGroup("System Config")][Required][SerializeField] [Expandable] private SystemConfig levelConfig;
        [BoxGroup("System Config")][Required][SerializeField] [Expandable] private SystemConfig playerConfig;
        [BoxGroup("System Config")][Required][SerializeField] [Expandable] private SystemConfig gridConfig;
        [BoxGroup("System Config")][Required][SerializeField] [Expandable] private SystemConfig shieldsConfig;
        [BoxGroup("System Config")][Required][SerializeField] [Expandable] private SystemConfig invadersConfig;
        [BoxGroup("System Config")][Required][SerializeField] [Expandable] private SystemConfig shootingConfig;
        [BoxGroup("System Config")][Required][SerializeField] [Expandable] private SystemConfig animationConfig;
        [BoxGroup("System Config")][Required][SerializeField] [Expandable] private SystemConfig audioConfig;

        private static Main      _instance;
        private GameState        _gameState;
        private Counter          _timeCounter;
        private bool             _isRun;
        private bool             _isFirstRun;
        
        private void Awake()
        {
            // --------------------------------------------------------------------------------
            // Game Entry Point
            // --------------------------------------------------------------------------------
            if (_instance != null)  return;
            _instance = this;
            _isFirstRun = true;
        }

        private void Start()
        {
            // --------------------------------------------------------------------------------
            // Initializing & Establishing
            // --------------------------------------------------------------------------------
            _timeCounter = new Counter(Settings.levelTime);
            _timeCounter.Start();
            ApplyGameSettings();
            gameData.Initialize(((PlayerConfig)playerConfig).lives );
            gameData.ResetData();
            DbManager.Initialize(Settings.dbConfig);
            Initialize();
            StateManager.UpdateGameState(Settings.startState);

            //- subscribe to events
            GameState.OnGameOver += OnGameOver;
            GameState.OnLevelWin += OnLevelWin;
        }

        private void ApplyGameSettings()
        {
            // --------------------------------------------------------------------------------
            // Setup game settings
            // --------------------------------------------------------------------------------
            Screen.sleepTimeout = SleepTimeout.NeverSleep;
            Application.targetFrameRate = Settings.targetFrameRate;
            Cursor.visible = Settings.cursorVisibility;
            
            //keep frame rate & monitor refresh rate in sync
            if(!Settings.vSync) return;
            if (Screen.currentResolution.refreshRate > 65) {
                QualitySettings.vSyncCount = Mathf.FloorToInt(Screen.currentResolution.refreshRate/60f);
            }
        }
        
        private void Initialize()
        {
            // --------------------------------------------------------------------------------
            // Initialize the User Interfaces
            // --------------------------------------------------------------------------------
            uiManager.Initialize();
            
            // --------------------------------------------------------------------------------
            // Create the Game State then add the necessary systems (the order is matter !)
            // --------------------------------------------------------------------------------
            _gameState = new GameState(States.Game, gameData);
            _gameState
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
        
        public static T GetSystem<T>() where T : GameSystem
            => _instance._gameState.GetSystem<T>();

        public States targetState;

        [Button]
        private void SetState()
        {
            StateManager.UpdateGameState(targetState);
        }
        
        private void OnLevelWin(int inScore)
        {
            AudioSystem.Play(AudioLabel.LevelWin);
            _gameState.Pause();
            _timeCounter.Stop();
            var bonus = Mathf.RoundToInt(_timeCounter.TimeRatio * Settings.timeBonus);
            gameData.Score = bonus + inScore;

            StateManager.UpdateGameState(
                            gameData.Score > gameData.Leaderboards.lastRankScore 
                            ? States.Winning 
                            : States.Menu
                        );
        }

        private void OnGameOver()
        {
            AudioSystem.Play(AudioLabel.GameOver);
            StateManager.UpdateGameState(States.Menu);
        }

        private void FirstStart()
        {
            _isRun = true;
            _gameState.Start();
            if(Settings.gameLoop == GameLoop.Coroutine) 
                StartCoroutine(Tick());
        }

        [Button]
        private void SubmitPlayer()
        {
            //fil with fake player
            LocalLeaderboardSystem.SubmitLeaderboard(Data.Register.idToken, 0, new SubmitData()
            {
                name = "Atef Sassi",
                score = 1989,
                tournamentId = Data.Leaderboards.@group.tournamentId
            });
            
            LocalLeaderboardSystem.SubmitLeaderboard(Data.Register.idToken, 0, new SubmitData()
            {
                name = "Haifa Sayeh",
                score = 589,
                tournamentId = Data.Leaderboards.@group.tournamentId
            });
        }
        
        [Button("New Game")]
        private void NewGame()
        {
            _isRun = true;
            _gameState.NewGame();
        }
        
        [Button]
        private void Pause()
        {
            _isRun = false;
            if(Settings.gameLoop == GameLoop.Coroutine) 
                StopCoroutine(Tick());
            _gameState.Pause();
        }
        
        [Button]
        private void Resume()
        {
            _isRun = true;
            if (Settings.gameLoop == GameLoop.Coroutine)
                StartCoroutine(Tick());
            _gameState.Resume();
        }
        
        
        private readonly Dictionary<float, WaitForSeconds>  _waitDictionary = new Dictionary<float, WaitForSeconds>();
        private static Dictionary<string, IEnumerator>      _coroutinesMap = new Dictionary<string, IEnumerator>();
        
        /// <summary>
        /// None-allocating WaitForSeconds
        /// </summary>
        private WaitForSeconds GetWait(float inTime)
        {
            if (_waitDictionary.TryGetValue(inTime, out var wait)) return wait;
            _waitDictionary[inTime] = new WaitForSeconds(inTime);
            return _waitDictionary[inTime];
        }

        
        public static string LateExecute(Action inAction, float inTime)
        {
            var operationId = Guid.NewGuid().ToString("N"); //generate unique identifier
            var coroutine = _instance.WaitAndExecute(inAction, inTime, operationId);
            _coroutinesMap.Add(operationId, coroutine);
            _instance.StartCoroutine(coroutine);
            return operationId;
        }

        public static string ExecuteCoroutine(IEnumerator inCoroutine)
        {
            var operationId = Guid.NewGuid().ToString("N"); //generate unique identifier
            _coroutinesMap.Add(operationId, inCoroutine);
            _instance.StartCoroutine(inCoroutine);
            return operationId;
        }

        public static bool CancelExecution(string inOperationId)
        {
            if (string.IsNullOrEmpty(inOperationId) || !_coroutinesMap.ContainsKey(inOperationId)) return false;
            _instance.StopCoroutine(_coroutinesMap[inOperationId]);
            return true;
        }

        private IEnumerator WaitAndExecute(Action inAction, float inTime, string inOperationId)
        {
            yield return GetWait(inTime);
            inAction?.Invoke();
            _coroutinesMap.Remove(inOperationId);
        }
        
        
        private void Update()
        {
            if(!_isRun || Settings.gameLoop != GameLoop.Update) return;
            StateManager.Tick(Time.deltaTime);
            _timeCounter.Tick(Time.deltaTime);
        }

        private IEnumerator Tick()
        {
            while (_isRun)
            {
                StateManager.Tick(Time.deltaTime);
                _timeCounter.Tick(Time.deltaTime);
                yield return null;
            }
        }

        public static void StartNewGame()
        {
            AudioSystem.Play(AudioLabel.NewGame);
            StateManager.UpdateGameState(States.Game);
            if (_instance._isFirstRun)
            {
                _instance.FirstStart();
                _instance._isFirstRun = false;
                return;
            }
            _instance._isRun = true;
            _instance._gameState.NewGame();
        }

        public static void PauseGame()
        {
            _instance.Pause();
            StateManager.UpdateGameState(States.Pause);
        }
        
        public static void ResumeGame()
        {
            _instance.Resume();
            StateManager.UpdateGameState(States.Game);
        }
    }
}