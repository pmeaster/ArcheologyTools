using System.Collections;
using System.Collections.Generic;

namespace FractalSource.Energy
{
    public class Information : IEnumerable<Quantum>
    {
        public IEnumerator<Quantum> GetEnumerator()
        {
            throw new System.NotImplementedException();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}