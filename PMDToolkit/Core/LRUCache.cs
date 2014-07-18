/*The MIT License (MIT)

Copyright (c) 2014 PMU Staff

Permission is hereby granted, free of charge, to any person obtaining a copy
of this software and associated documentation files (the "Software"), to deal
in the Software without restriction, including without limitation the rights
to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
copies of the Software, and to permit persons to whom the Software is
furnished to do so, subject to the following conditions:

The above copyright notice and this permission notice shall be included in
all copies or substantial portions of the Software.

THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN
THE SOFTWARE.
*/


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