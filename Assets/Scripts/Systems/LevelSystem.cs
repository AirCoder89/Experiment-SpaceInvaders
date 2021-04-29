using System;
using Core;
using Models.SystemConfigs;
using UnityEngine;
using Utils;
using Views;

namespace Systems
{
    public class LevelSystem : GameSystem
    {
        public static event Action OnHitVerticalEdges;
        private LevelConfig _config;
        private Transform _holder;
        
        public LevelSystem(SystemConfig inConfig): base(inConfig)
        {
            if(inConfig != null) _config = inConfig as LevelConfig;

            var levelState = new LevelState("[ Level State ]");
            
            _holder = new GameObject("Edges").transform;
            _holder.SetParent(levelState.transform);
            _holder.SetPositionAndRotation(Vector3.zero, Quaternion.identity);
        }
        
        public override void Start()
        {
            CreateVerticalEdges();
            CreateHorizontalEdges();
        }
        
        private void CreateVerticalEdges()
        {
            //- Left Edge
            var leftEdge = new GameObject("LeftEdge") {layer = LayersList.Other};
            leftEdge.transform.SetParent(_holder);
            leftEdge.transform.position = _config.leftEdge.position;
            var leftHitEdge = leftEdge.AddComponent<HitDetector>();
            leftHitEdge.Initialize(_config.leftEdge.size, false, _config.leftEdge.targetLayer);
            leftHitEdge.onHitEnter += o => OnHitVerticalEdges?.Invoke();
            
            //- Right Edge
            var rightEdge = new GameObject("RightEdge") {layer = LayersList.Other};
            rightEdge.transform.SetParent(_holder);
            rightEdge.transform.position = _config.rightEdge.position;
            var rightHitEdge = rightEdge.AddComponent<HitDetector>();
            rightHitEdge.Initialize(_config.rightEdge.size, false, _config.rightEdge.targetLayer);
            rightHitEdge.onHitEnter += o => OnHitVerticalEdges?.Invoke();
        }

        private void CreateHorizontalEdges()
        {
            //- Top Edge
            var topEdge = new GameObject("TopEdge") {layer = LayersList.Other};
            topEdge.transform.SetParent(_holder);
            topEdge.transform.position = _config.topEdge.position;
            topEdge.transform.localScale = _config.topEdge.size;
            topEdge.AddComponent<BoxCollider>().isTrigger = true;
            
            //- Bottom Edge
            var bottomEdge = new GameObject("BottomEdge") {layer = LayersList.Other};
            bottomEdge.transform.SetParent(_holder);
            bottomEdge.transform.position = _config.bottomEdge.position;
            bottomEdge.transform.localScale = _config.bottomEdge.size;
            bottomEdge.AddComponent<BoxCollider>().isTrigger = true;
        }
    }
}