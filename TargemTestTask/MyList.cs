using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TargemTestTask
{
    class MyList<T> : IList<T>
    {
        private T[] innerArray = new T[1];
        private int count;
        private bool hadChanged;

        public int Count => count;
        public bool IsReadOnly => true;

        public IEnumerator<T> GetEnumerator()
        {
            hadChanged = false;
            var enumeratorCounter = 0;
            var privateCount = count;
            foreach (var e in innerArray)
            {
                if (hadChanged)
                    throw new InvalidOperationException();
                if (enumeratorCounter >= privateCount)
                    break;
                yield return e;
                enumeratorCounter++;
            }
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        public void Add(T item)
        {
            if (innerArray.Length == count)
            {
                var tempArray = new T[innerArray.Length * 2];
                for (int i = 0; i < innerArray.Length; i++)
                {
                    tempArray[i] = innerArray[i];
                }
                innerArray = tempArray;
            }
            innerArray[count] = item;
            count++;
            hadChanged = true;
        }

        public void Clear()
        {
            innerArray = new T[1]; 
            count = 0;
            hadChanged = true;
        }

        public bool Contains(T item)
        {
            return innerArray.Contains(item);
        }

        public void CopyTo(T[] array, int arrayIndex)
        {
            for (int i = 0; i < count; i++)
            {
                array[arrayIndex + i] = innerArray[i];
            }
        }

        public bool Remove(T item)
        {
            for (int i = 0; i < count; i++)
            {
                if (innerArray[i].Equals(item))
                {
                    RemoveAt(i);
                    return true;
                }
            }
            hadChanged = true;

            return false;
        }


        public int IndexOf(T item)
        {
            for (int i = 0; i < count; i++)
            {
                if (innerArray[i].Equals(item))
                    return i;
            }

            return -1;
        }

        public void Insert(int index, T item)
        {
            if (index > count)
                throw new ArgumentOutOfRangeException();
            var tempItem = item;
            for (; index < count; index++)
            {
                tempItem = innerArray[index];
                innerArray[index] = item;
                item = tempItem;
            }
            Add(item);
            hadChanged = true;
        }

        public void RemoveAt(int index)
        {
            if (index >= count)
                throw new ArgumentOutOfRangeException();
            for (; index < count - 1; index++)
            {
                innerArray[index] = innerArray[index + 1];
            }
            hadChanged = true;
        }

        public T this[int index]
        {
            get
            {
                if (index >= count)
                    throw new ArgumentOutOfRangeException();
                return innerArray[index];
            }
            set
            {
                if (index >= count)
                    throw new ArgumentOutOfRangeException();
                innerArray[index] = value;
                hadChanged = true;
            }
        }
    }
}
