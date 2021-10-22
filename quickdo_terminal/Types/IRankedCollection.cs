using System.Collections;
using System.Collections.Generic;

namespace quickdo_terminal.Types
{
    public interface IRankedCollection<T> : ICollection<T>, IList<T> where T : IRankable
    {
        void Add(T item, int rank);
    }
}