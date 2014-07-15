namespace PMDToolkit.Core
{
    using System;
    using System.Collections.Generic;
    using System.Runtime.CompilerServices;
    using System.Text;

    public class LRUCache<K, V>
    {
        #region Fields

        Dictionary<K, LinkedListNode<LRUCacheItem<K, V>>> cacheMap = new Dictionary<K, LinkedListNode<LRUCacheItem<K, V>>>();
        int capacity;
        LinkedList<LRUCacheItem<K, V>> lruList = new LinkedList<LRUCacheItem<K, V>>();
        Object lockObject = new object();

        #endregion Fields

        public delegate void ItemRemovedEvent(V value);
        public ItemRemovedEvent OnItemRemoved;

        #region Constructors

        public LRUCache(int capacity) {
            this.capacity = capacity;
        }

        #endregion Constructors

        #region Methods

        //[MethodImpl(MethodImplOptions.Synchronized)]
        public void Add(K key, V val) {
            lock (lockObject) {
                if (cacheMap.Count >= capacity) {
                    RemoveFirst();
                }
                LRUCacheItem<K, V> cacheItem = new LRUCacheItem<K, V>(key, val);
                LinkedListNode<LRUCacheItem<K, V>> node = new LinkedListNode<LRUCacheItem<K, V>>(cacheItem);
                lruList.AddLast(node);
                cacheMap.Add(key, node);
            }
        }

        //[MethodImpl(MethodImplOptions.Synchronized)]
        public V Get(K key) {
            lock (lockObject) {
                LinkedListNode<LRUCacheItem<K, V>> node;
                if (cacheMap.TryGetValue(key, out node)) {
                    V value = node.Value.value;

                    lruList.Remove(node);
                    lruList.AddLast(node);
                    return value;
                }
                return default(V);
            }
        }

        protected void RemoveFirst() {
            // Remove from LRUPriority
            LinkedListNode<LRUCacheItem<K, V>> node = lruList.First;
            lruList.RemoveFirst();
            // Remove from cache
            cacheMap.Remove(node.Value.key);
            OnItemRemoved(node.Value.value);
        }

        public void Clear()
        {
            while (lruList.Count > 0)
            {
                RemoveFirst();
            }
        }

        #endregion Methods
    }

    internal class LRUCacheItem<K, V>
    {
        #region Fields

        public K key;
        public V value;

        #endregion Fields

        #region Constructors

        public LRUCacheItem(K k, V v) {
            key = k;
            value = v;
        }

        #endregion Constructors
    }
}