using System;
using System.Collections.Generic;
using System.Linq;
using Application.Settings;
using Board;
using Board.Config;
using Board.Settings;
using Common;
using ID;
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
        public BoardSettings boardSettings;
        public AnimationSettings Animation;
        public TileModel[] SpriteModels;
        public BoardSerializeConfigs boardSerializeConfigs;
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
    public struct BoardSerializeConfigs
    {
        public List<TextAsset> Param;
    }

    public interface IContext
    {
        T Resolve<T>();
        T[] ResolveAll<T>();
    }

    public class ApplicationContext : MonoBehaviour, IContext
    {
        [SerializeField] private GameplaySettingsSO GameplaySettings;
        [SerializeField] public Settings Settings;
        [SerializeField] private InputSystem _inputSystem;
        [SerializeField] private GameplayScreen _gameplayScreen;
        [SerializeField] private AnimationEnvironment _animationEnvironment;
        [SerializeField] private Slot _slotPrefab;
        [SerializeField] private Camera _camera;

        private Dictionary<Type, List<ObjectData>> _registeredObjects;
        private class ObjectData
        {
            public Type ImplementedType;
            public object ImplementedObject;
            public object[] Params;

            public ObjectData(Type type, params object[] args)
            {
                ImplementedType = type;
                ImplementedObject = null;
                Params = args;
            }

            public ObjectData(object implemented)
            {
                ImplementedType = null;
                ImplementedObject = implemented;
                Params = null;
            }

            public bool IsImplemented => ImplementedObject != null;

            public void CreateInstance(IContext context)
            {
                var args = Params.Length == 0
                    ? new object[] {context}
                    : new object[] {context}.Concat(Params).ToArray();
                
                ImplementedObject ??= Activator.CreateInstance(ImplementedType, args);

                Params = null;
            }
        }

        public void Construct()
        {
            _registeredObjects = new Dictionary<Type, List<ObjectData>>();

            RegisterInstance<IStateMachine<BaseState>, StateMachine<BaseState>>();
            RegisterInstance<ScreenService>();

            RegisterInstance<IPool<Slot>, SlotPool>(_slotPrefab, 200);
            
            RegisterComponent<IInputSystem>(_inputSystem);
            RegisterInstance<BoardModel>();
            RegisterInstance<IBoardService, BoardService>(_camera);
            RegisterInstance<SlotValidator>();
            RegisterInstance<ClearingBoard>();
            RegisterInstance<FindMatching>();
            RegisterInstance<Shifting>();
            RegisterInstance<UpdateBoard>();
            RegisterInstance<ScoreModel>();
            RegisterInstance<BoardConfigs>();
            
            RegisterInstance<Gameplay>();
            RegisterInstance<BoardRenderer>();
            RegisterComponent<IAnimationEnvironment>(_animationEnvironment);
            
            InitialiseScreenService();
            InitialiseGameplayStateMachine();
        }

        private void InitialiseScreenService()
        {
            var screenService = this.Resolve<IScreenService>();

            screenService.Register<GameplayScreen>(_gameplayScreen);
        }

        public void RegisterComponent<TInterface>(Component component)
        {
            Register(typeof(TInterface), new ObjectData(component));
        }

        public void RegisterInstance<TInterface, TImplement>(params object[] args) where TImplement : class
        {
            Register(typeof(TInterface), new ObjectData(typeof(TImplement), args));
        }

        public void RegisterInstance<TImplement>(params object[] args) where TImplement : class
        {
            var type = typeof(TImplement);
            var interfaces = type.GetInterfaces();
            var data = new ObjectData(type, args);
            
            if (interfaces.Length > 0)
            {
                for (int i = 0; i < interfaces.Length; i++)
                    Register(interfaces[i], data);
            }
            else
                Register(type, data);
        }

        private void Register(Type type, ObjectData data)
        {
            if(_registeredObjects.ContainsKey(type))
                _registeredObjects[type].Add(data);
            else
                _registeredObjects[type] = new List<ObjectData>() {data};
        }

        public T Resolve<T>()
        {
            var type = typeof(T);
            if (!_registeredObjects.ContainsKey(type))
                throw new ArgumentException($"The requested interface [{typeof(T).Name}] is not binded!");

            var objects = _registeredObjects[type];
            if (!objects.First().IsImplemented)
            {
                foreach (var data in objects)
                    data.CreateInstance(this);
            }

            return (T) objects.First().ImplementedObject;
        }

        public T[] ResolveAll<T>()
        {
            var type = typeof(T);
            if (!_registeredObjects.ContainsKey(type))
                return new T[]{};
            
            var objects = _registeredObjects[type];
            if (!objects.First().IsImplemented)
            {
                foreach (var data in objects)
                    data.CreateInstance(this);
            }

            return objects.Select(data => (T) data.ImplementedObject).ToArray();
        }

        public void Build()
        {
            var objects = ResolveAll<IInitializable>();
            foreach (var data in objects)
                data.Initialise();
        }

        private void InitialiseGameplayStateMachine()
        {
            var stateMachine = this.Resolve<IStateMachine<BaseState>>();

            stateMachine.Register<InitialiseState>(new InitialiseState(this));
            stateMachine.Register<GameplayState>(new GameplayState(this));
            stateMachine.Register<ResetState>(new ResetState(this));
        }

        public void Dispose()
        {
            var objects = ResolveAll<IDisposable>();
            foreach (var data in objects)
                data.Dispose();
        }
    }

    public interface IInitializable
    {
        void Initialise();
    }

    public interface IDisposable
    {
        void Dispose();
    }
}