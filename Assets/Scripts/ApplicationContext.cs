﻿using System;
using System.Collections.Generic;
using Common.Debug;
using Debug;
using Match3.Board;
using Match3.Screen;
using UnityEngine;

namespace Common
{
    [Serializable]
    public struct Settings
    {
        public BoardParam BoardParam;
        public AnimationSettings Animation;
        public IconSpriteModel[] SpriteModels;
    }
    
    [Serializable]
    public struct BoardParam
    {
        public int Row;
        public int Column;
        public float TileSize;
        public int Capacity => Row * Column;
    }

    [Serializable]
    public struct AnimationSettings
    {
        public float ShiftingTime;
        public float DestroySpeed;
        
    }
    public class ApplicationContext : MonoBehaviour
    {
        [SerializeField] public Settings Settings;
        [SerializeField] private InputSystem _inputSystem;
        [SerializeField] private GameplayScreen _gameplayScreen;
        [SerializeField] private Slot _slotPrefab;
        [SerializeField] private Tile _tilePrefab;

        private Dictionary<Type, object> _registeredTypes;

        public void Construct()
        {
            _registeredTypes = new Dictionary<Type, object>();
            
            RegisterInstance<IStateMachine<BaseState>>(new StateMachine<BaseState>());
            RegisterInstance<IScreenService>(new ScreenService());

            RegisterInstance<IPool<Slot>>(new SlotPool(_slotPrefab, Settings.BoardParam.Capacity));
            RegisterInstance<IPool<Tile>>(new TilePool(_tilePrefab, Settings.BoardParam.Capacity));

            RegisterInstance<IInputSystem>(_inputSystem);
            RegisterInstance<IBoardModel>(new BoardModel());
            RegisterInstance<IBoardService>(new BoardService(this));
            RegisterInstance<IValidator>(new SlotValidator(this));
            RegisterInstance<IClearingSlots>(new ClearingBoard(this));
            RegisterInstance<IMatching, ICheckResult>(new FindMatching(this));
            RegisterInstance<IShiftingSlots>(new ShiftingSlots(this));
            
            RegisterInstance<IGameplay>(new Gameplay(this));
            RegisterInstance<IBoardRenderer>(new BordRenderer(this));

            InitialiseScreenService();
            InitialiseGameplayStateMachine();
        }

        private void InitialiseScreenService()
        {
            var screenService = this.Resolve<IScreenService>();
            
            screenService.Register<GameplayScreen>(_gameplayScreen);
        }

        public T Resolve<T>()
        {
            return (T) _registeredTypes[typeof(T)];
        }

        private void RegisterInstance<T>(T instance)
        {
            _registeredTypes.Add(typeof(T), instance);
        }
        
        private void RegisterInstance<T0,T1>(object instance)
        {
            _registeredTypes.Add(typeof(T0), instance);
            _registeredTypes.Add(typeof(T1), instance);
        }

        private void InitialiseGameplayStateMachine()
        {
            var stateMachine = this.Resolve<IStateMachine<BaseState>>();
            
            stateMachine.Register<InitialiseState>(new InitialiseState(this));
            stateMachine.Register<GameplayState>(new GameplayState(this));
            stateMachine.Register<ResetState>(new ResetState(this));
        }
    }
}