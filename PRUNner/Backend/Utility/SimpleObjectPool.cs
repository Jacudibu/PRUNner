using System;
using System.Collections.Generic;

namespace PRUNner.Backend.Utility
{
    public class SimpleObjectPool<T> where T : new()
    {
        private readonly Queue<T> _pool = new();
        private readonly Action<T> _onReturn;
        
        public SimpleObjectPool(Action<T> onReturn)
        {
            _onReturn = onReturn;
        }

        public T Get()
        {
            return _pool.Count == 0 
                ? new T() 
                : _pool.Dequeue();
        }

        public void Return(T instance)
        {
            _onReturn.Invoke(instance);
            _pool.Enqueue(instance);
        }
    }
}