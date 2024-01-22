﻿using System;
using System.Collections.Generic;
using Board;
using Board.Settings;
using Common;
using Match3.Board;
using Match3.Environment;
using Match3.General;
using Match3.Models;
using Match3.Pool;
using Match3.Screen;
using Match3.Services;
using Match3.StateMachine;
using UnityEngine;
using UnityEngine.Serialization;

namespace Match3.Context
{
    [Serializable]
    public struct Settings
    {
        [FormerlySerializedAs("BoardParam")] public BoardSettings boardSettings;
        public AnimationSettings Animation;
        public TileModel[] SpriteModels;
        public BoardConfig BoardConfig;
    }
    
    [Serializable]
    public struct BoardSettings
    {
        public BoundsBoardSettingsSO Bounds;
        public BoardSize Size;
    }

    [Serializable]
    public struct AnimationSettings
    {
        public float ShiftingTime;
    }

    [Serializable]
    public struct BoardConfig
    {
        public List<TextAsset> Param;
    }
    public class ApplicationContext : MonoBehaviour
    {
        [SerializeField] public Settings Settings;
        [SerializeField] private InputSystem _inputSystem;
        [SerializeField] private GameplayScreen _gameplayScreen;
        [SerializeField] private AnimationEnvironment _animationEnvironment;
        [SerializeField] private Slot _slotPrefab;
        [SerializeField] private Camera _camera;

        private Dictionary<Type, object> _registeredTypes;

        public void Construct()
        {
            _registeredTypes = new Dictionary<Type, object>();
            
            RegisterInstance<IStateMachine<BaseState>>(new StateMachine<BaseState>());
            RegisterInstance<IScreenService>(new ScreenService());
            
            RegisterInstance<IPool<Slot>>(new SlotPool(_slotPrefab, 200));

            RegisterInstance<IInputSystem>(_inputSystem);
            RegisterInstance<IBoardModel>(new BoardModel());
            RegisterInstance<IBoardService>(new BoardService(this, _camera));
            RegisterInstance<IValidator>(new SlotValidator(this));
            RegisterInstance<IClearing>(new ClearingBoard(this));
            RegisterInstance<IMatching, ICheckResult>(new FindMatching(this));
            RegisterInstance<IShifting>(new Shifting(this));
            RegisterInstance<IUpdateBoard>(new UpdateBoard(this));
            
            RegisterInstance<IGameplay>(new Gameplay(this));
            RegisterInstance<IBoardRenderer>(new BoardRenderer(this));
            RegisterInstance<IAnimationEnvironment>(_animationEnvironment);

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