using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SearchAlgorithmsLib
{
    public interface IQueue<T>
    {
        T Top();
        T Dequeue();
        bool Contains();
        void Enqueue(T item);
        int Count();
    }
}
