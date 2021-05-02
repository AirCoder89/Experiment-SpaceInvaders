using Systems;
using Core;
using Models;
using Models.SystemConfigs;
using UnityEngine;
using UnityEngine.UI;

namespace UI.HUD
{
    [RequireComponent(typeof(Canvas))]
    public class HUDCanvas : MonoBehaviour
    {
        public static HUDCanvas instance;
        
        [SerializeField] private GameData gameData;
        [SerializeField] private Text scoreTxt;
        [SerializeField] private Button pauseBtn;
        [SerializeField] private LivesHandler lives;

        private Canvas _canvas;

        public Canvas RootCanvas
        {
            get
            {
                if (_canvas == null) _canvas = GetComponent<Canvas>();
                return _canvas;
            }
        }
        private void Awake()
        {
            instance = this;
            GameData.OnDataChanged += UpdateHud;
            pauseBtn.onClick.AddListener((() =>
            {
                Main.PauseGame();
                AudioSystem.Play(AudioLabel.Click);
            }));
        }

        private void OnDestroy()
        {
            GameData.OnDataChanged -= UpdateHud;
        }

        private void UpdateHud()
        {
            if(gameData == null) return;
            scoreTxt.text = gameData.score.ToString("000");
            lives.UpdateLives(gameData.lives);
        }
    }
}