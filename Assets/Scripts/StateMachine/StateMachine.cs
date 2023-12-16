using System;
using System.Collections.Generic;

namespace Match3.StateMachine
{
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