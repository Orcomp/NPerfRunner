namespace NPerf.Fixture.IList
{
    using System.Collections;
    using System.Collections.Generic;
    using System.Linq;

    /// <summary>
    /// This is an implementation of System.Collection.Generic.IList&lt;T&gt; interface
    /// just to give an example.
    /// When loading NPerf.Fixture.IList assembly on NPerfRunner you will see
    /// this class available for testing.
    /// </summary>
    /// <typeparam name="T">The type of the elements of the list.</typeparam>
    public class AnIListImplementation<T> : IList<T>
    {
        private List<T> objects;

        private List<int> indexes;

        public AnIListImplementation()
        {
            this.objects = new List<T>();
            this.indexes = new List<int>();
        }

        public IEnumerator<T> GetEnumerator()
        {
            var list = this.indexes.Select(index => this.objects[index]).ToList();
            return list.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return this.GetEnumerator();
        }

        public void Add(T item)
        {
            if (this.objects.Contains(item))
            {
                var index = this.objects.IndexOf(item);
                this.indexes.Add(index);
            }
            else
            {
                this.objects.Add(item);
                this.indexes.Add(this.objects.Count - 1);
            }
        }

        public void Clear()
        {
            this.objects.Clear();
            this.indexes.Clear();
        }

        public bool Contains(T item)
        {
            return this.objects.Contains(item);
        }

        public void CopyTo(T[] array, int arrayIndex)
        {
            array = new T[this.indexes.Count];
            var i = 0;
            foreach (var index in this.indexes)
            {
                array[i++] = this.objects[index];
            }
        }

        public bool Remove(T item)
        {
            var index = this.IndexOf(item);
            if (index == -1)
            {
                return false;
            }

            this.indexes.RemoveAt(index);
            return true;
        }

        public int Count
        {
            get
            {
                return this.indexes.Count;
            }
        }

        public bool IsReadOnly
        {
            get
            {
                return false;
            }
        }

        public int IndexOf(T item)
        {
            var i = 0;
            foreach (var index in this.indexes)
            {
                if (this.objects[index].Equals(item))
                {
                    return i;
                }
                i++;
            }
            return -1;
        }

        public void Insert(int index, T item)
        {
            var i = this.objects.IndexOf(item);
            if (i == -1)
            {
                this.objects.Add(item);
                this.indexes.Insert(index, this.objects.Count - 1);
            }
            else
            {
                this.indexes.Insert(index, i);
            }
        }

        public void RemoveAt(int index)
        {
            var i = this.indexes[index];
            this.indexes.RemoveAt(index);
            if (!this.indexes.Contains(i))
            {
                this.objects.RemoveAt(i);
            }
        }

        public T this[int index]
        {
            get
            {
                return this.objects[this.indexes[index]];
            }

            set
            {
                if (this.objects.Contains(value))
                {
                    this.indexes[index] = this.IndexOf(value);
                }
                else
                {
                    this.objects.Add(value);
                    this.indexes[index] = this.objects.Count - 1;
                }
            }
        }
    }
}
