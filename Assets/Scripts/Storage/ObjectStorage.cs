using System;
using System.Collections.Generic;
using UnityEngine;

namespace UnityGame.MVC
{
    public class ObjectStorage<T> : MonoBehaviour, IObjectStorage<T> where T: class
    {
        private Dictionary<Type, T> _values = new Dictionary<Type, T>();

        public void Initialize()
        {
            for (int a = 0; a < transform.childCount; ++a)
            {
                T value = transform.GetChild(a).GetComponent(typeof(T)) as T;
                if (value != null)
                {
                    Debug.Log("[ObjectStorage] Found Value. Type: " + value.GetType());
                    _values[value.GetType()] = value;
                }
            }
        }

        public V GetValue<V>() where V : class, T
        {
            return (V)GetValue(typeof(V));
        }

        public T GetValue(Type type)
        {
            //Debug.Log("[ObjectStorage] Type: " + type);
            foreach (var pair in _values)
            {
                if (type == pair.Key || type.IsAssignableFrom(pair.Key))
                {
                    //Debug.Log("[ObjectStorage] Found! Type: " + pair.Key);
                    return pair.Value;
                }
            }
            Debug.LogError("[ObjectStorage] Not Found! Type: " + type);
            return default;
        }
    }
}

