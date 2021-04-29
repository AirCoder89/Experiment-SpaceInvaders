using System.Collections.Generic;
using System.ComponentModel;
using Core;
using UnityEngine;

namespace Models.SystemConfigs
{
    public enum InputBehaviour
    {
        None = 0,
        Shoot = 1,
        Move = 2
    }

    public enum ButtonState
    {
        Button,
        ButtonDown,
        ButtonUp
    }
    
    [CreateAssetMenu(menuName = "Game/System Config/Input System Config")]
    public class InputConfig : SystemConfig
    {
        [SerializeField] private string shoot = "Shoot";
        [SerializeField] private string movement = "Horizontal";
        
        private Dictionary<InputBehaviour, string> _playersBehavioursList;
        
        public void Initialize()
        {
            _playersBehavioursList = new Dictionary<InputBehaviour, string>();
            AssignBehaviour(InputBehaviour.Shoot, this.shoot);
            AssignBehaviour(InputBehaviour.Move, movement);
        }

        public string GetBehaviourAxis(InputBehaviour inBehaviour)
        {
            if (!_playersBehavioursList.ContainsKey(inBehaviour))
                throw new WarningException($"Behaviours List doesn't contains the given behaviour {inBehaviour}");
            
            return _playersBehavioursList[inBehaviour];
        }
        
        private void AssignBehaviour(InputBehaviour inBehaviour, string inBehaviourName)
        {
            if(string.IsNullOrEmpty(inBehaviourName)) return;
            _playersBehavioursList.Add(inBehaviour, inBehaviourName);
        }
    }
}