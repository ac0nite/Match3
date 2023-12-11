using System;
using System.Collections.Generic;
using UnityEngine;

namespace Common
{
    public class ApplicationContext : MonoBehaviour
    {
        [SerializeField] private InputSystem _inputSystem;
        [SerializeField] private GameplayBoard _gameplayBoard;
        [SerializeField] private IconSpriteModel[] _icons;
        
        private Dictionary<Type, object> _registeredTypes;

        public void Construct()
        {
            _registeredTypes = new Dictionary<Type, object>();
            
            RegisterInstance<IInputSystem>(_inputSystem);
            RegisterInstance<IGameplayBoardRenderer>(_gameplayBoard);
            RegisterInstance<IconSpriteModel[]>(_icons);
        }
        
        public T Resolve<T>()
        {
            return (T) _registeredTypes[typeof(T)];
        }

        private void RegisterInstance<T>(T instance)
        {
            _registeredTypes.Add(typeof(T), instance);
        }
    }
}