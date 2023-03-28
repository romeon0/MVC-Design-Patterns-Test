using System;
using System.Collections.Generic;

namespace UnityGame.MVC
{
    public interface IObjectStorage<T> where T: class
    {
        public void Initialize();
        public V GetValue<V>() where V : class, T;
        public T GetValue(Type type);
    }
}

