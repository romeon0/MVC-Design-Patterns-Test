using System;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;

namespace UnityGame.MVC
{
    public interface IValueGetter<T>
    {
        public T GetValue(Type type);
    }

    public class FieldInjector
    {
        public void InjectReferences<T>(object obj, IValueGetter<T> valueGetter)
        {
            List<FieldInfo> views = GetFieldsOfType<T>(obj);
            foreach (var field in views)
            {
                T value = valueGetter.GetValue(field.FieldType);
                field.SetValue(obj, value);
                Debug.Log($"[FieldInjector] Injected View. Field:{field.Name}; Value:{value.GetType()}");
            }
        }

        private List<FieldInfo> GetFieldsOfType<T>(object obj)
        {
            List<FieldInfo> result = new List<FieldInfo>();
            Type neededFieldType = typeof(T);
            Type objectType = obj.GetType();
            foreach (FieldInfo field in objectType.GetFields(BindingFlags.Instance | BindingFlags.NonPublic))
            {
                bool isValid = field.FieldType == neededFieldType || neededFieldType.IsAssignableFrom(field.FieldType);
                if (isValid)
                {
                    result.Add(field);
                }
            }
            return result;
        }
    }
}
