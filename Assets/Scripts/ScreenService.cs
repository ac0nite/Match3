using System;
using System.Collections.Generic;

namespace Common
{
    public interface IScreenService
    {
        void Register<T>(object state);
        T Get<T>();
    }
    
    public class ScreenService : IScreenService
    {
        private Dictionary<Type, object> _screens;

        public ScreenService()
        {
            _screens = new Dictionary<Type, object>();
        }

        public void Register<T>(object state)
        {
            _screens.Add(typeof(T), state);
        }

        public T Get<T>()
        {
            return (T) _screens[typeof(T)];
        }
    }
}