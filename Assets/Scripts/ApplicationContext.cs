using System;
using System.Collections.Generic;
using Common.Debug;
using Debug;
using Match3.Board;
using UnityEngine;

namespace Common
{
    [Serializable]
    public struct Settings
    {
        public BoardParam BoardParam;
        public IconSpriteModel[] SpriteModels;
        public float ShiftingAnimationTime;
    }
    
    [Serializable]
    public struct BoardParam
    {
        public int Row;
        public int Column;
        public float TileSize;
        public int Capacity => Row * Column;
    }
    public class ApplicationContext : MonoBehaviour
    {
        [SerializeField] public Settings Settings;
        [SerializeField] private InputSystem _inputSystem;
        [SerializeField] private Slot _slotPrefab;
        [SerializeField] private Tile _tilePrefab;

        private Dictionary<Type, object> _registeredTypes;

        public void Construct()
        {
            _registeredTypes = new Dictionary<Type, object>();
            
            RegisterInstance<IPool<Slot>>(new SlotPool(_slotPrefab, Settings.BoardParam.Capacity));
            RegisterInstance<IPool<Tile>>(new TilePool(_tilePrefab, Settings.BoardParam.Capacity));

            RegisterInstance<IInputSystem>(_inputSystem);
            RegisterInstance<IBoardModel>(new BoardModel());
            RegisterInstance<IBoardService>(new BoardService(this));
            RegisterInstance<IValidator>(new SlotValidator(this));
            RegisterInstance<ICleaningBoard>(new ClearingBoard(this));
            RegisterInstance<IMatching>(new FindMatching(this));
            RegisterInstance<IShiftingSlots>(new ShiftingSlots(this));
            
            RegisterInstance<IGameplay>(new Gameplay(this));
            RegisterInstance<IBoardRenderer>(new BordRenderer(this));
        }
        
        public T Resolve<T>()
        {
            return (T) _registeredTypes[typeof(T)];
        }

        private void RegisterInstance<T>(T instance)
        {
            _registeredTypes.Add(typeof(T), instance);
        }
        
        private void RegisterPoolInstance<TPool,T>(T instance)
        {
            _registeredTypes.Add(typeof(T), instance);
        }
    }
}