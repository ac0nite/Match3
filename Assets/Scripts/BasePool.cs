using System.Collections.Generic;

namespace Common
{
    public interface IPool<T>
    {
        T Get();
        void Return(T item);
    }
    public abstract class BasePool<T> : IPool<T>
    {
        private Queue<T> _pool;
        protected BasePool(int capacity)
        {
            _pool = new Queue<T>(capacity);
            for (int i = 0; i < capacity; i++)
                _pool.Enqueue(Create());
        }

        public abstract T Create();
        public abstract T Configure(T item);

        public T Get()
        {
            return Configure(_pool.Dequeue());
        }

        public void Return(T item)
        {
            _pool.Enqueue(item);
        }
    }
}