using System;

namespace TinyHeap
{
    public class Heap<T>
    {
        public Heap() : this((l, r) => (l as IComparable).CompareTo(r) > 0)
        {
        }

        public Heap(Func<T, T, bool> comparer)
        {
            _comparer = comparer;
            _elems = new T[16];
        }

        // Returns true if (left, right) are in the right order.
        private Func<T, T, bool> _comparer;
        private T[] _elems;
        private int _count;

        public T Top => _count > 0 ? _elems[0] : throw new InvalidOperationException();
        public int Count => _count;

        public void Push(T elem)
        {
            if (_elems.Length <= _count)
                Array.Resize(ref _elems, _elems.Length * 2);            

            _elems[_count] = elem;
            int index = _count;
            _count++;

            while (index > 0 && !_comparer(_elems[ParentIndex(index)], _elems[index]))
            {
                Swap(ref _elems[ParentIndex(index)], ref _elems[index]);
                index = ParentIndex(index);
            }
        }

        public T Pop()
        {
            if (_count == 0) throw new InvalidOperationException();
            T elem = _elems[0];

            Swap(ref _elems[0], ref _elems[_count - 1]);
            _count--;

            int index = 0;
            while (LeftIndex(index) < _count || RightIndex(index) < _count)
            {
                int maxchild = LeftIndex(index);
                if (_count > maxchild + 1 && !_comparer(_elems[maxchild], _elems[maxchild + 1]))
                    maxchild++;
                
                if (_comparer(_elems[index], _elems[maxchild]))
                    break;
                
                Swap(ref _elems[index], ref _elems[maxchild]);
                index = maxchild;
            }

            return elem;
        }

        private void Swap(ref T left, ref T right)
        {
            T tmp = left;
            left = right;
            right = tmp;
        }

        private int LeftIndex(int i) => 2*i + 1;
        private int RightIndex(int i) => 2*i + 2;
        private int ParentIndex(int i) => (i - (2-i%2))/2;
    }
}
