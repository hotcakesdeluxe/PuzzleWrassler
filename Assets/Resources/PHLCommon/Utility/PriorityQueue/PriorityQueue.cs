using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PHL.Common.Utility
{
    public class PriorityQueue<T> where T : IQueueable
    {
        private T[] _items;
        private int _currentItemCount;
        public int Count => _currentItemCount;
        public PriorityQueue(int maxHeapSize)
        {
            _items = new T[maxHeapSize];
        }

        public void Enqueue(T item)
        {
            item.QueueIndex = _currentItemCount;
            _items[_currentItemCount] = item;
            SortUp(item);
            _currentItemCount++;
        }

        public T Dequeue()
        {
            T firstItem = _items[0];
            _currentItemCount--;
            _items[0] = _items[_currentItemCount];
            _items[0].QueueIndex = 0;
            SortDown(_items[0]);
            return firstItem;
        }

        public void UpdateElement(T item)
        {
            SortUp(item);
        }

        public bool Contains(T item) => Equals(_items[item.QueueIndex], item);

        void SortDown(T item)
        {
            while (true)
            {
                int childIndexLeft = item.QueueIndex * 2 + 1;
                int childIndexRight = item.QueueIndex * 2 + 2;
                int swapIndex = 0;

                if (childIndexLeft < _currentItemCount)
                {
                    swapIndex = childIndexLeft;

                    if (childIndexRight < _currentItemCount)
                    {
                        if (_items[childIndexLeft].CompareTo(_items[childIndexRight]) < 0)
                        {
                            swapIndex = childIndexRight;
                        }
                    }

                    if (item.CompareTo(_items[swapIndex]) < 0)
                    {
                        Swap(item, _items[swapIndex]);
                    }
                    else
                    {
                        return;
                    }

                }
                else
                {
                    return;
                }
            }
        }

        void SortUp(T item)
        {
            int parentIndex = (item.QueueIndex - 1) / 2;

            while (true)
            {
                T parentItem = _items[parentIndex];
                if (item.CompareTo(parentItem) > 0)
                {
                    Swap(item, parentItem);
                }
                else
                {
                    break;
                }
                parentIndex = (item.QueueIndex - 1) / 2;
            }
        }

        void Swap(T itemA, T itemB)
        {
            _items[itemA.QueueIndex] = itemB;
            _items[itemB.QueueIndex] = itemA;
            int itemAIndex = itemA.QueueIndex;
            itemA.QueueIndex = itemB.QueueIndex;
            itemB.QueueIndex = itemAIndex;
        }
    }
}