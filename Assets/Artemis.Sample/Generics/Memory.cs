using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Artemis.Sample.Generics
{
    public class Memory<T> : IEnumerable<T>
    {
        private readonly List<T> _items = new();
        private readonly List<DateTime> _expiration = new();
        public int Count => _items.Count;
        public T this[int index] => _items[index];

        public void Add(T item, DateTime expiration)
        {
            _items.Add(item);
            _expiration.Add(expiration);
        }

        public void RemoveExpiredItems()
        {
            for (int i = _items.Count - 1; i >= 0; i--)
            {
                if (DateTime.Now >= _expiration[i])
                {
                    _items.RemoveAt(i);
                    _expiration.RemoveAt(i);
                }
            }
        }

        public IEnumerator<T> GetEnumerator()
        {
            return _items.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        public void Clear()
        {
            _items.Clear();
            _expiration.Clear();
        }
    }
}