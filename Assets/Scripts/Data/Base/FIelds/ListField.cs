using System;
using System.Collections.Generic;

namespace Game.Data
{
    public class ListField<T> : DataField<List<T>>
    {
        public event Action<T> OnItemAdded;
        
        public event Action<T> OnItemRemoved;
        
        public void Add(T item)
        {
            _value ??= new();

            _value.Add(item);
            
            OnItemAdded?.Invoke(item);
        }

        public void Remove(T item)
        {
            if (_value == null) return;

            _value.Remove(item);
            
            OnItemRemoved?.Invoke(item);
        }
    }
}