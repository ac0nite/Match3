using System;
using System.Collections.Generic;
using UnityEngine;

namespace Common
{
    public interface IPool<T>
    {
        T Get();
        void Put(T item);
    }
    public abstract class BasePool<T> : IPool<T>
    {
        protected Queue<T> _pool;
        private readonly int _capacity;

        protected BasePool(int capacity)
        {
            _capacity = capacity;
        }
        
        protected void Initialise()
        {
            _pool = new Queue<T>(_capacity);
            for (int i = 0; i < _capacity; i++)
                _pool.Enqueue(Create());
        }

        public abstract T Create();
        public abstract T Configure(T item);

        public T Get()
        {
            return Configure(_pool.Dequeue());
        }

        public virtual void Put(T item)
        {
            _pool.Enqueue(item);
        }
    }
}