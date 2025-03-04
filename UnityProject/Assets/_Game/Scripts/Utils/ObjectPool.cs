using System.Collections.Generic;
using UnityEngine;

namespace _Game.Utils
{
    public class ObjectPool<T> where T : MonoBehaviour
    {
        private Queue<T> _pool;
        private T _prefab;
        private Transform _parent;
        private int _initialSize;
        private int _currentSize;

        public ObjectPool(T prefab, int initialSize, Transform parent = null)
        {
            _prefab = prefab;
            _initialSize = initialSize;
            _parent = parent;
            _pool = new Queue<T>();
            _currentSize = 0;

            ExpandPool(_initialSize);
        }

        private void ExpandPool(int amount)
        {
            for (int i = 0; i < amount; i++)
            {
                T obj = Object.Instantiate(_prefab,null);
                obj.gameObject.SetActive(false);
                _pool.Enqueue(obj);
                _currentSize++;
            }
        }

        public T Get()
        {
            if (_pool.Count == 0) ExpandPool(1);

            T obj = _pool.Dequeue();
            obj.gameObject.SetActive(true);
            return obj;
        }

        public void Return(T obj)
        {
            obj.gameObject.SetActive(false);
            obj.transform.position = _parent.position;
            _pool.Enqueue(obj);
        }
    }
}