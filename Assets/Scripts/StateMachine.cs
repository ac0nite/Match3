using System;
using System.Collections.Generic;
using Common.Debug;
using Cysharp.Threading.Tasks;
using Debug;
using Match3.Environment;
using Match3.Screen;

namespace Common
{
    public interface IState
    {
        void Enter();
        void Exit();
    }
    
    public abstract class BaseState : IState
    {
        protected readonly ApplicationContext _context;
        protected BaseState(ApplicationContext context)
        {
            _context = context;
        }
        public abstract void Enter();
        public abstract void Exit();
    }

    public class InitialiseState : BaseState
    {
        private readonly IBoardRenderer _boardRenderer;
        private readonly IStateMachine<BaseState> _stateMachine;

        public InitialiseState(ApplicationContext context) : base(context)
        {
            _boardRenderer = context.Resolve<IBoardRenderer>();
            _stateMachine = context.Resolve<IStateMachine<BaseState>>();
        }

        public override void Enter()
        {
            UnityEngine.Debug.Log($"INITIALISE STATE");
            
            _boardRenderer.Create();
            //_boardRenderer.RendererRandom();
            _boardRenderer.RendererConfig();
            
            _stateMachine.Next<GameplayState>();
        }

        public override void Exit()
        {
        }
    }
    
    public class GameplayState : BaseState
    {
        private readonly IGameplay _gameplay;
        private readonly ICheckResult _checkResult;
        private readonly IStateMachine<BaseState> _stateMachine;
        private readonly GameplayScreen _gameplayScreen;
        private readonly IAnimationEnvironment _animationEnvironment;

        public GameplayState(ApplicationContext context) : base(context)
        {
            _gameplay = context.Resolve<IGameplay>();
            _checkResult = context.Resolve<ICheckResult>();
            _stateMachine = context.Resolve<IStateMachine<BaseState>>();
            _gameplayScreen = context.Resolve<IScreenService>().Get<GameplayScreen>();
            _animationEnvironment = context.Resolve<IAnimationEnvironment>();
        }

        public override void Enter()
        {
            UnityEngine.Debug.Log($"GAMEPLAY STATE");
            
            _checkResult.RoundCompletedEvent += RoundCompleted;
            _gameplayScreen.EndButtonPressedEvent += RoundCompleted;
            
            _gameplay.SetActive(true);
            _gameplayScreen.Show();
            _animationEnvironment.Play();
        }

        private void RoundCompleted()
        {
            _stateMachine.Next<ResetState>();
        }

        public override void Exit()
        {
            _gameplay.SetActive(false);
            _gameplayScreen.Hide();
            _animationEnvironment.Stop();
            
            _checkResult.RoundCompletedEvent -= RoundCompleted;
            _gameplayScreen.EndButtonPressedEvent -= RoundCompleted;
        }
    }
    
    public class ResetState : BaseState
    {
        private readonly IBoardRenderer _boardRenderer;
        private readonly IStateMachine<BaseState> _stateMachine;
        private readonly IClearingSlots _clearing;

        public ResetState(ApplicationContext context) : base(context)
        {
            _clearing = context.Resolve<IClearingSlots>();
            _boardRenderer = context.Resolve<IBoardRenderer>();
            _stateMachine = context.Resolve<IStateMachine<BaseState>>();
        }

        public override void Enter()
        {
            UnityEngine.Debug.Log($"RESET STATE");
            CleanBoardAsync();
        }

        public override void Exit()
        {
        }

        private async void CleanBoardAsync()
        {
            UnityEngine.Debug.Log($"CleanBoardAsync begin");
            
            await UniTask.Delay(1000);
            await _clearing.AllExecuteAsync();
            
            UnityEngine.Debug.Log($"boardRenderer.Clear");
            _boardRenderer.Clear();
            await UniTask.DelayFrame(120);
            
            UnityEngine.Debug.Log($"CleanBoardAsync end");
            
            _stateMachine.Next<InitialiseState>();
        }
    }


    public interface IStateMachine<T> where T : IState
    {
        void Register<T>(object state);
        void Next<T>();
    }
    public class StateMachine<T> : IStateMachine<T> where T : IState
    {
        private Dictionary<Type, object> _states;
        private IState _currentState;
        
        public StateMachine()
        {
            _states = new Dictionary<Type, object>();
        }

        public void Register<T>(object state)
        {
            _states.Add(typeof(T), state);
        }

        public void Next<T>()
        {
            if(_currentState != null)
                _currentState.Exit();
            
            _currentState = (IState) _states[typeof(T)];
            _currentState.Enter();
        }
    }
}