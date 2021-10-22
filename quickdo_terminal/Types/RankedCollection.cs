using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace quickdo_terminal.Types
{
    public class RankedCollection<T> : IRankedCollection<T> where T : IRankable
    {
        List<T> items = new();

        public T this[int index]
        {
            get
            {
                return items[index];
            }
            set
            {
                items[index] = value;
            }
        }

        public int Count => items.Count;

        public bool IsReadOnly => false;

        public RankedCollection()
        {
        }

        public RankedCollection(IList collection)
        {
            foreach(var item in collection)
            {
                items.Add((T)item);
            }
        }

        public void Add(T item)
        {
            item.Rank = items.Count + 1;
            items.Add(item);
        }

        public void Add(T item, int rank)
        {
            Add(item);

            if (rank == 0)
                return;

            while (item.Rank != rank)
            {
                Promote(item.Rank);
            }
        }

        public void Promote(int rank)
        {
            var promotee = items.SingleOrDefault(i => i.Rank == rank);
            if (promotee == null)
                return;

            var demotee = items.SingleOrDefault(i => i.Rank == rank - 1);
            if (promotee == null)
                return;

            promotee.Rank = demotee.Rank;
            demotee.Rank = rank;
        }

        public void Demote(int rank)
        {
            var demotee = items.SingleOrDefault(i => i.Rank == rank);
            if (demotee == null)
                return;

            var promotee = items.SingleOrDefault(i => i.Rank == rank + 1);
            if (promotee == null)
                return;

            promotee.Rank = demotee.Rank;
            demotee.Rank = rank;
        }

        public void Clear()
        {
            items.Clear();
        }

        public bool Contains(T item)
        {
            return items.Contains(item);
        }

        public void CopyTo(T[] array, int arrayIndex)
        {
            items.CopyTo(array, arrayIndex);
        }

        public IEnumerator<T> GetEnumerator()
        {
            return items.GetEnumerator();
        }

        public bool Remove(T item)
        {
            return items.Remove(item);
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return items.GetEnumerator();
        }

        public int IndexOf(T item)
        {
            return items.IndexOf(item);
        }

        public void Insert(int index, T item)
        {
            items.Insert(index, item);
        }

        public void RemoveAt(int index)
        {
            items.RemoveAt(index);
        }
    }
}